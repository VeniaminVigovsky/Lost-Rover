using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathNode : MonoBehaviour
{
    Rover rover;
    float minimumDistance = 0.25f;
    Sprite selectedNodeSprite;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        selectedNodeSprite = sr.sprite;
        sr.sprite = null;
    }

    public void SetRover()
    {
        rover = GameManager.GetRover();
    }

    public void OnMouseDown()
    {

        if (GameManager.InputLocked()) return;

        else if (RoverIsUnderThisTile())
        {
            GameManager.SetMakingPath(true);
            AddToNodeList();
            SetSprite(selectedNodeSprite);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
        }
        else if (!RoverIsUnderThisTile())
        {

            SetSprite(null);
            rover?.ClearNodeList();
            GameManager.SetMakingPath(false);

        }

        //check if Rover is on this tile AND if input is unlocked
        //if not - error message
        //if yes:
        //static class bool isMakingPath = true
        //Add()

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, rover.transform.position) < 2)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
        }
    }

    public void OnMouseEnter()
    {
        //if static class bool isMakingPath == true:
        //if Rover.PathNodeList.Contain(this)
        //Remove() and Rover.RemoveEveryNodeAfter(this)
        //else Add()
        if (GameManager.IsMakingPath())
        {
            SetSprite(selectedNodeSprite);
            AddToNodeList();
        }
    }


    public void OnMouseUp()
    {
        if (GameManager.IsMakingPath())
        {
            rover?.StartMovement();
            GameManager.SetMakingPath(false);
        }


        //if static class bool isMakingPath == true:
        //Rover.StartMovement
        //static class bool isMakingPath == false

    }

    private bool RoverIsUnderThisTile()
    {
        if (rover != null)
        {
            return Vector2.Distance(rover.transform.position, transform.position) <= minimumDistance;
        }
        else return false;
        
    }

    private void AddToNodeList()
    {
        rover.AddToNodeList(this);
    }

    public void SetSprite(Sprite spr = null)
    {
        sr.sprite = spr;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }
}
