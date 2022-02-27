using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Scene
{
    MENU,
    PLAY,
    END,
}

public class UIManager : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;

    private AudioSource audioSource;

    public AudioClip buttonPress;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        AudioManager.GetInstance().PlaySFX(audioSource,buttonPress,0.5f,false);
        AudioManager.GetInstance().PlaySceneTrack(AudioManager.MusicTrack.BGM_PlayScene, 0.1f, 0.2f);
        SceneManager.LoadScene((int)Scene.PLAY);
    }


    /// <summary>
    /// Instructions button
    /// </summary>
    public void Button_Instructions()
    {
        AudioManager.GetInstance().PlaySFX(audioSource, buttonPress, 0.5f, false);
        InstructionsPanel.SetActive(true);
    }


    /// <summary>
    /// Credits button
    /// </summary>
    public void Button_Credits()
    {
        AudioManager.GetInstance().PlaySFX(audioSource, buttonPress, 0.5f, false);
        CreditsPanel.SetActive(true);
    }

    /// <summary>
    /// Back button functionality
    /// </summary>
    public void Button_Back()
    {
        AudioManager.GetInstance().PlaySFX(audioSource, buttonPress, 0.5f, false);
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
        AudioManager.GetInstance().PlaySFX(audioSource, buttonPress, 0.5f, false);
        AudioManager.GetInstance().PlaySceneTrack(AudioManager.MusicTrack.BGM_StartScene, 0.1f, 0.2f);
        SceneManager.LoadScene(0);
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Resume game
    /// </summary>
    public void Button_Resume()
    {
        Time.timeScale = 1f;
        AudioManager.GetInstance().PlaySFX(audioSource, buttonPress, 0.5f, false);
        GameManager.GetInstance().pausePanel.SetActive(false);
        GameManager.GetInstance().player.isPaused = false;
        Cursor.visible = false;
    }
}
