using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargePointPlacer : MonoBehaviour
{
    [SerializeField] 
    GameObject rechargePoint;
    MapGenerator mapGenerator;
    

    private void Awake()
    {
        if (rechargePoint != null)
        {
            rechargePoint.transform.position = new Vector3(-30f, 0f, 0f);
        }
        mapGenerator = GetComponent<MapGenerator>();
    }

    private void OnEnable()
    {
        if (mapGenerator == null)
        {
            mapGenerator = GetComponent<MapGenerator>();
        }
        mapGenerator.GenerationEnded += PlaceRechargePoint;
    }

    private void OnDisable()
    {
        mapGenerator.GenerationEnded -= PlaceRechargePoint;
    }
    private void OnDestroy()
    {
        mapGenerator.GenerationEnded -= PlaceRechargePoint;
    }

    private void Start()
    {

    }

    private void PlaceRechargePoint(Vector3 context)
    {
        if (rechargePoint == null) return;
        
        List<Vector3> tilePositions = mapGenerator.GetOccupiedPositions();        
        int r = Random.Range((int)(tilePositions.Count / 3), tilePositions.Count - 3);        
        rechargePoint.transform.position = tilePositions[r];

    }
}
