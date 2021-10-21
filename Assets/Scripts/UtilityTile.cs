using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTile : MonoBehaviour
{
    FogTile[] tiles;
    
    private void Awake()
    {
        tiles = GetComponentsInChildren<FogTile>();        
    }


    public void MakeTilesTransparent()
    {
        foreach(var tile in tiles)
        {
            tile?.MakeTransparent();
        }
    }


}
