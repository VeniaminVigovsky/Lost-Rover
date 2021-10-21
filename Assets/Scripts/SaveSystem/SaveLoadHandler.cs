using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadHandler : MonoBehaviour
{

    public void Save()
    {
        int scoreToSave = Score.currentScore > Score.bestScore ? Score.currentScore : Score.bestScore;

        SaveData data = new SaveData(scoreToSave);

        SaveSystem.SaveData(data);
    }

    public SaveData Load()
    {
        return SaveSystem.LoadData();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Save();
    }



}
