using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Scene
{
    MENU,
    PLAY
}

public class UIManager : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Play Button 
    /// </summary>
    public void Button_Play()
    {
        SceneManager.LoadScene((int)Scene.PLAY);
    }


    /// <summary>
    /// Instructions button
    /// </summary>
    public void Button_Instructions()
    {
        InstructionsPanel.SetActive(true);
    }


    /// <summary>
    /// Credits button
    /// </summary>
    public void Button_Credits()
    {
        CreditsPanel.SetActive(true);
    }

    /// <summary>
    /// Back button functionality
    /// </summary>
    public void Button_Back()
    {
        InstructionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    /// <summary>
    /// Exit button functionality
    /// </summary>
    public void Button_Exit()
    {

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    /// <summary>
    /// Open Main Menu
    /// </summary>
    public void Button_Menu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Resume game
    /// </summary>
    public void Button_Resume()
    {
        Time.timeScale = 1f;
        GameManager.GetInstance().pausePanel.SetActive(false);
        GameManager.GetInstance().player.isPaused = false;
    }
}
