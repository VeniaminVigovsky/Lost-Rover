using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRoadTile : MapTile
{
    public override GameObject GetHorizontalEndingTile()
    {
        return base.GetHorizontalEndingTile();
    }

    public override GameObject GetHorizontalEndingTile(Vector3 offset)
    {
        return base.GetHorizontalEndingTile(offset);
    }

    public override GameObject GetVerticalEndingTile()
    {
        return base.GetVerticalEndingTile();
    }

    public override GameObject GetVerticalEndingTile(Vector3 currentPosition)
    {
        return base.GetVerticalEndingTile(currentPosition);
    }

    public override void SetPreviousTile(GameObject tile)
    {
        base.SetPreviousTile(tile);
    }

    // Start is called before the first frame update
    void Awake()
    {
        tilesForOffset.Add(nextTileOffset[0], new List<GameObject>());
        foreach (var tile in nextTiles)
        {
            tilesForOffset[nextTileOffset[0]].Add(tile);
        }
        
    }


}
