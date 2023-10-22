using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementTest : MonoBehaviour
{
    [SerializeField] CharacterController character;
    FloatingJoystick joystick;
    [SerializeField] float speed=7f;
    void Awake()
    {
        joystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float x = joystick.Horizontal * Time.deltaTime * speed;
       
        float z = joystick.Vertical * Time.deltaTime * speed;
        if (x!=0 || z!=0)
        {
            Vector3 motion = Vector3.forward * z + Vector3.right * x;
            character.Move(motion);

            transform.rotation = Quaternion.LookRotation(motion, Vector3.up);

        }
   

             
    }
}
