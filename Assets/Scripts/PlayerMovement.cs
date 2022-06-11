using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    [SerializeField] Vector3 movementInput;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 velocity;
    [SerializeField] float maxVelocity;
    [SerializeField] float acceleration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //LEFT AND RIGHT MOVEMENT:
        movementInput.x = Input.GetAxis("Horizontal");
        if (movementInput.x > 0 && velocity.x < maxVelocity)
            velocity.x += acceleration * Time.deltaTime;
        if (movementInput.x < 0 && velocity.x > -maxVelocity)
            velocity.x -= acceleration * Time.deltaTime;
        if (movementInput.x == 0)
        {
            if(velocity.x > 0)
                velocity.x -= acceleration * Time.deltaTime;
            if (velocity.x < 0)
                velocity.x += acceleration * Time.deltaTime;
            if(velocity.x < .01 && velocity.x > -.01) //deadzone
                velocity.x = 0;
        }


        //FORWARD AND BACK MOVEMENT:
        movementInput.z = Input.GetAxis("Vertical");
        if (movementInput.z > 0 && velocity.z < maxVelocity)
            velocity.z += acceleration * Time.deltaTime;
        if (movementInput.z < 0 && velocity.z > -maxVelocity)
            velocity.z -= acceleration * Time.deltaTime;
        if (movementInput.z == 0)
            velocity.z = 0;
        if(movementInput.x != 0 && movementInput.z != 0)
            movement = velocity.normalized * Time.deltaTime;
        else
            movement = velocity * Time.deltaTime;

        controller.Move(movement);
    }
}
/*
if (velocityZ < 0) //prevent forward velocity from being negative
    velocityZ = 0;
if (velocityZ < maxWalkVelocity && pt.isGrounded && !pt.isClimbing) //increase forward velocity when below walking speed and is grounded
    velocityZ += (walkAcceleration / 10) * Time.deltaTime;
else if (velocityZ < maxRunVelocity && pt.isGrounded && !pt.isClimbing) //increse forward velocity when below running speed and is grounded
    velocityZ += (runAcceleration / 10) * Time.deltaTime;
if (velocityZ != 9 && pt.AnimCheck(0, "Land1") || pt.isClimbing) //setting forward velocity when landing or climbing successfully
{
    velocityZ = 9;
}
else if (velocityZ != 6 && pt.AnimCheck(0, "Land2")) //setting velocity when landing unsuccessfully
    velocityZ = 3;
animator.SetFloat(hashVelocityZ, velocityZ); //correlating variables with animater
*/