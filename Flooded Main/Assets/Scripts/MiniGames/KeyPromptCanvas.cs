using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPromptCanvas : MonoBehaviour
{
    Text ePrompt;
    Image toolImage;
    [SerializeField]
    Sprite toolSprite;
    GameObject playerCam;

    // Canvas' active state
    bool active;

    // If the player has all the correct tools, if applicable
    // (Set it using the parent object's script)
    public bool hasCorrectTools { get; set; }

    Vector3 scaleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Get E key prompt and tool image prompt
        Transform ima = transform.GetChild(0).GetChild(0);
        ePrompt = ima.GetChild(0).GetComponent<Text>();
        toolImage = ima.GetChild(1).GetComponent<Image>();

        transform.localScale = Vector3.zero;

        toolImage.sprite = toolSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the displayed image, if needed
        if(hasCorrectTools && !ePrompt.enabled)
        {
            ePrompt.enabled = true;
            toolImage.enabled = false;
        }
        else if(!hasCorrectTools && !toolImage.enabled)
        {
            ePrompt.enabled = false;
            toolImage.enabled = true;
        }

        // Show/hide canvas with smooth movement
        if(active && transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one, ref scaleSpeed, 0.1f);
        }
        else if (!active && transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref scaleSpeed, 0.1f);
        }

        // Look at player camera
        if(playerCam)
        {
            transform.LookAt(playerCam.transform, playerCam.transform.up);
        }
    }

    // Show canvas
    public void Show(GameObject playerCamera)
    {
        active = true;
        playerCam = playerCamera;
    }

    // Hide canvas
    public void Hide()
    {
        active = false;
        playerCam = null;
    }
}
