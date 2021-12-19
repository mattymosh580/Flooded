using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolPickup : MonoBehaviour
{
    [SerializeField]
    Sprite image;
    public GameObject player = null;
    object[] objPacket = new object[2];
    KeyPromptCanvas ePrompt;

    public Vector3 itemStartPosition;

    private void Start()
    {
        itemStartPosition = this.transform.position;

        ePrompt = transform.GetComponentInChildren<KeyPromptCanvas>();
        ePrompt.hasCorrectTools = true;
        objPacket[0] = this.gameObject;
        objPacket[1] = image;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
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

    void PickUp()
    {
        if (player != null)
        {
            transform.GetComponent<Rigidbody>().isKinematic = false;
            player.SendMessage("PickUpTool", objPacket);
        }
    }

    void OnPickup()
    {
        EventManager.PressEActions -= PickUp;
        EventManager.pressEActionsCounter--;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

    public void SetSprite(Sprite _sprite)
    {
        image = _sprite;
    }

}
