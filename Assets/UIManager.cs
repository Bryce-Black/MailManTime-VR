using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject optionsPanel;
    public GameObject youWinPanel;
    private float totalTime;
    public TextMeshProUGUI totalTimeText;
    public FirstPersonController firstPersonController;
    public AudioSource music;
    private bool gameOver = false;
    public XRInteractorLineVisual xrLineVisualLeft, xrLineVisualRight;
    private void Start()
    {
        //YouWin();
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
            SetRayInteractorAlphaValue(0f, false);
            optionsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            SetRayInteractorAlphaValue(1f, false);
            Time.timeScale = 0f;
            optionsPanel.SetActive(true);
        }
    }
    public void YouWin()
    {
        Time.timeScale = 0f;
        SetRayInteractorAlphaValue(1f, false);
        gameOver = true;
        firstPersonController.enabled = false;
        youWinPanel.SetActive(true);
        float roundedTime = (float)Math.Round(totalTime, 2);
        totalTime = roundedTime;
        totalTimeText.text = "TOTAL TIME: " + totalTime.ToString();

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

    public void SetRayInteractorAlphaValue(float alphaValue, bool onlyLeft)
    {
        //SETTING GRADIENT TO REMOVE UGLY LINES
        var gradient = new Gradient();

        // Blend color from red at 0% to blue at 100%
        var colors = new GradientColorKey[2];
        colors[0] = new GradientColorKey(Color.red, 0.0f);
        colors[1] = new GradientColorKey(Color.blue, 1.0f);

        // Blend alpha from opaque at 0% to transparent at 100%
        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(alphaValue, 0);
        alphas[1] = new GradientAlphaKey(alphaValue, 1);

        gradient.SetKeys(colors, alphas);
        if (onlyLeft)
        {
            xrLineVisualLeft.validColorGradient = gradient;
            xrLineVisualLeft.invalidColorGradient = gradient;
        }
        else
        {
            xrLineVisualLeft.validColorGradient = gradient;
            xrLineVisualLeft.invalidColorGradient = gradient;

            xrLineVisualRight.validColorGradient = gradient;
            xrLineVisualRight.invalidColorGradient = gradient;
        }
        
        //xrLineVisualRight.validColorGradient = gradient;
    }
}
