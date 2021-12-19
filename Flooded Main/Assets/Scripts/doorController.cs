using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    //GameObject newText;

    Animator doorAnimator;

    List<GameObject> playerObjects = new List<GameObject>();


    private void Awake()
    {


        if (EventManager.pressEActionsCounter != 0)
        {
            EventManager.PressEActions -= OpenDoor;
            EventManager.pressEActionsCounter--;
        }
    }

    private void Start()
    {
        
        doorAnimator = door.GetComponent<Animator>();

    }

    void OnDisable()
    {
        EventManager.PressEActions -= OpenDoor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //      DYNAMIC UI CANVAS INSTANTIATION
            //
            //newText = EventManager.WritePressE();

            playerObjects.Add(other.gameObject);
            EventManager.PressEActions += OpenDoor;
            EventManager.pressEActionsCounter++;

            
        }
    }

    
    //      FUNCTION THAT CLEANS UP DOOR FUNCTIONALITY WHEN PLAYER LEAVES TRIGGER
    //
    private void OnTriggerExit(Collider other)
    {
        //      ALTERNATE FUNCTIONALITY FOR DOOR CLOSING  
        //
        //    if (other.gameObject.tag == "Player")
        //    {
        //        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("door1OpenPositive"))
        //        {
        //            doorAnimator.SetTrigger("CloseDoorPositive");
        //            Debug.Log("True");
        //        }
        //        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("door1OpenNegative"))
        //        {
        //            doorAnimator.SetTrigger("CloseDoorNegative");
        //            Debug.Log("True");
        //        }
        //    }

        //Destroy(newText);
        playerObjects.Remove(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            EventManager.PressEActions -= OpenDoor;
            EventManager.pressEActionsCounter--;
        }
    }

    //      FUNTION THAT SETS DOOR ANIMATION STATES BASED ON PLAYER POSITION RELATIVE TO DOOR
    //
    void OpenDoor()
    {
        Vector3 localDirection = transform.InverseTransformDirection((transform.position - playerObjects[0].transform.position).normalized);
        if (localDirection.z > 0)
        {

            doorAnimator.SetTrigger("OpenDoorNegative");
        }
        else
        {
            doorAnimator.SetTrigger("OpenDoorPositive");
        }
    }
}
