using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarButtonHandler : MonoBehaviour
{
    [SerializeField]
    Button quitButton, radarButton;

    private void OnEnable()
    {
        EnableButtons();
        Rover.EndingReached += DisableButtons;
        Rover.RoverDead += DisableButtons;
        Rover.RoverJustDead += DisableButtons;
        FogTile.RadarUsedStarted += DisableButtons;
        FogTile.RadarUsedEnded += EnableButtonsDelayed;
    }

    private void OnDisable()
    {
        Rover.EndingReached -= DisableButtons;
        Rover.RoverDead -= DisableButtons;
        Rover.RoverJustDead -= DisableButtons;
        FogTile.RadarUsedStarted -= DisableButtons;
        FogTile.RadarUsedEnded -= EnableButtonsDelayed;
    }

    private void OnDestroy()
    {
        Rover.EndingReached -= DisableButtons;
        Rover.RoverDead -= DisableButtons;
        Rover.RoverJustDead -= DisableButtons;
        FogTile.RadarUsedStarted -= DisableButtons;
        FogTile.RadarUsedEnded -= EnableButtonsDelayed;
    }


    private void DisableButtons()
    {
        quitButton.enabled = false;
        radarButton.enabled = false;
    }

    private void EnableButtons()
    {
        quitButton.enabled = true;
        radarButton.enabled = true;
    }

    private void EnableButtonsDelayed()
    {
        Invoke("EnableButtons", .6f);
    }

}
