using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRoadTile : MapTile
{
    List<GameObject> upTiles = new List<GameObject>();
    List<GameObject> downTiles = new List<GameObject>();
    public GameObject topHorizontalEndingTile = null;
    public GameObject bottomHorizontalEndingTile = null;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var tile in nextTiles)
        {
            if (tile.GetComponent<MapTile>() is VerticalRoadTile)
            {
                upTiles.Add(tile);
                downTiles.Add(tile);
            }
            else if (tile.GetComponent<MapTile>() is TurnDownRightRoad || tile.GetComponent<MapTile>() is TurnDownLeftRoad)
            {
                downTiles.Add(tile);
            }
            else if (tile.GetComponent<MapTile>() is TurnUpRightRoad || tile.GetComponent<MapTile>() is TurnUpLeftRoad)
            {
                upTiles.Add(tile);
            }
        }

        if (nextTileOffset[0].y < 0 && nextTileOffset[1].y > 0)
        {
            tilesForOffset.Add(nextTileOffset[0], downTiles);
            tilesForOffset.Add(nextTileOffset[1], upTiles);
        }
        else if (nextTileOffset[0].y > 0 && nextTileOffset[1].y < 0)
        {
            tilesForOffset.Add(nextTileOffset[0], upTiles);
            tilesForOffset.Add(nextTileOffset[1], downTiles);
        }

    }

    public override GameObject GetVerticalEndingTile()
    {
        return base.GetVerticalEndingTile();
    }

    public override GameObject GetHorizontalEndingTile(Vector3 offset)
    {
        if (offset.y > 0)
        {
            return topHorizontalEndingTile;
        }
        else if (offset.y < 0)
        {
            return bottomHorizontalEndingTile;
        }
        else return null;
    }

}
