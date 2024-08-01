using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu



public class FPSInput : MonoBehaviour
{

    public float jumpSpeed = 15.0f;
    public float terminalVelocity = -20.0f;
    private float vertSpeed;
    // movement sensitivity
    public float speed = 6.0f;

    // gravity setting
    public float gravity = -9.8f;

    // reference to the character controller
    private CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // changes based on WASD keys
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // make diagonal movement consistent
        movement = Vector3.ClampMagnitude(movement, speed);


        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }
        else if (!charController.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }
        // add gravity in the vertical direction
        movement.y = vertSpeed;

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // transform from local space to global space
        movement = transform.TransformDirection(movement);


        

        // pass the movement to the character controller
        charController.Move(movement);
    }
}
