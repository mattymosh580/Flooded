using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletFillBar : MonoBehaviour
{
    Image bar;
    Vector3 scaleSpeed = Vector3.zero;
    bool expand = false;

    // E key blink
    private Image blinkingEKey;
    private bool eFlip = false;
    private float eTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.GetChild(0).GetComponent<Image>();
        blinkingEKey = transform.GetChild(2).GetComponent<Image>();
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Expand and shrink the bar
        if (expand && transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one, ref scaleSpeed, 0.1f);
        }
        else if (!expand && transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref scaleSpeed, 0.1f);
        }

        // Blinking E key
        if(expand)
        {
            eTime += Time.deltaTime;
            if (eTime >= 0.2f && !eFlip)
            {
                blinkingEKey.color = new Color(0.8f, 0.8f, 0.8f);
                eFlip = true;
            }
            else if (eTime >= 0.4)
            {
                blinkingEKey.color = Color.white;
                eFlip = false;
                eTime = 0.0f;
            }
        }
    }

    public void Expand()
    {
        expand = true;
    }

    public void Shrink()
    {
        expand = false;
    }

    public void SetBar(float fill)
    {
        bar.fillAmount = fill;
    }
}
