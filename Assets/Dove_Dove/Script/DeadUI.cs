using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadUI : MonoBehaviour
{
    public Button RestartButton;
    public Button ExitButtton;

    void Start()
    {
        RestartButton.onClick.AddListener(GameRestart);
        ExitButtton.onClick.AddListener(GameEixt);
    }

    private void GameRestart()
    {
        SceneManager.LoadScene("Map1");
        gameObject.SetActive(false);
    }

    private void GameEixt()
    {
        Application.Quit();
    }
}
