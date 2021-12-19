using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameCameraController : MonoBehaviour
{
    cameraController cameraController;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = this.gameObject.GetComponent<cameraController>();
        cameraController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        transform.localEulerAngles = Vector3.zero; 
    }

    private void OnDestroy()
    {
        cameraController.enabled = true;
    }
}
