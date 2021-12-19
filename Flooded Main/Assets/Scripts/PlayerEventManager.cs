using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    Canvas playerUI;

    [SerializeField]
    GameObject pressEText;

    public delegate void OnPressE();
    public event OnPressE PressEActions;
    int pressEActionsCounter = 0;



    public void EPressed()
    {
        PressEActions();
    }

    public void Start()
    {
        playerUI = this.GetComponentInChildren<Canvas>();
        WritePressE();
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

    public GameObject WritePressE()
    {
        GameObject newText = Instantiate(pressEText);
        newText.transform.SetParent(playerUI.transform);
        newText.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);
        newText.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
        newText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100, 0);
        return newText;
    }

    public void PressECounterUp()
    {
        pressEActionsCounter++;
    }
    public void PressECounterDown()
    {
        pressEActionsCounter--;
    }
}
