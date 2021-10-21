using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public GameObject[] nextTiles;
    public Vector3[] nextTileOffset;
    public Vector3[] brimOffset;
    public Dictionary<Vector3, List<GameObject>> tilesForOffset = new Dictionary<Vector3, List<GameObject>>();
    public GameObject previousTile;
    //public GameObject endingTile = null;
    public GameObject horizontalEndingTile;
    public GameObject verticalEndingTile;

    public virtual void SetPreviousTile(GameObject tile)
    {
        previousTile = tile;
    }

    public GameObject GetPreviousTile()
    {
        return previousTile;
    }

    public virtual GameObject GetHorizontalEndingTile()
    {
        return horizontalEndingTile;
    }

    public virtual GameObject GetHorizontalEndingTile(Vector3 offset)
    {
        return horizontalEndingTile;
    }

    public virtual GameObject GetVerticalEndingTile()
    {
        return verticalEndingTile;
    }

    public virtual GameObject GetVerticalEndingTile(Vector3 currentPosition)
    {
        return verticalEndingTile;
    }
}
