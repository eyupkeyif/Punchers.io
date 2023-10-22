using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private CharacterController charController;

    private Joystick joystick;

    private Vector3 moveVec;

    public float movementSpeed=8f;

    public bool canMove;


    private void Awake()
    {
        canMove = true;
    }


    private void Start()
    {
        SetupComponents();
    }

    public void SetupStats(Stats stats)
    {
        movementSpeed = stats.movementSpeed;
    }

    private void SetupComponents()
    {
        charController = GetComponent<CharacterController>();

        joystick = FindObjectOfType<Joystick>();
        joystick.enabled = true;
    }

    private void Update()
    {
        if (GameManager.isGameStarted && !GameManager.isGameOver && canMove)
        {
            Move();
        }
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void Move()
    {
        Vector2 input = joystick.GetInput();
        moveVec = new Vector3(input.x, transform.position.y, input.y).normalized;

        charController.Move(moveVec * movementSpeed* Time.deltaTime);

        if (moveVec != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveVec, Time.deltaTime * 50f);
        }


    }

    public void PointerDown()
    {

    }

    public void PointerUp()
    {

    }

    private void SubscribeEvents()
    {
        if (joystick == null)
        {
            Debug.Log("Joystick Missing");

            return;
        }

        joystick.pointerDown += PointerDown;
        joystick.pointerUp += PointerUp;
    }
}
