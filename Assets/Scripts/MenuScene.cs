using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    SaveLoadHandler saveLoadHandler;
    public void Awake()
    {
        saveLoadHandler = GetComponent<SaveLoadHandler>();
    }

    public void Start()
    {
        Score.UpdateScore(0);
        SaveData data = saveLoadHandler.Load();
        Score.SetBestScore(data.bestScore);
    }


    public void StartGame()
    {

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
