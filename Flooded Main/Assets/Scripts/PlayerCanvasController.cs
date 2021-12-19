using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().worldCamera = this.transform.parent.gameObject.GetComponentInChildren<Camera>();
        this.GetComponent<Canvas>().planeDistance = 1;
    }
}
