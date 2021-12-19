using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucketAction : MonoBehaviour
{
    GameObject player;
    GameObject flood;
    bool isFull;
    bool isHeld;
    GameObject bucketUIimage;

    // Start is called before the first frame update
    void Start()
    {
        flood = GameObject.Find("Flood");
        player = GameObject.Find("Player");
        isFull = false;
        isHeld = false;
        bucketUIimage = GameObject.Find("BucketUI").transform.GetChild(0).gameObject;
        bucketUIimage.SetActive(false);
    }

    private void Update()
    {
        if(player.GetComponent<InventoryManager>().heldTool == this && player.GetComponent<InventoryManager>().getSelectCounter() == 2)
        {
            if(isHeld == false)
            {
                isHeld = true;
            }
        }
        else
        {
            if (isHeld == true)
            {
                isHeld = false;
            }
        }

        drawUI();

        if (Input.GetKey(KeyCode.E) && isHeld)
        {
            fill();
        }
    }

    void drawUI()
    {
        if(flood.transform.localScale.y <= 0)
        {
            return;
        }
        if (isHeld)
        {
            bucketUIimage.SetActive(true);
        }
        else
        {
            bucketUIimage.SetActive(false);
        }
    }

    void fill()
    {
        if (isFull)
        {
            return;
        }
        flood.GetComponent<FloodScript>().Lower(.1f);
    }

}
