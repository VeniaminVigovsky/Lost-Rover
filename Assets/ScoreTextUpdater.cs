using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextUpdater : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
        Score.ScoreUpdated += DisplayScore;
    }

    private void Start()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = $"Score: {Score.currentScore}\n\nBest: {Score.bestScore}";
    }

    private void OnDestroy()
    {
        Score.ScoreUpdated -= DisplayScore;
    }

    private void OnDisable()
    {
        Score.ScoreUpdated -= DisplayScore;
    }


}
