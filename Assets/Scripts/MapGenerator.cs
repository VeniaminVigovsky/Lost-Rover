using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapTile[] walkableTiles;
    public MapTile[] horizontalBrims;
    public MapTile[] verticalBrims;

    [SerializeField]
    private GameObject startingVerticalTile;
    [SerializeField]
    private GameObject startingHorizontalTile;
    [SerializeField]
    private GameObject endingPoint;

    private MapTile currentTile;
    private string borderType;
    private float topBorder = 2f;
    private float bottomBorder = -4f;
    private float leftBorder = -7.75f;
    private float rightBorder = 8.25f;
    GameObject tileToInstantiate;

    private int verticalEndingID = 1;
    private int horizontalEndingID = 2;
    

    static Vector3 startingPosition = new Vector3(-5.75f, 2f, 0f);

    Vector3[] endingPositions = 
    {   
        new Vector3(8.25f, 0f, 0f), 
        new Vector3(8.25f, -2f, 0f), 
        new Vector3(6.25f, 2f, 0f),
        new Vector3(4.25f, 2f, 0f),
        new Vector3(6.25f, -4f, 0f),
        new Vector3(4.25f, -4f, 0f)
    };

    Vector3 endingTilePosition = new Vector3(8.25f, 0f, 0f);

    Dictionary<Vector3, Vector3> endingStartingTiles = new Dictionary<Vector3, Vector3>()
    {
        [new Vector3(8.25f, 0f,0f)] = new Vector3(-7.75f, 0f, 0f),
        [new Vector3(8.25f, -2f, 0f)] = new Vector3(-7.75f, -2f, 0f),
        [new Vector3(6.25f, 2f, 0f)] = new Vector3(-3.75f, -4f, 0f),
        [new Vector3(4.25f, 2f, 0f)] = new Vector3(-5.75f, -4f, 0f),
        [new Vector3(6.25f, -4f, 0f)] = new Vector3(-3.75f, 2f, 0f),
        [new Vector3(4.25f, -4f, 0f)] = new Vector3(-5.75f, 2f, 0f),
    };

    Vector3 nextPosition;
    Vector3 offset;    

    List<Vector3> occupiedPositions = new List<Vector3>();

    int iterrationCount = 0;
    

    public delegate void GenerationEndHandler(Vector3 endingPosition);
    public event GenerationEndHandler GenerationEnded;

    public delegate void GenerationFailHandler();
    public event GenerationFailHandler GenerationFailed;

    public void OnEnable()
    {
        GenerationEnded += OnGenerationEnded;

    }

    public void OnDisable()
    {
        GenerationEnded -= OnGenerationEnded;
    }

    private void Start()
    {

        if (endingPoint == null)
        {
            GenerationFailed?.Invoke();
            return;
        }

        endingPoint.transform.position = new Vector3(-20f, 0f, 0f);

        endingTilePosition = endingPositions[Random.Range(0, endingPositions.Length)];

        GameObject gO;
        if (startingPosition.x < -7f)
        {
            gO = Instantiate(startingHorizontalTile);
            
        }
        else
        {
            gO = Instantiate(startingVerticalTile);
        }

        currentTile = gO.GetComponent<MapTile>();
        currentTile.transform.position = startingPosition;
        occupiedPositions.Add(currentTile.transform.position);
        GenerateNextTile();

    }

    private void GenerateNextTile()
    {
        iterrationCount++;
        if (iterrationCount > 200)
        {            
            GenerationFailed?.Invoke();
            return;
        }
        //1) get offset
        //if (currentTile == startingTile)
        //{
        //    offset = startingTile.nextTileOffset[1];
        //}
        //else
        //{
            
        //}
        offset = GetOffset(currentTile);

        //2) get nextPosition
        nextPosition = currentTile.transform.position + offset;

        //3) check nextPosition for Occupied and CrossBorder
        if (IsNextOccupied(nextPosition) || IsNextCrossedBorder(nextPosition))
        {
            //if Any true =  check if another offset is present:
            if (HasAnotherOffset(currentTile))
            {
                //true change offset and nextPosition
                offset = -offset;
                nextPosition = currentTile.transform.position + offset;
            }
            else
            {
                //false ? : SetPrevious etc
                SetPreviousMapTileAsCurrent(currentTile);
                return;
            }

        }      

        //if All false = move to the next step
        //4) check nextPosition for isNexttoOccupied and CrossBorder
        if (IsNextCloseToOccupied(nextPosition, currentTile.transform.position) || IsNextCrossedBorder(nextPosition))
        {
            //if Any true = SetPreviousMapTileAsCurrent(), GenerateNextTile()
            SetPreviousMapTileAsCurrent(currentTile);
            return;
        }        
        //if All false = move to the next step
        //5) check nextPosition for OnBorder
        
        if (IsNextOnBorder(nextPosition, out borderType))
        {
            //if true:
            //6a) check nextPosition for IsEngingTile()
            if (IsNextEngingTile(nextPosition))
            {

                //1 check if it's horizontal Ending
                if (GetEndingID(nextPosition) == horizontalEndingID)
                {
                    //If Horizontal Ending:
                    //Check if current is Vertical

                    if (currentTile is VerticalRoadTile)
                    {
                        tileToInstantiate = currentTile.GetHorizontalEndingTile(offset);
                    }
                    else
                    {
                        tileToInstantiate = currentTile.GetHorizontalEndingTile();
                    }

                }
                else if (GetEndingID(nextPosition) == verticalEndingID)
                {
                    //IF IT"S NOT HORIZONTAL ENDING
                    //Check if current is Horizontal

                    if (currentTile is HorizontalRoadTile)
                    {
                        //if it IS:
                        //GetVerticalEnding(currentposition)
                        tileToInstantiate = currentTile.GetVerticalEndingTile(nextPosition);
                    }
                    else
                    {
                        //if it IS NOT:
                        //GetVerticalEnding()
                        //check if null
                        tileToInstantiate = currentTile.GetVerticalEndingTile();
                    }


                    //if NULL go back
                    //if NOT NULL instantiate
                }
                else
                {
                    SetPreviousMapTileAsCurrent(currentTile);
                    return;
                }

                if (tileToInstantiate != null)
                {
                    InstantiateEndingTile();
                    return;
                }
                else
                {
                    SetPreviousMapTileAsCurrent(currentTile);
                    return;
                }







            }
            else
            {
                //if IsEngingTile() is false: 
                //check if OnAngle
                if (IsNextOnAngle(nextPosition))
                {
                    //if true = go to previous
                    SetPreviousMapTileAsCurrent(currentTile);
                    return;
                }
                //    * check if current.Type IsBorderType()
                if (IsCurrentBorderType())
                {
                    //if true = check if current.options has this tile
                    foreach(var tileOption in currentTile.tilesForOffset[offset])
                    {
                        if (tileOption.GetComponent<MapTile>() is TurnDownRightRoad ||
                            tileOption.GetComponent<MapTile>() is TurnUpRightRoad ||
                            tileOption.GetComponent<MapTile>() is TurnLeftDownRoad ||
                            tileOption.GetComponent<MapTile>() is TurnRightDownRoad ||
                            tileOption.GetComponent<MapTile>() is TurnRightUpRoad)
                        {                            
                            tileToInstantiate = tileOption;                            
                            break;
                        }                        
                    }
                    //if has = generate, else: SetPrevious etc.
                    if (tileToInstantiate != null)
                    {
                        InstantiateAndGetNext();
                        return;
                    }

                    else if (tileToInstantiate == null)
                    {
                        SetPreviousMapTileAsCurrent(currentTile);
                        return;
                    }
                }
                else
                {

                    if (HasAvailableOptions(currentTile, offset))
                    {
                        //if more than 0 = get random
                        int r = Random.Range(0, GetAvailableOptionsCount(currentTile, offset));
                        tileToInstantiate = currentTile.tilesForOffset[offset][r];
                        InstantiateAndGetNext();
                        return;

                    }
                    else
                    {

                        SetPreviousMapTileAsCurrent(currentTile);
                        return;
                    }



                }




            }

        }

        else
        {
            //if OnBorder == false
                       
            if (HasAvailableOptions(currentTile, offset))
            {
                //if more than 0 = get random
                int r = Random.Range(0, GetAvailableOptionsCount(currentTile, offset));
                tileToInstantiate = currentTile.tilesForOffset[offset][r];
                InstantiateAndGetNext();
                return;

            }
            else if (!HasAvailableOptions(currentTile, offset))
            {
                //check for available options, if zero = set previous,
                SetPreviousMapTileAsCurrent(currentTile);
                return;
            }
        }
    }

    bool IsNextOccupied(Vector3 position)
    {
        foreach (var occupiedPosition in occupiedPositions)
        {
            if (Vector3.Distance(occupiedPosition, position) < 1.5f)
            {
                
                return true;
            }
        }
        return false;
    }

    bool IsNextCloseToOccupied(Vector3 position, Vector3 currentPosition)
    {
        foreach (var occupiedPosition in occupiedPositions)
        {
            if (Vector3.Distance(occupiedPosition, currentPosition) < 1.5f)
            {
                continue;
            }

            if (Vector3.Distance(position, occupiedPosition) < 2.5f)
            {
                return true;
            }
        }

        return false;
    }

    bool IsNextOnBorder(Vector3 position, out string borderType)
    {
        if (position.y - bottomBorder < 1.5f)
        {
            borderType = "bottomBorder";
            return true;

        }
        else if (topBorder - position.y < 1.5f)
        {
            borderType = "topBorder";
            return true;

        }
        else if (rightBorder - position.x < 1.5f)
        {
            borderType = "rightBorder";
            return true;
        }
        else if (position.x - leftBorder < 1.5f)
        {
            borderType = "leftBorder";
            return true;
        }
        else
        {
            borderType = "noBorder";
            return false;
        }
    }

    bool IsNextCrossedBorder(Vector3 position)
    {
        if (position.y < bottomBorder )
        {            
            return true;

        }
        else if (topBorder < position.y)
        {        
            return true;
        }
        else if (rightBorder < position.x)
        {            
            return true;
        }
        else if (position.x < leftBorder)
        {
            return true;
        }
        else
        {            
            return false;
        }
    }

    bool IsNextEngingTile(Vector3 position)
    {
        if (Vector3.Distance(position, endingTilePosition) < 1.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsNextOnAngle(Vector3 position)
    {
        if (position.y - bottomBorder < 1.5f && rightBorder - position.x < 1.5f)
        {            
            return true;

        }
        else if (topBorder - position.y < 1.5f && rightBorder - position.x < 1.5f)
        {            
            return true;

        }
        if (position.y - bottomBorder < 1.5f && position.x - leftBorder < 1.5f)
        {            
            return true;

        }
        else if (topBorder - position.y < 1.5f && position.x - leftBorder < 1.5f)
        {            
            return true;

        }
        else
        {            
            return false;
        }

    }

    bool IsCurrentBorderType()
    {
        if (borderType == "bottomBorder")
        {
            return currentTile is VerticalRoadTile;
        }
        else if (borderType == "topBorder")
        {
            return currentTile is VerticalRoadTile;
        }
        else if (borderType == "leftBorder")
        {
            return currentTile is HorizontalRoadTile;
        }
        else if (borderType == "rightBorder")
        {
            return currentTile is HorizontalRoadTile;
        }
        else return false;
    }

    bool HasAnotherOffset(MapTile tile)
    {
        return tile.nextTileOffset.Length > 1;
    }

    bool HasAvailableOptions(MapTile tile, Vector3 offset)
    {
        return tile.tilesForOffset[offset].Count > 0;
    }

    int GetAvailableOptionsCount(MapTile tile, Vector3 offset)
    {
        return tile.tilesForOffset[offset].Count;
    }

    public void ClearLastOccupiedPosition()
    {
        occupiedPositions.RemoveAt(occupiedPositions.Count - 1);
    }

    public void SetPreviousMapTileAsCurrent(MapTile tileToDestroy)
    {
        var temp = tileToDestroy.GetPreviousTile().GetComponent<MapTile>();
        if (temp == null) return;        
        var objDestroy = currentTile.gameObject;
        currentTile = temp;
        Destroy(objDestroy);        
        ClearLastOccupiedPosition();        
        GenerateNextTile();
    }

    public Vector3 GetOffset(MapTile tile)
    {
        return tile.nextTileOffset[0];
    }

    public void InstantiateAndGetNext()
    {
        if (tileToInstantiate == null) return;
        currentTile.tilesForOffset[offset].Remove(tileToInstantiate);        
        GameObject gO = Instantiate(tileToInstantiate);
        gO.transform.position = nextPosition;
        gO.GetComponent<MapTile>().SetPreviousTile(currentTile.gameObject);
        occupiedPositions.Add(gO.transform.position);
        tileToInstantiate = null;
        currentTile = gO.GetComponent<MapTile>();        
        GenerateNextTile();
    }

    public void InstantiateEndingTile()
    {
        if (tileToInstantiate == null) return;
        GameObject gO = Instantiate(tileToInstantiate);
        gO.transform.position = nextPosition;
        gO.GetComponent<MapTile>().SetPreviousTile(currentTile.gameObject);
        occupiedPositions.Add(gO.transform.position);
        tileToInstantiate = null;
        //OnEndingTilePlacedEventInvoke
        GenerationEnded?.Invoke(gO.transform.position);
        
    }


    public void OnGenerationEnded(Vector3 endingPosition)
    {

        SetNextStartingPosition(endingPosition);
        
        endingPoint.transform.position = endingPosition;
        
    }



    public void SetNextStartingPosition(Vector3 endingPosition)
    {
        foreach (var keyValuePair in endingStartingTiles)
        {
            if (Vector3.Distance(keyValuePair.Key, endingPosition) < 1.5f)
            {
                startingPosition = endingStartingTiles[keyValuePair.Key];
            }
        }
    }

    public int GetEndingID(Vector3 nextPosition)
    {
        if (nextPosition.x > 7f)
        {
            return horizontalEndingID;
        }
        else return verticalEndingID;
    }


    public List<Vector3> GetOccupiedPositions()
    {
        return occupiedPositions;
    }

    public Vector3 GetStartingPosition()
    {
        return startingPosition;
    }

}
