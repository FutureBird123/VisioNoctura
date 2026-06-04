using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public AudioSource clickSound;
    private int sceneIndexToLoad;
    public void PlayClickSound()
    {
        clickSound.Play();
    }
    public void loadNewGame(int index)
    {
        PlayClickSound();
        sceneIndexToLoad = index;

        Invoke("LoadLevel", 0.2f);
    }
    
    public void loadMainMenu()
    {
        PlayClickSound();
        Invoke("MainMenuFunc", 0.2f);
    }

    private void MainMenuFunc()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void LoadLevel()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    public void ExitGame()
    {
        PlayClickSound();
        Invoke("ExitApp", 0.2f);
    }

    private void ExitApp()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
}