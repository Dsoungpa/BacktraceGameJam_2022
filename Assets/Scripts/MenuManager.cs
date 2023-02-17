using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Awake() {
        Time.timeScale = 1f;
    }

    public void StartGame() {
        SceneManager.LoadScene(0);
    }

    public void CloseGame() {
        Application.Quit();
    }
    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

    public void GoToMenu() {
        SceneManager.LoadScene(1);
    }
}
