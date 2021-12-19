using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    [SerializeField]
    GameObject LevelSelect;
    [SerializeField]
    GameObject playMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        LevelSelect.SetActive(true);
        playMenu.SetActive(false);
    }
}

