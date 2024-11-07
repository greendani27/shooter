using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //TODO cambiar las 2 variables para la ejecucion de animaciones por un vector2

    public Camera pov;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    //public Transform leftCheck;
    //public Transform rightCheck;
    //public LayerMask sideMask;
    //public Transform centerCheck;
    public Animator playerAnimationController;
    private Vector3 initialCameraPosition;

    [Header("Movement parameters")]
    [SerializeField]
    private float walkSpeed = 7f;
    [SerializeField]
    private float crouchSpeed = 3f;
    [SerializeField]
    private float runningSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 2f;

    [Header("Jumping parameters")]
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float groundDistance = 0.4f;

    [Header("Crouch parameters")]
    [SerializeField]
    private float standingHeight = 1.8f;
    [SerializeField]
    private float crouchingHeight = 0.9f;
    [SerializeField]
    //private float sideDistance = 0.2f;

    private float speed;
    private float WalkX;
    private float WalkZ;
    private float animationStateAcceleration = 5f;
    private float animationStateDecceleration = -5f;

    private bool canDoubleJump = true;
    private bool isGrounded;
    //bool isTouchingRightWall;
    //bool isTouchingLeftWall;

    Vector3 velocity;

    private Vector2 axis;

    private void Start()
    {
        initialCameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
    }

    private void Update()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * axis.x + transform.forward * axis.y;

        

        speed = walkSpeed;

        if (movement == Vector3.zero)
        {
            StopsMoveAnimation();
        }
        else 
        {
            StartsMoveAnimation(axis.x, axis.y);

            if (Input.GetKey(KeyCode.LeftShift) && axis.y > 0)
            {
                playerAnimationController.SetBool("Running", true);
                speed = runningSpeed;

                if (Input.GetKey(KeyCode.LeftControl)) {
                    
                }
            }
            else {
                playerAnimationController.SetBool("Running", false);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
            playerAnimationController.SetBool("Crouching", true);
            controller.height = crouchingHeight;
        }
        else {
            playerAnimationController.SetBool("Crouching", false);
            controller.height = standingHeight;
        }

        playerAnimationController.SetFloat("Walk X", WalkX);
        playerAnimationController.SetFloat("Walk Z", WalkZ);

        controller.Move(Time.deltaTime * speed * movement);

        Jump();
    }

    private void Jump() {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            canDoubleJump = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void StartsMoveAnimation(float x, float z) {
        if (WalkX < 1 && z > 0)
            WalkX += Time.deltaTime * animationStateAcceleration;

        if (WalkX > -1 && z < 0)
            WalkX -= Time.deltaTime * animationStateAcceleration;

        if (WalkZ < 1 && x > 0)
            WalkZ += Time.deltaTime * animationStateAcceleration;

        if (WalkZ > -1 && x < 0)
            WalkZ -= Time.deltaTime * animationStateAcceleration;
    }

    private void StopsMoveAnimation() {
        if (WalkX > 0)
        {
            WalkX += Time.deltaTime * animationStateDecceleration;
        }
        else if (WalkX < 0)
        {
            WalkX -= Time.deltaTime * animationStateDecceleration;
        }


        if (WalkZ > 0)
        {
            WalkZ += Time.deltaTime * animationStateDecceleration;
        }
        else if (WalkZ < 0)
        {
            WalkZ -= Time.deltaTime * animationStateDecceleration;
        }
        //WalkX = 0;
        //WalkZ = 0;
    }

}



/*isTouchingLeftWall = Physics.CheckSphere(leftCheck.position, sideDistance, sideMask);
isTouchingRightWall = Physics.CheckSphere(rightCheck.position, sideDistance, sideMask);



if (!isGrounded) {
    if (isTouchingRightWall)
    {
        //TODO hacerlo sin limite de velocidad


    }
    else if (isTouchingLeftWall) 
    { 

    }
}*/