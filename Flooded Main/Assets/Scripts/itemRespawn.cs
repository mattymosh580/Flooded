using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemRespawn : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        

        if (other.CompareTag("Wrench") || other.CompareTag("Plunger") || other.CompareTag("Bucket"))
        {

            other.transform.position = other.GetComponent<toolPickup>().itemStartPosition;

        }
        else if (other.CompareTag("Pipe"))
        {

            Destroy(other.gameObject);
        }


    }





}
