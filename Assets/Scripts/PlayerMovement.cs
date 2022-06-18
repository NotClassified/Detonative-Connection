using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerMovement : Player
{
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] Slider camXSensitivity;
    [SerializeField] Slider camYSensitivity;

    [SerializeField] Vector3 movementInput;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] float velocity;
    float maxVelocity;
    [SerializeField] float acceleration;
    [SerializeField] float deacceleration;
    [SerializeField] float walkingVelocity;
    [SerializeField] float runningVelocity;
    [SerializeField] float rotationDuration;
    [SerializeField] float rotationVelocity;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameOver)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            return;
        }

        //INPUT:
        movementInput.z = Input.GetAxisRaw("Vertical");
        movementInput.x = Input.GetAxisRaw("Horizontal");

        if (movementInput.magnitude > 0) //when moving
        {
            animator.SetBool("IsWalking", true);
            if (Input.GetKey(KeyCode.LeftShift)) //when sprinting
            {
                maxVelocity = runningVelocity;
                animator.SetBool("IsRunning", true);
            }
            else
            {
                maxVelocity = walkingVelocity;
                animator.SetBool("IsRunning", false);
            }
            if (velocity < maxVelocity)
                velocity += acceleration * Time.deltaTime;
            else
                velocity = maxVelocity;
        }
        else if (movementInput.magnitude == 0) //when not moving
        {
            animator.SetBool("IsWalking", false);
            if (velocity > 0) //slow down
                velocity -= deacceleration * Time.deltaTime;
            else
                velocity = 0; //stop
        }

        if (velocity > 0) //when player is moving
        {
            if (movementInput.magnitude > 0)
            {
                //find angle that the player needs to rotate to
                float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + camMain.eulerAngles.y;
                //smooth the transition of the rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationDuration);
                transform.rotation = Quaternion.Euler(0f, angle, 0f); //apply rotation
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            controller.Move(moveDirection.normalized * Time.deltaTime * velocity); //move player

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            print(transform.eulerAngles.y);
        }
    }

    public void LookAtEnemy(Transform enemy)
    {
        //transform.rotation = Quaternion.LookRotation(enemy.position - transform.position, Vector3.up);
        cam.m_XAxis.Value = Quaternion.LookRotation(enemy.position - transform.position, Vector3.up).eulerAngles.y;
        cam.m_YAxis.Value = .5f;
    }

    public void ChangeCamSensitivity(string axis)
    {
        if (axis.Equals("X"))
        {
            cam.m_XAxis.m_MaxSpeed = camXSensitivity.value;
        }
        else if(axis.Equals("Y"))
        {
            cam.m_YAxis.m_MaxSpeed = camYSensitivity.value;
        }
    }
}