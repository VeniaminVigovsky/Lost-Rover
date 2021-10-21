using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRoadTile : MapTile
{
    List<GameObject> rightTiles = new List<GameObject>();
    List<GameObject> leftTiles = new List<GameObject>();

    public GameObject topVerticalEndingTile;
    public GameObject bottomVerticalEndingTile;
 
    void Awake()
    {
        foreach(var tile in nextTiles)
        {
            if (tile.GetComponent<MapTile>() is HorizontalRoadTile)
            {
                rightTiles.Add(tile);
                leftTiles.Add(tile);
            }
            else if (tile.GetComponent<MapTile>() is TurnRightDownRoad || tile.GetComponent<MapTile>() is TurnRightUpRoad)
            {
                rightTiles.Add(tile);
            }
            else if (tile.GetComponent<MapTile>() is TurnLeftDownRoad || tile.GetComponent<MapTile>() is TurnLeftUpRoad)
            {
                leftTiles.Add(tile);
            }
        }

        if (nextTileOffset[0].x < 0 && nextTileOffset[1].x > 0)
        {
            tilesForOffset.Add(nextTileOffset[0], leftTiles);
            tilesForOffset.Add(nextTileOffset[1], rightTiles);
        }
        else if (nextTileOffset[0].x > 0 && nextTileOffset[1].x < 0)
        {
            tilesForOffset.Add(nextTileOffset[0], rightTiles);
            tilesForOffset.Add(nextTileOffset[1], leftTiles);            
        }
    }

    public override GameObject GetHorizontalEndingTile()
    {
        return base.GetHorizontalEndingTile();
    }

    public override GameObject GetVerticalEndingTile(Vector3 currentPosition)
    {
        if (currentPosition.y > 0)
        {
            return topVerticalEndingTile;
        }
        else if (currentPosition.y < 0)
        {
            return bottomVerticalEndingTile;
        }
        else return null;
    }

}
