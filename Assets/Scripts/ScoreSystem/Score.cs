using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int bestScore { get; private set; }
    public static int currentScore { get; private set; }

    public delegate void ScoreUpdateHandler();
    public static event ScoreUpdateHandler ScoreUpdated;


    public static void UpdateScore(int newScore)
    {
        currentScore = newScore;
        ScoreUpdated?.Invoke();
    }

    public static void SetBestScore(int score)
    {
        bestScore = score;
    }


}
