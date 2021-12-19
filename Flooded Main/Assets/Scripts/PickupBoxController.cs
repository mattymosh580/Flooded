using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBoxController : MonoBehaviour
{
    [SerializeField]
    GameObject pipe;
    KeyPromptCanvas ePrompt;
    Sprite pipeSprite;
    Image image;
    Text eText;
    bool onCoolDown = false;
    IEnumerator waitForInput;
    IEnumerator coolDown;
    BoxCollider triggerBox;

    GameObject player;

    private void Start()
    {
        // Get player camera
        GameObject playerCam = GameObject.FindGameObjectsWithTag("Player")[0].transform.GetChild(0).gameObject;
        ePrompt = GetComponentInChildren<KeyPromptCanvas>();
        ePrompt.Show(playerCam);

        triggerBox = transform.GetComponents<BoxCollider>()[0];

        pipeSprite = pipe.GetComponent<ItemPickup>().GetSprite();
        image = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        eText = transform.GetComponentInChildren<Text>();
        image.sprite = pipeSprite;
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        waitForInput = WaitForInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerActions(other.gameObject);
        }
    }

    public void TriggerActions(GameObject _player)
    {
        if (!onCoolDown)
        {
            player = _player;
            ePrompt.hasCorrectTools = true;
            StartCoroutine(waitForInput);
        }
    }


    public IEnumerator WaitForInput()
    {
        for(; ; )
        {
            if (player && !onCoolDown && Input.GetKeyDown(KeyCode.E))
            {
                GameObject tempPipe = Instantiate(pipe);
                if (GameObject.Find("GameManager").GetComponent<NewGameManager>().player.GetComponent<InventoryManager>().PickUpHeldItem(tempPipe))
                {
                    player = null;
                    StartCoroutine(CoolDown());
                }
                else
                {
                    Destroy(tempPipe);
                }
            }
            yield return null;
        }
    }

    public IEnumerator CoolDown()
    {
        onCoolDown = true;
        image.color = Color.gray;
        image.fillAmount = 0;
        float timer = 0;

        ePrompt.hasCorrectTools = false;
        triggerBox.enabled = false;

        while (timer < 5)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer / 5;
            yield return null;
        }
        image.fillAmount = 1;
        image.color = Color.white;
        onCoolDown = false;

        triggerBox.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerExitActions();
        }
    }

    public void TriggerExitActions()
    {
        player = null;
        ePrompt.hasCorrectTools = false;
        StopCoroutine(waitForInput);
    }
}
