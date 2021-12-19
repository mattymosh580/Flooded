using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelec : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject loadScene;
    [SerializeField]
    GameObject playMenu;

    public void Back()
    {
        gameObject.SetActive(false);
        playMenu.SetActive(true);
    }

    public void SelectLevel(int level)
    {
        gameObject.SetActive(false);
        loadScene.GetComponent<LoadingScreen>().currentScene = (Levels)level;
        loadScene.GetComponent<LoadingScreen>().LoadLevel();
    }
}
