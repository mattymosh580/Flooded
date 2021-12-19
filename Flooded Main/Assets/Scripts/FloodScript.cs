using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodScript : MonoBehaviour
{
    // How tall it's going to be if the level fills with water
    public float endSizeY;

    // The starting Y position of the water object
    float startY;

    // Player variables
    GameObject player;
    characterController charController;
    float playerMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        endSizeY = transform.localScale.y;

        transform.localScale -= new Vector3(0, endSizeY, 0);
        transform.localPosition -= new Vector3(0, endSizeY / 2, 0);

        startY = transform.localPosition.y;
    }

    void Update()
    {
        // Slow down player
        if(player)
        {
            float yDistance = (player.transform.position.y - startY) / transform.localScale.y;
            charController.movementSpeed = Mathf.Clamp(playerMoveSpeed * yDistance / 2, playerMoveSpeed / 3, playerMoveSpeed);
        }
    }

    // Increase the water object size & change its position
    public void Raise(float deltaT)
    {
        if(transform.localScale.y >= endSizeY)
        {
            return;
        }

        float deltaSizeY = endSizeY * deltaT;

        transform.localScale += new Vector3(0, deltaSizeY, 0);
        transform.localPosition += new Vector3(0, deltaSizeY / 2, 0);
    }

    public void Lower(float deltaT)
    {
        if (transform.localScale.y <= 0)
        {
            return;
        }

        float deltaSizeY = transform.localScale.y * deltaT;

        transform.localScale -= new Vector3(0, deltaSizeY, 0);
        transform.localPosition -= new Vector3(0, deltaSizeY / 2, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.gameObject;
            charController = player.GetComponent<characterController>();
            playerMoveSpeed = charController.movementSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            charController.movementSpeed = playerMoveSpeed;

            player = null;
            charController = null;
            playerMoveSpeed = 0;
        }
    }

}
