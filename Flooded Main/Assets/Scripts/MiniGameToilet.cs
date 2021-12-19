using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameToilet : MiniGameBase
{
    // Minigame fill bar
    private float barFillAmount = 0.0f;
    ToiletFillBar barCanvas;
    // Minigame plunger object
    private GameObject minigamePlunger;

    // Bar size and scale speed
    private static Vector3 barMainScale = new Vector3(0.07f, 0.07f, 1);
    private static Vector3 barScaleSpeed = Vector3.zero;
    // Minigame plunger velocity, position, and scale vectors
    private Vector3 pVelocity = Vector3.zero;
    private Vector3 pScaleSpeed = Vector3.zero;
    private Vector3 pMainScale = new Vector3(0.85f, 0.85f, 0.85f);
    private static Vector3 plungerUp = new Vector3(0, 1.25f, 0);
    private static Vector3 plungerDown = new Vector3(0, 0.85f, 0);
    // Player minigame position vector
    private Vector3 playerPosition;

    // E key prompt canvas
    private GameObject eKeyCanvas;
    // Images displayed on the prompt
    private GameObject eText;
    private GameObject requiredToolImage;
    // Canvas velocity, position, and scale vectors
    private Vector3 eVelocity = Vector3.zero;
    private Vector3 eScaleSpeed = Vector3.zero;
    private static Vector3 eMainScale = new Vector3(1, 1, 1);
    private static Vector3 eMainPos = new Vector3(0, 1.25f, 0);

    // The player's tool and camera
    private GameObject player;
    private string playerTool;
    private GameObject playerCam;
    // Is the player near the object?
    private bool playerETrigger = false;
    // Is the player playing the minigame?
    bool isPlaying = false;

    protected override void Start()
    {
        // Get particles
        particles = transform.GetComponent<ParticleSystem>();

        // Get minigame plunger
        minigamePlunger = transform.GetChild(1).gameObject;
        minigamePlunger.transform.localScale = Vector3.zero;
        minigamePlunger.SetActive(false);

        // Get fill bar cnavas
        barCanvas = FindObjectOfType<ToiletFillBar>();

        // Get E key prompt and images
        eKeyCanvas = transform.GetChild(0).gameObject;
        eText = eKeyCanvas.transform.GetChild(0).GetChild(0).gameObject;
        requiredToolImage = eKeyCanvas.transform.GetChild(0).GetChild(1).gameObject;

        eKeyCanvas.transform.localPosition = Vector3.zero;

        // Call base class Start
        base.Start();
        GetComponent<BoxCollider>().enabled = broken;
        if(!broken)
        {
            particles.Stop();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Call base class Update
        base.Update();
        KeyPromptMovement();

        // Disable burst particles if active
        if(particles.isPlaying && transform.GetChild(2).GetComponent<ParticleSystem>().particleCount > 1)
        {
            transform.GetChild(2).GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        // Minigame
        if(player && isPlaying)
        {
                MiniGame();
        }
        else
        {
            // Shrink plunger
            if (minigamePlunger.transform.localScale != Vector3.zero)
            {
                minigamePlunger.transform.localScale = Vector3.SmoothDamp(minigamePlunger.transform.localScale, Vector3.zero, ref pScaleSpeed, 0.1f);
            }
            else if(minigamePlunger.transform.localScale == Vector3.zero && minigamePlunger.activeSelf)
            {
                minigamePlunger.SetActive(false);
            }
        }
    }

    // Breaks the toilet
    public override void Break()
    {
        if(!broken)
        {
            GetComponent<BoxCollider>().enabled = true;
            particles.Play();
            broken = true;
        }
    }

    // Plays the toilet minigame
    void PlayGame()
    {
        if(player && playerTool == "Plunger")
        {
            // Hide E prompt
            playerETrigger = false;

            // Show fill bar and plunger
            minigamePlunger.SetActive(true);

            // Reset fill level
            barFillAmount = 0.0f;

            // Disable player controls
            player.GetComponent<characterController>().enabled = false;
            player.GetComponent<InventoryManager>().enabled = false;
            player.transform.GetChild(0).gameObject.GetComponent<cameraController>().enabled = false;

            // Change player position
            playerPosition = new Vector3(transform.position.x, player.transform.position.y + 0.05f, transform.position.z) + transform.forward;
            player.transform.position = playerPosition;
            player.transform.LookAt(new Vector3(transform.position.x, player.transform.position.y, transform.position.z));

            isPlaying = true;
            barCanvas.Expand();

            EventManager.PressEActions -= PlayGame;
            EventManager.pressEActionsCounter--;
        }
    }

    // The toilet minigame
    void MiniGame()
    {
        // Expand plunger
        if(minigamePlunger.transform.localScale != pMainScale)
        {
            minigamePlunger.transform.localScale = Vector3.SmoothDamp(minigamePlunger.transform.localScale, pMainScale, ref pScaleSpeed, 0.1f);
        }

        // Lower and raise plunger
        if (minigamePlunger.transform.localScale.y >= pMainScale.y * 0.9 && Input.GetKey(KeyCode.E))
        {
            minigamePlunger.transform.localPosition = Vector3.SmoothDamp(minigamePlunger.transform.localPosition, plungerDown, ref pVelocity, 0.1f);
        }
        else
        {
            minigamePlunger.transform.localPosition = Vector3.SmoothDamp(minigamePlunger.transform.localPosition, plungerUp, ref pVelocity, 0.1f);
        }

        // Add plunger velocity to fill bar
        barFillAmount += Mathf.Abs(pVelocity.y * (Time.deltaTime / 7));
        barCanvas.SetBar(barFillAmount);

        // If full, end the minigame
        if (barFillAmount >= 1.0f)
        {
            GameObject.Find("GameManager").GetComponent<ScoreCalculator>().toiletScore += 1;

            FixLeak();
        }

        // Constantly drain fill bar
        if (barFillAmount > 0.0f)
        {
            barFillAmount -= Time.deltaTime / 10; // 10% of fill bar per second
        }
    }

    // Fixes the toilet
    public override void FixLeak()
    {
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        // Re-enable player controls
        if(player)
        {
            player.GetComponent<characterController>().enabled = true;
            player.GetComponent<InventoryManager>().enabled = true;
            player.transform.GetChild(0).gameObject.GetComponent<cameraController>().enabled = true;
        }

        GetComponent<BoxCollider>().enabled = false;

        barCanvas.Shrink();

        broken = false;
        isPlaying = false;
    }

    


    // Handles the functionality of the E key prompt
    private void KeyPromptMovement()
    {
        // Activate/deactivate the E key prompt
        if(!eKeyCanvas.activeSelf && playerETrigger)
        {
            eKeyCanvas.SetActive(true);
        }
        else if(eKeyCanvas.activeSelf && eKeyCanvas.transform.localPosition == Vector3.zero)
        {
            eKeyCanvas.SetActive(false);
        }

        // Check if the player's tool has changed
        if (player && player.GetComponent<InventoryManager>().heldTool)
        {
            playerTool = player.GetComponent<InventoryManager>().heldTool.tag;
        }
        else if (player && !player.GetComponent<InventoryManager>().heldTool)
        {
            playerTool = null;
        }

        // Choose the image to display on the prompt
        if (playerTool == "Plunger")
        {
            eText.SetActive(true);
            requiredToolImage.SetActive(false);
        }
        else
        {
            eText.SetActive(false);
            requiredToolImage.SetActive(true);
        }

        // Pop up the E key prompt if the player comes near
        if (playerETrigger && eKeyCanvas.transform.localPosition != eMainPos)
        {
            eKeyCanvas.transform.localPosition = Vector3.SmoothDamp(eKeyCanvas.transform.localPosition, eMainPos, ref eVelocity, 0.1f);
            eKeyCanvas.transform.localScale = Vector3.SmoothDamp(eKeyCanvas.transform.localScale, eMainScale, ref eScaleSpeed, 0.1f);
        }
        // Put the E key prompt back if the player goes away
        else if(!playerETrigger && eKeyCanvas.transform.localPosition != Vector3.zero)
        {
            eKeyCanvas.transform.localPosition = Vector3.SmoothDamp(eKeyCanvas.transform.localPosition, Vector3.zero, ref eVelocity, 0.1f);
            eKeyCanvas.transform.localScale = Vector3.SmoothDamp(eKeyCanvas.transform.localScale, Vector3.zero, ref eScaleSpeed, 0.1f);
        }

        // Make the prompt face the player's camera
        if(playerCam)
        {
            eKeyCanvas.transform.LookAt(playerCam.transform, playerCam.transform.up);
        }
    }

    // When the player gets close to the toilet
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerETrigger = true;
            // Get the player, its tool, and its camera
            player = other.gameObject;
            if(player.GetComponent<InventoryManager>().heldTool)
            {
                playerTool = player.GetComponent<InventoryManager>().heldTool.tag;
            }
            playerCam = player.transform.GetChild(0).gameObject;

            EventManager.PressEActions += PlayGame;
            EventManager.pressEActionsCounter++;
        }
    }

    // When the player moves away from the toilet
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isPlaying)
        {
            playerETrigger = false;
            playerCam = null;
            EventManager.PressEActions -= PlayGame;
            EventManager.pressEActionsCounter--;
        }
    }

    

}
