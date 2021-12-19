using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    Image itemImage;
    Image toolImage;
    GameObject[] highlights;

    // Start is called before the first frame update
    void Start()
    {
        itemImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        toolImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();

        highlights = new GameObject[] { transform.GetChild(0).GetChild(1).gameObject, transform.GetChild(1).GetChild(1).gameObject };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemImage(Sprite item)
    {
        itemImage.sprite = item;
        if(item == null)
        {
            itemImage.color = new Color(255, 255, 255, 0);
        }
        else
        {
            itemImage.color = new Color(255, 255, 255, 1);
        }
    }

    public void SetToolImage(Sprite tool)
    {
        toolImage.sprite = tool;
        if (tool == null)
        {
            toolImage.color = new Color(255, 255, 255, 0);
        }
        else
        {
            toolImage.color = new Color(255, 255, 255, 1);
        }
    }

    public void Highlight(int select)
    {
        switch (select)
        {
            case 1:
                highlights[0].SetActive(true);
                highlights[1].SetActive(false);
                break;
            case 2:
                highlights[0].SetActive(false);
                highlights[1].SetActive(true);
                break;
        }
    }
}
