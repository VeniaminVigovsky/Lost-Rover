using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTile : MonoBehaviour
{
    Rover rover;
    SpriteRenderer sr;
    float minDist = 2f;
    float fullTransparentDistance = 1f;
    float dist;    
    bool lockAlpha;
    float tempDist;
    bool isUsingRadar = false;

    public delegate void RadarUsedHandler();
    public static event RadarUsedHandler RadarUsedStarted;

    public static event RadarUsedHandler RadarUsedEnded;


    private void OnEnable()
    {
        Rover.RoverDead += HideGrid;        
    }

    private void OnDisable()
    {
        Rover.RoverDead -= HideGrid;
    }

    private void OnDestroy()
    {
        Rover.RoverDead -= HideGrid;
    }


    void Start()
    {
        rover = GameManager.GetRover();
        sr = GetComponent<SpriteRenderer>();
        tempDist = Vector2.Distance(rover.gameObject.transform.position, transform.position);
        dist = Vector2.Distance(rover.gameObject.transform.position, transform.position);
        if (dist <= fullTransparentDistance)
        {

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            lockAlpha = true;
        }
        else if (dist <= minDist && !lockAlpha)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, dist / minDist);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsingRadar == false)
        {
            dist = Vector2.Distance(rover.gameObject.transform.position, transform.position);

            if (dist < tempDist)
            {
                if (dist <= fullTransparentDistance)
                {

                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
                    lockAlpha = true;
                }
                else if (dist <= minDist && !lockAlpha)
                {
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, dist / minDist);

                }

                tempDist = dist;
            }
        }



    }

    public void MakeTransparent()
    {
        //var color = sr.color;
        StartCoroutine(AlphaToTransparentAndBack(sr.color.a));

    }

    private IEnumerator AlphaToTransparentAndBack(float tileAlpha)
    {
        
        isUsingRadar = true;
        GameManager.LockInput();
        RadarUsedStarted?.Invoke();
        var newAlpha = sr.color.a;
        while (sr.color.a > 0)
        {
            
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            newAlpha -= 0.2f;
            if (newAlpha < 0) 
            {
                newAlpha = 0;
            }
            yield return new WaitForSeconds(0.05f);
        }

        
        yield return new WaitForSeconds(GameManager.GetTimeToPeak());

        while (sr.color.a < tileAlpha)
        {
            
            
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            newAlpha += 0.2f;
            if (newAlpha > tileAlpha) newAlpha = tileAlpha;
            yield return new WaitForSeconds(0.05f);
        }

        GameManager.UnlockInput();
        isUsingRadar = false;
        RadarUsedEnded?.Invoke();
        StopCoroutine(AlphaToTransparentAndBack(tileAlpha));
    }

    private void HideGrid()
    {
        StopAllCoroutines();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }
}
