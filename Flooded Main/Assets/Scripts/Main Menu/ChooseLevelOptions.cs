using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelOptions : MonoBehaviour
{

    public Dropdown dropDownList;
    public LoadingScreen loadingScreen;
    public Levels selectedLevel;

    

    void Start()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }


    public void NewGame()
    {
        loadingScreen.currentScene = Levels.LevelOne;
        loadingScreen.LoadLevel();
    }

    public void cancelButton()
    {

        gameObject.SetActive(false);

    }
}
