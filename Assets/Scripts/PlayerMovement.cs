using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    [SerializeField] Vector3 movementInput;
    [SerializeField] Vector3 velocity;
    [SerializeField] float maxVelocity;
    [SerializeField] float maxWalkingVelocity;
    [SerializeField] float maxRunningVelocity;
    [SerializeField] float inputAcceleration;
    [SerializeField] float maxVelocityAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (maxVelocity < maxRunningVelocity)
                    maxVelocity += maxVelocityAcceleration * Time.deltaTime;
                else if (maxVelocity > maxRunningVelocity)
                    maxVelocity = maxRunningVelocity;
            }
            else
            {
                if (maxVelocity > maxWalkingVelocity)
                    maxVelocity -= maxVelocityAcceleration * Time.deltaTime;
                else if (maxVelocity < maxWalkingVelocity)
                    maxVelocity = maxWalkingVelocity;
            }
        }
        else
            maxVelocity = maxWalkingVelocity;

        //LEFT AND RIGHT MOVEMENT:
        movementInput.x = Input.GetAxis("Horizontal");
        if (movementInput.x > 0 && velocity.x < 1) //moving right
            velocity.x += inputAcceleration * Time.deltaTime; 
        if (movementInput.x < 0 && velocity.x > -1) //moving left
            velocity.x -= inputAcceleration * Time.deltaTime;
        if (movementInput.x == 0) //not moving
        {
            if(velocity.x > 0) //stopping from moving right
                velocity.x -= inputAcceleration * Time.deltaTime;
            if (velocity.x < 0) //stopping from moving left
                velocity.x += inputAcceleration * Time.deltaTime;
            if(velocity.x < .1 && velocity.x > -.1) //stop completely (.1 velocity is the deadzone)
                velocity.x = 0;
        }

        //FORWARD AND BACK MOVEMENT:
        movementInput.z = Input.GetAxis("Vertical");
        if (movementInput.z > 0 && velocity.z < 1) //moving forward
            velocity.z += inputAcceleration * Time.deltaTime;
        if (movementInput.z < 0 && velocity.z > -1) //moving backward
            velocity.z -= inputAcceleration * Time.deltaTime;
        if (movementInput.z == 0) //not moving
        {
            if (velocity.z > 0) //stopping from moving forward
                velocity.z -= inputAcceleration * Time.deltaTime;
            if (velocity.z < 0) //stopping from moving backward
                velocity.z += inputAcceleration * Time.deltaTime;
            if (velocity.z < .1 && velocity.z > -.1) //stop completely (.1 velocity is the deadzone)
                velocity.z = 0;
        }

        if (velocity.magnitude > 1) //normalize velocity when moving diagonally
            velocity.Normalize();
        controller.Move(velocity * Time.deltaTime * maxVelocity); //move player

    }
}
/*
if (velocityZ < 0) //prevent forward velocity from being negative
    velocityZ = 0;
//increase forward velocity when below walking speed and is grounded
if (velocityZ < maxWalkVelocity && pt.isGrounded && !pt.isClimbing) 
    velocityZ += (walkAcceleration / 10) * Time.deltaTime;
//increse forward velocity when below running speed and is grounded
else if (velocityZ < maxRunVelocity && pt.isGrounded && !pt.isClimbing) 
    velocityZ += (runAcceleration / 10) * Time.deltaTime;
//setting forward velocity when landing or climbing successfully
if (velocityZ != 9 && pt.AnimCheck(0, "Land1") || pt.isClimbing) 
{
    velocityZ = 9;
}
else if (velocityZ != 6 && pt.AnimCheck(0, "Land2")) //setting velocity when landing unsuccessfully
    velocityZ = 3;
animator.SetFloat(hashVelocityZ, velocityZ); //correlating variables with animater
*/