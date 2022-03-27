using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 10f;
    private float moveHorizontal;
    private float moveVertical;
    private CharacterController controller;
    public float gravity = -9.8f;


    Vector3 velocity;

    //Used by CameraBobbing and HandBobbing scripts
    public static bool isWalking = false;
    public static bool isSprinting = false;

    //Sprinting Movement
    [Header("Sprint")]
    private float sprintMeter = 60;
    public float stamina;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Inventory.active)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            //Debug.Log("Movement " + moveHorizontal + " " + moveVertical);

            isWalking = moveVertical != 0 || moveHorizontal != 0;
            isSprinting = Input.GetButton("Sprint") && sprintMeter > 0;

            //Debug.Log("Walking " + isWalking + " Sprinting " + isSprinting);

            Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

            if (isSprinting)
            {
                controller.Move(move * playerSpeed * 2 * Time.deltaTime);
                sprintMeter -= stamina;
            }
            else
            {
                sprintMeter = 60;
                controller.Move(move * playerSpeed * Time.deltaTime);
            }


            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

    }
}
