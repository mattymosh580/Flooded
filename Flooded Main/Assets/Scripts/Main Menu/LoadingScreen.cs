using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Levels
{
    MainMenu,
    LevelOne,
    LevelTwo,
    LevelThree,
    LevelFour,
    //LevelFive,
    //LevelSix
};

public class LoadingScreen : MonoBehaviour
{
    public Levels currentScene;

    public GameObject loadingScreen;

    public void LoadLevel ()
    {

        StartCoroutine(loadAsynchronously(currentScene));
    }

    IEnumerator loadAsynchronously (Levels level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)level);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {

            yield return null;
        }

    }

}
