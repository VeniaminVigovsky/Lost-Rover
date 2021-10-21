using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoverEnergy : MonoBehaviour
{
    [SerializeField]
    Image[] energyCells;
    [SerializeField]
    MusicManager musicManager;

    RoverAudioManager roverAudio;

    int maxEnergyCellsCount;
    int currentEnergyCellsCount;


    public delegate void EnergyDrainHandler();
    public event EnergyDrainHandler EnergyDrained;

    void Start()
    {
        roverAudio = GetComponentInChildren<RoverAudioManager>();
        maxEnergyCellsCount = 4;
        currentEnergyCellsCount = maxEnergyCellsCount;

        if (energyCells.Length > 0)
        {
            foreach (var cell in energyCells)
            {
                cell.gameObject.SetActive(true);
            }
        }
    }

    public int GetCurrentEnergyCellsCount()
    {
        return currentEnergyCellsCount;
    }

    public void UseEnergyCell()
    {
        currentEnergyCellsCount--;
        foreach (var cell in energyCells)
        {
            cell.gameObject.SetActive(false);
        }
        for (int i = 0; i < currentEnergyCellsCount; i++)
        {
            energyCells[i].gameObject.SetActive(true);
        }

        if (currentEnergyCellsCount <= 0)
        {
            EnergyDrained?.Invoke();
        }
    }

    public void RefillEnergy()
    {
        roverAudio.PlayRefillEnergySound();
        currentEnergyCellsCount = maxEnergyCellsCount;
        foreach (var cell in energyCells)
        {
            cell.gameObject.SetActive(true);
        }
    }

    public void SetEnergy(int cellCount)
    {
        currentEnergyCellsCount = cellCount;
        foreach (var cell in energyCells)
        {
            cell.gameObject.SetActive(false);
        }
        for (int i = 0; i < currentEnergyCellsCount; i++)
        {
            energyCells[i].gameObject.SetActive(true);
        }
    }

}
