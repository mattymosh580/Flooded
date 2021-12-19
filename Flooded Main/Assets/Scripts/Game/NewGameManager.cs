using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject endScreen;
    public GameObject optionsScreen;
    public GameObject musicSettingsScreen;
    public Timer timer;
    public GameObject controlScreen;
    public GameObject TutorialScreen;
    public GameObject VideoScreen;
    public GameObject bucketUi;

    public MiniGameManager miniGame = null;

    public GameObject player;
    public GameObject UI;

    public Slider musicSlider;
    public Slider sfxSlider;

    GameObject[] pipeList;
    GameObject[] wrenchList;
    GameObject[] plungerList;

    //List<Vector3> pipeVelList = new List<Vector3>();
    //List<Vector3> wrenchVelList = new List<Vector3>();
    //List<Vector3> plungerVelList = new List<Vector3>();

    public AudioSource music;

    float second;

    public static bool inPause = false;

    bool gameOver;
    void Start()
    {
        inPause = false;

        musicSettingsScreen.SetActive(true);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSettingsScreen.SetActive(false);

        Time.timeScale = 1;
        endScreen.SetActive(false);
        music.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void restartGame()
    {
        inPause = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void toMainMenu()
    {
        inPause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelChanges()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        optionScreen();
    }

    public void ControlScreen()
    {
        TutorialScreen.SetActive(true);
        optionsScreen.SetActive(false);
        //controlScreen.SetActive(false);
    }

    public void optionScreen()
    {
        pausePanel.SetActive(false);
        musicSettingsScreen.SetActive(false);
        controlScreen.SetActive(false);
        optionsScreen.SetActive(true);
        TutorialScreen.SetActive(false);
        VideoScreen.SetActive(false);
    }

    public void pauseScreen()
    {
        optionsScreen.SetActive(false);
        musicSettingsScreen.SetActive(false);
        TutorialScreen.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Video()
    {
        VideoScreen.SetActive(true);
        optionsScreen.SetActive(false);
        TutorialScreen.SetActive(false);
        pausePanel.SetActive(false);
        musicSettingsScreen.SetActive(false);
    }

    public void musicSettings()
    {
        optionsScreen.SetActive(false);
        musicSettingsScreen.SetActive(true);
    }

    public void ApplyChanges()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
        inPause = false;
        UI.SetActive(true);
        pausePanel.SetActive(false);
        player.GetComponent<characterController>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (waterLevel.waterBar.fillAmount == 1 || timer.timeValue == 0 && !gameOver)
        {

            if (timer.timeValue == 0)
            {
                GameObject.Find("GameManager").GetComponent<ScoreCalculator>().win = true;
            }

            GameObject.Find("GameManager").GetComponent<ScoreCalculator>().timerScore = (timer.timeValue);
            GameObject.Find("GameManager").GetComponent<ScoreCalculator>().updateScore();

            UI.SetActive(false);
            bucketUi.SetActive(false);

            inPause = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            endScreen.SetActive(true);
            Time.timeScale = 0;

            gameOver = true;
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pipeList = GameObject.FindGameObjectsWithTag("Pipe");
            wrenchList = GameObject.FindGameObjectsWithTag("Wrench");
            plungerList = GameObject.FindGameObjectsWithTag("Plunger");

            if (!inPause)
            {
                Time.timeScale = 0;
                inPause = true;
                pausePanel.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                UI.SetActive(false);

                player.GetComponent<characterController>().enabled = false;

            }
            else if(inPause)
            {
                Time.timeScale = 1;
                inPause = false;
                pausePanel.SetActive(false);
                optionsScreen.SetActive(false);
                musicSettingsScreen.SetActive(false);

                UI.SetActive(true);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                for (int i = 0; i < pipeList.Length; i++)
                {

                    pipeList[i].GetComponent<Rigidbody>().isKinematic = false;

                    //pipeList[i].GetComponent<Rigidbody>().AddRelativeForce(pipeVelList[i], ForceMode.VelocityChange);
                }

                for (int i = 0; i < wrenchList.Length; i++)
                {
                    wrenchList[i].GetComponent<Rigidbody>().isKinematic = false;

                    //wrenchList[i].GetComponent<Rigidbody>().AddForce(wrenchVelList[i], ForceMode.VelocityChange);
                }

                for (int i = 0; i < plungerList.Length; i++)
                {
                    plungerList[i].GetComponent<Rigidbody>().isKinematic = false;

                    //wrenchList[i].GetComponent<Rigidbody>().AddForce(plungerVelList[i], ForceMode.VelocityChange);
                }

                GetComponent<RandomLeakGenerator>().pauseLeaks = false;

                GetComponent<RandomLeakGenerator>().enabled = true;

                if (miniGame == null)
                {
                    
                    player.GetComponent<characterController>().enabled = true;
                }

            }
        }


    }
}
