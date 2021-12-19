using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainController : MonoBehaviour
{
    GameObject UI;
    IEnumerator waitForAction;
    GameObject player;
    bool readyToDrain = false;
    // Start is called before the first frame update
    void Start()
    {
        UI = transform.GetChild(0).gameObject;
        UI.SetActive(false);
        waitForAction = WaitForAction();
    }

    IEnumerator WaitForAction()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("GameManager").GetComponent<BucketManager>().Empty();
                ClearActions();
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            GameObject heldTool = player.GetComponent<InventoryManager>().heldTool;
            if(heldTool != null)
            {
                if(heldTool.name == "Bucket")
                {
                    if (GameObject.Find("GameManager").GetComponent<BucketManager>().IsFull())
                    {
                        readyToDrain = true;
                    }
                    else
                    {
                        readyToDrain = false;
                    }
                }
                else
                {
                    readyToDrain = false;
                }
            }
            else
            {
                readyToDrain = false;
            }
            if (readyToDrain)
            {
                StartCoroutine(waitForAction);
                UI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            ClearActions();
        }
    }

    private void ClearActions()
    {
        StopCoroutine(waitForAction);
        UI.SetActive(false);
        readyToDrain = false;
    }
}
