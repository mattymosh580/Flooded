using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPipeGame : MiniGameBase
{

    public enum pipeTypes { PipeLong = 1, PipePlus, PipeT, PipeElbow};
    public pipeTypes pipeType;
    public int waterAmount;

    public GameObject UI;

    

    private GameObject player;
    private GameObject heldTool;
    private GameObject heldItem;

    int playerItem = 0;

    bool playerInGame = false;
    bool playerHasItems = false;                    // check to see if player has items that is needed for this exact puzzle
    bool puzzleComplete = false;

    bool paused = false;

    public GameObject pipeBroken;
    public GameObject pipeFixed;

    public GameObject normalWall;
    public GameObject brokenWall;

    // Start is called before the first frame update
    protected override void Start()
    {
        pipeBroken = transform.GetChild(4).gameObject;
        pipeFixed = transform.GetChild(3).gameObject;

        normalWall = transform.GetChild(1).gameObject;
        brokenWall = transform.GetChild(2).gameObject;

        particles = transform.GetChild(4).GetChild(1).GetComponent<ParticleSystem>();
        particles.Stop();
        base.Start();
        GetComponent<BoxCollider>().enabled = broken;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!NewGameManager.inPause && paused)
        {
            particles.Play();
            paused = false;
        }
        else if (NewGameManager.inPause && !paused)
        {
            particles.Stop();
            paused = true;
        }


        if (player && broken && playerHasItems)
        {
            if (Input.GetButtonDown("Interact") || playerInGame)
            {

                playerInGame = true;

                if (!puzzleComplete)
                {
                    rotation();
                }

                pipeBroken.SetActive(false);
                pipeFixed.SetActive(true);


                if (pipeType == pipeTypes.PipeLong)
                {
                   
                    checkStraight();
                }
                else if (pipeFixed.transform.rotation == pipeBroken.transform.rotation)
                {
                    
                    puzzleComplete = true;

                    
                }

                if (puzzleComplete)
                {
                    UI.SetActive(false);
                    FixLeak();
                }
            }
        }
    }

    void rotation()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {

            pipeFixed.transform.Rotate(new Vector3(0, 0, 45));

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            pipeFixed.transform.Rotate(new Vector3(0, 0, -45));

        }

    }

    void checkStraight()
    {

        if (pipeFixed.transform.rotation == pipeBroken.transform.rotation)
        {
            puzzleComplete = true;
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            heldItem = player.GetComponent<InventoryManager>().heldItem;
            heldTool = player.GetComponent<InventoryManager>().heldTool;
        }

        if (heldItem == null || heldTool == null)
        {
            return;
        }

        if (heldItem.ToString().Remove(8) == "PipeLong")
        {
            playerItem = 1;
        }
        else if (heldItem.ToString().Remove(8) == "PipePlus")
        {
            playerItem = 2;
        }
        else if (heldItem.ToString().Remove(5) == "PipeT")
        {
            playerItem = 3;
        }
        else if (heldItem.ToString().Remove(9) == "PipeElbow")
        {
            playerItem = 4;
        }

        if (heldTool.tag == "Wrench")
        {
            UI.SetActive(true);

            if ((int)pipeType == playerItem)
            {
                playerHasItems = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            UI.SetActive(false);
            playerItem = 0;

            playerInGame = false;

            if (!puzzleComplete)
            {
                pipeBroken.SetActive(true);
                pipeFixed.SetActive(false);
            }
        }
    }

    public override void Break()
    {
        broken = true;

        GetComponent<BoxCollider>().enabled = true;

        particles.Play();
        normalWall.SetActive(false);
        brokenWall.SetActive(true);

        pipeBroken.SetActive(true);
        pipeFixed.SetActive(false);
    }

    public override void FixLeak()
    {
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        normalWall.SetActive(true);
        brokenWall.SetActive(false);

        pipeBroken.SetActive(false);

        puzzleComplete = false;
        GetComponent<BoxCollider>().enabled = false;
        broken = false;
    }
}
