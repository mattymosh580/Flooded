using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class characterController : MonoBehaviour
{
    CharacterController controller;

    public float movementSpeed = 5;
    [SerializeField]
    float jumpHeight = 0.35f;
    //[SerializeField]
    //float rotateSpeed = 120;
    Animator animator;

    Vector3 playerGravity;
    
    bool isOnGround;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        animator = transform.GetChild(1).gameObject.GetComponent<Animator>();
        animator.SetFloat("Speed_f", 0);
    }

    private void Update()
    {
        if (!controller.isGrounded)
        {
            playerGravity.y -= 0.098f * Time.deltaTime;
        }
        else
        {
            if(Input.GetButtonDown("Jump"))
            {
                playerGravity.y = jumpHeight * 0.098f;
            }
        }

        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            animator.SetFloat("Speed_f", .6f);
        }
        else
        {
            animator.SetFloat("Speed_f", 0);
        }

        //transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeed = movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        right *= curSpeed;
        controller.Move(right + playerGravity);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        curSpeed = movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        forward *= curSpeed;
        controller.Move(forward + playerGravity);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        Vector3 push = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = push * movementSpeed / 2;
    }

    void OnCollisionExit(Collision coll)
    {
        if (isOnGround)
        {
            isOnGround = false;
        }
    }
}
