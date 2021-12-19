using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    KeyPromptCanvas ePrompt;
    [SerializeField]
    Sprite image;
    public GameObject player = null;
    object[] objPacket = new object[2];

    private void OnEnable()
    {
        objPacket[0] = this.gameObject;
        objPacket[1] = image;

        ePrompt = transform.GetChild(0).GetChild(0).GetComponent<KeyPromptCanvas>();
        ePrompt.hasCorrectTools = true;
    }

    private void Start()
    {
        ePrompt = transform.GetComponentInChildren<KeyPromptCanvas>();
        ePrompt.hasCorrectTools = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<characterController>().enabled)
        {

            TriggerActions(other.gameObject);
        }
    }

    public void TriggerActions(GameObject _player)
    {

        player = _player;
        ePrompt.Show(player.transform.GetChild(0).gameObject);
        EventManager.PressEActions += PickUp;
        EventManager.pressEActionsCounter++;
    }

    public void PickUp()
    {
        if(player != null)
        {
            player.SendMessage("PickUpHeldItem", objPacket);
        }
    }

    void OnPickup()
    {
        EventManager.PressEActions -= PickUp;
        EventManager.pressEActionsCounter--;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<characterController>().enabled)
        {
            TriggerExitActions();
        }
    }

    public void TriggerExitActions()
    {
        ePrompt.Hide();
        EventManager.PressEActions -= PickUp;
        EventManager.pressEActionsCounter--;
        player = null;
    }

    public Sprite GetSprite()
    {
        return image;
    }


    public KeyPromptCanvas GetKeyPrompt()
    {


        return ePrompt;
    }


}
