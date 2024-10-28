using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera pov;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    //public Transform leftCheck;
    //public Transform rightCheck;
    //public LayerMask sideMask;
    //public Transform centerCheck;
    public Animator playerAnimationController;

    public float walkSpeed = 7f;
    public float crouchSpeed = 3f;
    public float runningSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    //public float sideDistance = 0.2f;
    private float speed;
    private float WalkX;
    private float WalkY;
    private float animationStateAcceleration = 5f;
    private float animationStateDecceleration = -5f;

    private bool doubleJump = true;
    bool isGrounded;
    //bool isTouchingRightWall;
    //bool isTouchingLeftWall;

    Vector3 velocity;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;

        if (movement == Vector3.zero)
        {
            StopsWalkAnimation();
        }
        else 
        {

            StartsWalkAnimation(x, z);

            speed = walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (WalkX < 0.2)
                    WalkX += Time.deltaTime * animationStateAcceleration;

                speed = runningSpeed;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
            playerAnimationController.SetBool("Crouching", true);
        }
        else {
            
            playerAnimationController.SetBool("Crouching", false);
            speed = walkSpeed;
        }

        playerAnimationController.SetFloat("Walk Y", WalkX);
        playerAnimationController.SetFloat("Walk X", WalkY);

        controller.Move(Time.deltaTime * speed * movement);

        Jump();
    }

    private void Jump() {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
            doubleJump = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && doubleJump)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            doubleJump = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void StartsWalkAnimation(float x, float z) {
        if (WalkX < 1 && z > 0)
            WalkX += Time.deltaTime * animationStateAcceleration;

        if (WalkX > -1 && z < 0)
            WalkX -= Time.deltaTime * animationStateAcceleration;

        if (WalkY < 1 && x > 0)
            WalkY += Time.deltaTime * animationStateAcceleration;

        if (WalkY > -1 && x < 0)
            WalkY -= Time.deltaTime * animationStateAcceleration;
    }

    private void StopsWalkAnimation() {
        if (WalkX > 0)
        {
            WalkX += Time.deltaTime * animationStateDecceleration;
        }
        else if (WalkX < 0)
        {
            WalkX -= Time.deltaTime * animationStateDecceleration;
        }


        if (WalkY > 0)
        {
            WalkY += Time.deltaTime * animationStateDecceleration;
        }
        else if (WalkY < 0)
        {
            WalkY -= Time.deltaTime * animationStateDecceleration;
        }
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