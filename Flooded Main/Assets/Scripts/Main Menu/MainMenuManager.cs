using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject OptionsScreen;
    public GameObject MusicSettingsScreen;
    public GameObject ChooseLevelScreen;
    public GameObject CreditsScreen;
    public GameObject TutorialScreen;
    public GameObject VideoScreen;

    AudioSource hover;
    AudioSource confirm;

    public Slider musicVol;
    public Slider sfxVol;

    bool hoverSoundPlayed;

    private void Start()
    {
        MusicSettingsScreen.SetActive(true);
        AudioSource[] sounds = transform.GetComponents<AudioSource>();
        hover = sounds[0];
        confirm = sounds[1];

        musicVol.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVol.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        hover.volume = sfxVol.value;
        confirm.volume = sfxVol.value;
        MusicSettingsScreen.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() && !hoverSoundPlayed && !CreditsScreen.activeSelf)
        {
            hover.Play();
            hoverSoundPlayed = true;
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && hoverSoundPlayed)
        {
            hoverSoundPlayed = false;
        }
    }

    public void LevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void ControlScreen()
    {
        confirm.Play();
        TutorialScreen.SetActive(true);
        OptionsScreen.SetActive(false);
    }

    public void playGame()
    {
        ChooseLevel();
    }
    
    public void ChooseLevel()
    {
        confirm.Play();
        ChooseLevelScreen.SetActive(true);
        MainPanel.SetActive(false);
        OptionsScreen.SetActive(false);
    }

    public void Options()
    {
        confirm.Play();
        OptionsScreen.SetActive(true);
        ChooseLevelScreen.SetActive(false);
        TutorialScreen.SetActive(false);
    }

    public void Video()
    {
        confirm.Play();
        VideoScreen.SetActive(true);
        OptionsScreen.SetActive(false);
        ChooseLevelScreen.SetActive(false);
        TutorialScreen.SetActive(false);
        MainPanel.SetActive(false);
    }

    public void MusicOptions()
    {
        confirm.Play();
        MusicSettingsScreen.SetActive(true);
        MainPanel.SetActive(false);
        OptionsScreen.SetActive(false);
    }

    public void Credits()
    {
        confirm.Play();
        MainPanel.SetActive(false);
        CreditsScreen.SetActive(true);
    }

    public void ApplyChanges()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVol.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol.value);
        confirm.Play();
        MusicSettingsScreen.SetActive(false);
        OptionsScreen.SetActive(true);
    }

    public void CancelMusicOptions()
    {
        musicVol.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVol.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        hover.volume = sfxVol.value;
        confirm.volume = sfxVol.value;
        ReturnToOptionsScreen();
    }

    public void ReturnToOptionsScreen()
    {
        confirm.Play();
        OptionsScreen.SetActive(true);
        MusicSettingsScreen.SetActive(false);
        TutorialScreen.SetActive(false);
        VideoScreen.SetActive(false);
    }

    public void CancelChanges()
    {
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        confirm.Play();
        MainPanel.SetActive(true);
        OptionsScreen.SetActive(false);
        ChooseLevelScreen.SetActive(false);
        CreditsScreen.SetActive(false);
    }

    public void quitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
        //Used only in development remove for build
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    
#endif
    }
}
