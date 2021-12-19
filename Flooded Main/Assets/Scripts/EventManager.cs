using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
    public Canvas dynamaicCanvas;

    static Canvas _dynamicCanvas;

    [SerializeField]
    GameObject pressEText;

    static GameObject _pressEText;

    public delegate void OnPressE();
    public static event OnPressE PressEActions;
    public static int pressEActionsCounter = 0;



    public static void EPressed()
    {
        PressEActions();
    }

    public void Start()
    {
        

        _dynamicCanvas = dynamaicCanvas;
        _pressEText = pressEText;
    }

    public void Update()
    {    
        if (pressEActionsCounter != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                EPressed();
            }
        }
        
    }

    public static GameObject WritePressE()
    {
        GameObject newText = Instantiate(_pressEText);
        newText.transform.SetParent(_dynamicCanvas.transform);
        newText.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);
        newText.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
        newText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100, 0);

        return newText;
    }
}
