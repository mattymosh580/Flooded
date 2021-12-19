using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour
{
    float pitch = 0;
    float yaw = 0;
    [SerializeField]
    float yawRotationSpeed;
    [SerializeField]
    float pitchRotationSpeed;
    RaycastHit hit;
    GameObject hitObject;
    GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair = GameObject.Find("Crosshair");
    }

    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, .8f, -.5f);
        transform.forward = transform.parent.forward;
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            FollowMouse();
        }
    }

    private void FixedUpdate()
    {
        DetectObjectInCrosshair();
    }

    private void FollowMouse()
    {
        yaw += Input.GetAxis("Mouse X") * yawRotationSpeed;
        pitch += Input.GetAxis("Mouse Y") * pitchRotationSpeed;
        pitch = Mathf.Clamp(pitch, -45, 45);

        transform.parent.localEulerAngles = new Vector3(0, yaw, 0);

        transform.localEulerAngles = new Vector3(-pitch, 0, 0);
    }

    private void DetectObjectInCrosshair()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.forward;
        if (Physics.Raycast(ray, out hit, 1.5f, LayerMask.GetMask("CrosshairHit"), QueryTriggerInteraction.Collide))
        {
            crosshair.GetComponent<Image>().color = new Color(crosshair.GetComponent<Image>().color.r, crosshair.GetComponent<Image>().color.g, crosshair.GetComponent<Image>().color.b, .5f);
            if (hitObject != null)
            {
                if (hit.transform.gameObject != hitObject)
                {
                    if (hitObject.CompareTag("Pipe Generator"))
                    {
                        hitObject.GetComponent<PickupBoxController>().TriggerExitActions();
                    }
                    else if (hitObject.CompareTag("Wrench") || hitObject.CompareTag("Bucket") || hitObject.CompareTag("Plunger"))
                    {
                        hitObject.GetComponent<toolPickup>().TriggerExitActions();
                    }
                    else if (hitObject.CompareTag("Pipe"))
                    {
                        hitObject.GetComponent<ItemPickup>().TriggerExitActions();
                    }
                }
            }
            hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Pipe Generator"))
            {
                hitObject.GetComponent<PickupBoxController>().TriggerActions(transform.parent.gameObject);
            }
            else if (hitObject.CompareTag("Wrench") || hitObject.CompareTag("Bucket") || hitObject.CompareTag("Plunger"))
            {
                hitObject.GetComponent<toolPickup>().TriggerActions(transform.parent.gameObject);
            }
            else if (hitObject.CompareTag("Pipe"))
            {
                hitObject.GetComponent<ItemPickup>().TriggerActions(transform.parent.gameObject);
            }
        }
        else
        {
            crosshair.GetComponent<Image>().color = new Color(crosshair.GetComponent<Image>().color.r, crosshair.GetComponent<Image>().color.g, crosshair.GetComponent<Image>().color.b, .2f);
            if (hitObject != null)
            {
                if (hitObject.CompareTag("Pipe Generator"))
                {
                    hitObject.GetComponent<PickupBoxController>().TriggerExitActions();
                }
                else if (hitObject.CompareTag("Wrench") || hitObject.CompareTag("Bucket") || hitObject.CompareTag("Plunger"))
                {
                    hitObject.GetComponent<toolPickup>().TriggerExitActions();
                }
                else if (hitObject.CompareTag("Pipe"))
                {
                    hitObject.GetComponent<ItemPickup>().TriggerExitActions();
                }
                hitObject = null;
            }
        }
    }

}