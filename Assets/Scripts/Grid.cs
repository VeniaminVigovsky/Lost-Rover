using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    UtilityTile[] utilityTiles;
    PathNode[] nodes;
    [SerializeField]
    RoverEnergy roverEnergy;
    [SerializeField]
    MusicManager musicManager;



    private float radarUseTime;

    private float radarUseDuration;
    private void Awake()
    {
        utilityTiles = GetComponentsInChildren<UtilityTile>();
        nodes = GetComponentsInChildren<PathNode>();        
    }

    private void Start()
    {
        foreach (var node in nodes)
        {
            node?.SetRover();
        }
        radarUseDuration = GameManager.GetTimeToPeak();

        radarUseTime = Time.time;

    }



    public void ShowTransparentGrid()
    {
        if (GameManager.InputLocked()) return;

        if (radarUseDuration + radarUseTime > Time.time) return;
        radarUseTime = Time.time;

        roverEnergy?.UseEnergyCell();
        musicManager?.PlayRadarSound();

        foreach(var utilityTile in utilityTiles)
        {
            utilityTile?.MakeTilesTransparent();
        }
    }
}
