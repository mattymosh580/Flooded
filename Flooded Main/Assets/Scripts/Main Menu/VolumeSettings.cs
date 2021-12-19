using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    public GameObject OptionsScreen;
    public AudioSource musicSource;
    public AudioSource mainGameSource;

    public Slider musicSliderBar;

    float curMusicVolume;
    float curGameVolume;

    void Start()
    {

        if(musicSource == null)
        {
            Debug.Log("test");

            musicSource = GameObject.Find("BackgroundAudio").GetComponent<AudioSource>();

            if (musicSource != null)
            {
                Debug.Log("worked");
            }
        }



        curMusicVolume = musicSource.volume;

        //curGameVolume = mainGameSource.volume;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void applySettings()
    {
        curMusicVolume = musicSource.volume;
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        OptionsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void cancelButton()
    {
        musicSource.volume = curMusicVolume;

        musicSliderBar.value = curMusicVolume;

        gameObject.SetActive(false);

        OptionsScreen.SetActive(true);
    }

}
