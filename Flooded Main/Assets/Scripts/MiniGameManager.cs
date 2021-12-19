using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MiniGameManager : MonoBehaviour
{
    [SerializeField]
    KeyPromptCanvas UI;
    protected GameObject player;
    GameObject heldTool;
    [SerializeField]
    GameObject requiredTool;

    bool gameRan = false;

    [SerializeField]
    protected bool broken = false;

    protected FloodScript flood;

    protected virtual void Start()
    {
        if (broken)
        {
            Break();
        }

        // Automatically adds the object to Random Leak Generator's breakables list
        FindObjectOfType<RandomLeakGenerator>().AddBreakable(gameObject);

        // Find flood object
        flood = FindObjectOfType<FloodScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!broken)
            {
                return;
            }
            
            player = other.gameObject;

            if (broken)
            {
                EventManager.PressEActions += RunGame;
                EventManager.pressEActionsCounter++;


                if (player)
                {
                    UI.Show(player.transform.GetChild(0).gameObject);
                }
            }
        }
    }
    void RunGame()
    {
        if (player.GetComponent<InventoryManager>().heldTool != null)
        {
            if (player != null && player.GetComponent<InventoryManager>().heldTool.name == requiredTool.name)
            {

                GameObject.Find("GameManager").GetComponent<NewGameManager>().miniGame = this;

                UI.Hide();
                EventManager.pressEActionsCounter--;
                EventManager.PressEActions -= RunGame;

                player.GetComponent<characterController>().enabled = false;
                player.transform.GetChild(0).gameObject.GetComponent<cameraController>().enabled = false;
                player.GetComponent<InventoryManager>().enabled = false;

                player.GetComponent<CharacterController>().Move(Vector3.zero);

                Vector3 pos = transform.position + transform.forward;

                player.transform.position = new Vector3(pos.x, pos.y + .85f, pos.z);
                player.transform.LookAt(new Vector3(transform.position.x, player.transform.position.y, transform.position.z));

                gameRan = true;

                PlayGame();
            }
        }
    }

    protected virtual void Update()
    {
        if (broken)
        {
            waterLevel.waterBar.fillAmount += Time.deltaTime / 500;    // 0.2% of water level bar per second; 500 seconds before filled
            flood.Raise(Time.deltaTime / 500);
        }

        // Check player's tool
        if(player)
        {
            heldTool = player.GetComponent<InventoryManager>().heldTool;

            if (heldTool && heldTool.name == requiredTool.name && !UI.hasCorrectTools)
            {
                UI.hasCorrectTools = true;
            }
            else if((!heldTool || heldTool.name != requiredTool.name) && UI.hasCorrectTools)
            {
                UI.hasCorrectTools = false;
            }
        }
    }

    public virtual void EndGame()
    {
        GameObject.Find("GameManager").GetComponent<NewGameManager>().miniGame = null;

        player.GetComponent<characterController>().enabled = true;
        player.GetComponent<InventoryManager>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (broken)
            {
                if (gameRan)
                {
                    gameRan = false;
                }
                else
                {
                    UI.Hide();
                    EventManager.pressEActionsCounter--;
                    EventManager.PressEActions -= RunGame;
                    gameRan = false;
                    player = null;
                }
            }
        }
    }

    public bool IsBroken()
    {
        return broken;
    }

    // Makes the object start leaking
    public abstract void Break();

    // Fixes the object's leak
    public virtual void FixLeak()
    {
        broken = false;
    }

    public abstract void PlayGame();
}
