using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public InventoryHUD HUD;

    public GameObject heldItem = null;
    public GameObject heldTool = null;

    int selectCounter = 1;


    public void Start()
    {
        HUD = FindObjectOfType<InventoryHUD>();
    }

    public void PickUpHeldItem(object obj)
    {

        if(heldItem != null)
        {
            return;
        }
        object[] objPacket = (object[])obj;        
        heldItem = (GameObject)objPacket[0];
        heldItem.SendMessage("OnPickup");
        heldItem.SetActive(false);
        HUD.SetItemImage((Sprite)objPacket[1]);
    }

    public bool PickUpHeldItem(GameObject item)
    {
        if (heldItem != null)
        {
            return false;
        }
        heldItem = item;
        heldItem.SetActive(false);
        HUD.SetItemImage(item.GetComponent<ItemPickup>().GetSprite());
        return true;
    }

    public bool PickUpTool(object obj)
    {
        if (heldTool != null)
        {
            return false;
        }
        object[] objPacket = (object[])obj;
        heldTool = (GameObject)objPacket[0];
        heldTool.SendMessage("OnPickup");
        heldTool.SetActive(false);
        HUD.SetToolImage((Sprite)objPacket[1]);
        return true;
    }


    public void Update()
    {



        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (selectCounter == 1)
            {
                DropHeldItem();
            }
            else
            {
                DropHeldTool();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectCounter = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectCounter = 2;
        }

        if(Input.mouseScrollDelta.y < 0 && selectCounter < 2)
        {
            selectCounter++;
        }

        if(Input.mouseScrollDelta.y > 0 && selectCounter > 1)
        {
            selectCounter--;
        }

        HighlightBox();
    }

    public GameObject DropHeldItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.position = transform.position;
            heldItem.transform.Translate(new Vector3(0, .5f, .5f), transform);
            heldItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldItem.SetActive(true);
            GameObject temp = heldItem;
            heldItem = null;
            HUD.SetItemImage(null);
            return temp;
        }
        return null;
    }

    private void DropHeldTool()
    {
        if (heldTool != null)
        {
            heldTool.transform.position = transform.position;
            heldTool.transform.Translate(new Vector3(0, .5f, .5f), transform);
            heldTool.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldTool.SetActive(true);
            heldTool = null;
            HUD.SetToolImage(null);
        }
    }

    private void HighlightBox()
    {
        HUD.Highlight(selectCounter);
    }

    public int getSelectCounter()
    {
        return selectCounter;
    }

}
