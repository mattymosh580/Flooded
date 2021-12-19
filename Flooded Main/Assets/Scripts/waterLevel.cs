using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waterLevel : MonoBehaviour
{
    public static Image waterBar;

    float fill;

    public void Awake()
    {
        waterBar = transform.Find("waterImage").GetComponent<Image>();
        waterBar.color = new Color(0f, 132f, 191f, .4f);

        waterBar.fillAmount = 0f;
    }

    public void Update()
    {
        //waterBar.fillAmount += (.001f / 60); //this will add a consistent filling effect even if there are no leaks.
    }
}
