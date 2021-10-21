using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject endMenuPanelGO, restartMenuPanelGO, deathPanelGO, pauseMenuPanelGO;
    [SerializeField]
    MusicManager musicManager;
    [SerializeField]
    RoverEnergy roverEnergy;
    [SerializeField]
    MapGenerator mapGenerator;

    bool unlockOnResume = false;
    [SerializeField]
    SaveLoadHandler saveLoadHandler;

    private void OnEnable()
    {
        if (mapGenerator != null)
        {
            mapGenerator.GenerationFailed += OnGenerationFailed;
        }
        Rover.EndingReached += OnEndingReached;
        Rover.RoverDead += OnRoverDead;
    }

    private void OnDisable()
    {
        Rover.EndingReached -= OnEndingReached;
        if (mapGenerator != null)
        {
            mapGenerator.GenerationFailed -= OnGenerationFailed;
        }
        Rover.RoverDead -= OnRoverDead;
    }
    private void OnDestroy()
    {
        Rover.EndingReached -= OnEndingReached;
        if (mapGenerator != null)
        {
            mapGenerator.GenerationFailed -= OnGenerationFailed;
        }
        Rover.RoverDead -= OnRoverDead;
    }

    void Start()
    {
        endMenuPanelGO?.SetActive(false);
        restartMenuPanelGO?.SetActive(false);
        deathPanelGO?.SetActive(false);
        pauseMenuPanelGO?.SetActive(false);
        StopAllCoroutines();
    }



    private void OnEndingReached()
    {
        GameManager.LockInput();
        StartCoroutine(LevelEndCoroutine());

    }

    private void OnGenerationFailed()
    {
        GameManager.LockInput();
        restartMenuPanelGO?.SetActive(true);
    }

    private void OnRoverDead()
    {
        deathPanelGO?.SetActive(true);
    }

    IEnumerator LevelEndCoroutine()
    {
        musicManager.PlayVictoryMusic();
        yield return new WaitForSeconds(2f);
        roverEnergy.RefillEnergy();
        yield return new WaitForSeconds(1f);
        endMenuPanelGO?.SetActive(true);
        yield break;
    }


    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        deathPanelGO?.SetActive(false);
        saveLoadHandler.Save();
        Score.UpdateScore(0);
        Score.SetBestScore(saveLoadHandler.Load().bestScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        if (pauseMenuPanelGO == null) return;
        if (!GameManager.InputLocked()) unlockOnResume = true;
        else unlockOnResume = false;
        GameManager.LockInput();
        Time.timeScale = 0;
        pauseMenuPanelGO.SetActive(true);
    }

    public void ResumeGame()
    {
        if (pauseMenuPanelGO == null) return;
        if (unlockOnResume) GameManager.UnlockInput();
        
        Time.timeScale = 1;
        pauseMenuPanelGO.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        saveLoadHandler?.Save();
        SceneManager.LoadScene(0);
    }
}
