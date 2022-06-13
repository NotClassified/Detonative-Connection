using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
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

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        //INPUT:
        movementInput.z = Input.GetAxisRaw("Vertical");
        movementInput.x = Input.GetAxisRaw("Horizontal");

        if (movementInput.magnitude > 0) //when moving
        {
            if (Input.GetKey(KeyCode.LeftShift)) //when sprinting
                maxVelocity = runningVelocity;
            else
                maxVelocity = walkingVelocity;
            if (velocity < maxVelocity)
                velocity += acceleration * Time.deltaTime;
            else
                velocity = maxVelocity;
        }
        else if (movementInput.magnitude == 0) //when not moving
        {
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

    }
}