using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MenuUi : MonoBehaviour
{
    [SerializeField] TMP_Text notiText;
    [SerializeField] GameObject winUi;
    [SerializeField] GameObject loseUi;
    [SerializeField] GameObject notiUi;
    InputManager inputManager;

    private void Start()
    {
        inputManager = FindAnyObjectByType<InputManager>();
    }

    public void Update()
    {   
        if(SceneManager.GetActiveScene().buildIndex == 0) { return; }
        if (!GameplayManager.instance.isGameOver) return;
        if(inputManager != null) inputManager.enabled = false;
        Time.timeScale = 0f;
        notiUi.SetActive(true);
        if(GameplayManager.instance.isWinMission)
        {
            notiText.text = "You Win";
            winUi.SetActive(true);
        }
        else
        {
            notiText.text = "You Lose";
            loseUi.SetActive(true);
        }
    }

    public void RetryButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}

