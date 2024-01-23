using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerspeed = 1.9f;
    public float currentPlayerspeed;
    public float playerSprint = 3f;
    public float CurrentPlayerSprint = 0;
    [Header("Player Animator and Gravity")]
    public Animator animator;
    public float gravity = -9.81f;
    public CharacterController CC;
    [Header("Player Jumping And Velocity")]
    public float turnCalmTime = 0.7f;
    public float turnCalmVelocity;
    public float surfaceDistance = 0.4f;
    Vector3 velocity;
    public Transform SurfaceCheck;
    bool OnSurface;
    public LayerMask SurfaceMask;
    public float Jumprange = 1f;
    [Header("Player Camera")]
    public Transform PlayerCamera;
   

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void PlayerMove()
    {
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        Vector3 Direction = new Vector3(HorizontalAxis, 0f, VerticalAxis).normalized;

        if (Direction.magnitude >= 0.1f)
        {
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
            animator.SetBool("idle", false);
            animator.SetBool("aimwalk", false);

            // Calculate the target angle
            float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + PlayerCamera.eulerAngles.y;

            // Smoothly rotate the player towards the target angle
            float rotationSpeed = 10f; // Adjust the rotation speed as needed
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, TargetAngle, 0f), Time.deltaTime * rotationSpeed);

            // Move the player
            Vector3 MoveDirection = Quaternion.Euler(0f, TargetAngle, 0f) * Vector3.forward;
            CC.Move(MoveDirection.normalized * playerspeed * Time.deltaTime);
            currentPlayerspeed = playerspeed;
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            currentPlayerspeed = 0;
        }
    }






    void Update()
    {
        OnSurface = Physics.CheckSphere(SurfaceCheck.position, surfaceDistance, SurfaceMask);
        if (OnSurface && velocity.y==0) 
        { 
           velocity.y=-2f;               

        }

        //gravity
        velocity.y+= gravity * Time.deltaTime;
        CC.Move(velocity * Time.deltaTime);




        Jump();
        PlayerMove();
        Sprint();
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && OnSurface)
        {
            animator.SetBool("walk", false);
            animator.SetTrigger("jump");
            velocity.y = Mathf.Sqrt(Jumprange * -2f * gravity );
        }
        else
        {
            animator.ResetTrigger("jump");

        }
    }
    // sprinting on player
    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && OnSurface)
        {
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
            animator.SetBool("idle", false);

            float HorizontalAxis = Input.GetAxisRaw("Horizontal");
            float VerticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 Direction = new Vector3(HorizontalAxis, 0f, VerticalAxis).normalized;

            if (Direction.magnitude >= 0.1f)
            {
                // Calculate the target angle
                float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + PlayerCamera.eulerAngles.y;

                // Smoothly rotate the player towards the target angle
                float rotationSpeed = 10f; // Adjust the rotation speed as needed
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, TargetAngle, 0f), Time.deltaTime * rotationSpeed);

                // Move the player
                Vector3 MoveDirection = Quaternion.Euler(0f, TargetAngle, 0f) * Vector3.forward;
                CC.Move(MoveDirection.normalized * playerSprint * Time.deltaTime);
                CurrentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("walk", false);
                animator.SetBool("idle", true);
                currentPlayerspeed = 0f;
            }
        }
    }

    //playerhitdamage
    //playerdie




}
