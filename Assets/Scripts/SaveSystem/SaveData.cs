using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int bestScore { get; private set; }

    public SaveData(int bestScore)
    {
        this.bestScore = bestScore;
    }
}

