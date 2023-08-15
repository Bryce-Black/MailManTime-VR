using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUIManager : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;
    public MusicManager musicManager;
    public void StartTheGame()
    {
        SceneManager.LoadScene("MailManTime");
    }
    private void Update()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        if(howToPlayPanel.activeSelf == true)
        {
            howToPlayPanel.SetActive(false);
        }
        else
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void ToggleCredits()
    {
        if (creditsPanel.activeSelf == true)
        {
            creditsPanel.SetActive(false);
        }
        else
        {
            creditsPanel.SetActive(true);
        }
    }

    public void MuteMusic()
    {
        musicManager.ToggleMusic();
    }
}
