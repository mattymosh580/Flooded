using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCameraController : MonoBehaviour
{

    cameraController cameraController;
    Vector3 pos;

    void Start()
    {
        cameraController = gameObject.GetComponent<cameraController>();
        cameraController.enabled = false;
        pos = transform.parent.transform.position + transform.parent.transform.right * 1.5f;
        pos = pos + transform.parent.transform.forward * 1.5f;
        pos.y -= .5f;
        transform.position = pos;
        transform.LookAt(transform.parent.transform);
        pos = transform.localPosition;
    }

    private void Update()
    {
        transform.LookAt(transform.parent.transform);
    }

    private void OnDestroy()
    {
        cameraController.enabled = true;
    }
}
