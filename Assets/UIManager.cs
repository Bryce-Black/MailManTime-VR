using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject optionsPanel;
    public GameObject youWinPanel;
    private float totalTime;
    public TextMeshProUGUI totalTimeText;
    public FirstPersonController firstPersonController;
    private AudioSource music;
    private bool gameOver = false;

    private void Start()
    {
        music = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (!gameOver)
        {
            totalTime += Time.deltaTime;

        }

    }
    public void ToggleOptionsMenu()
    {
        if (optionsPanel.activeSelf == true)
        {
            optionsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            optionsPanel.SetActive(true);
        }
    }
    public void YouWin()
    {
        gameOver = true;
        firstPersonController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        youWinPanel.SetActive(true);
        float roundedTime = (float)Math.Round(totalTime, 2);
        totalTime = roundedTime;
        totalTimeText.text = totalTime.ToString();

    }
    public void ToggleMusic()
    {
        if (music.enabled == true)
        {
            music.enabled = false;
        }
        else
        {
            music.enabled = true;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MailManTime");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
