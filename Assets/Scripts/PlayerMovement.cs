using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRigidbody;
    PlayerInput playerInput;
    InputAction move;
    InputAction jump;
    InputAction sprint;
    InputAction look;

    [SerializeField]
    float walkSpeed = 2;

    [SerializeField]
    float sprintSpeed = 5;

    [SerializeField]
    float jumpForce = 5;

    [SerializeField]
    GameObject groundChecker = null;

    [SerializeField]
    LayerMask Ground;

    [SerializeField]
    GameObject cameraRoot = null;

    [SerializeField]
    GameObject playerMesh = null;

    bool isGrounded = true;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();

        move = playerInput.Player.Move;
        jump = playerInput.Player.Jump;
        sprint = playerInput.Player.Sprint;
        look = playerInput.Player.Look;


        Debug.Log("test");
    }

    private void Update()
    {
        Move();

        isGrounded = GroundCheck();

        RotateToCurrentView(playerMesh);
    }

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        sprint.Enable();
        //look.Enable();

        jump.started += OnJump;
    }

    private void OnDisable()
    {
        jump.started -= OnJump;

        move.Disable();
        jump.Disable();
        sprint.Disable();
        //look.Disable();
    }

    private void RotateToCurrentView(GameObject toRotateObject)
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraRoot.transform.eulerAngles.y, 0);

        if (toRotateObject.transform.localRotation != targetRotation)
        toRotateObject.transform.localRotation = Quaternion.Slerp(toRotateObject.transform.localRotation, targetRotation, 4f * Time.deltaTime);
    }

    private void Move()
    {
        if (!move.IsPressed()) return;

        float speed = sprint.inProgress ? sprintSpeed : walkSpeed;

        Vector3 input = new Vector3(move.ReadValue<Vector2>().x, playerRigidbody.velocity.y, move.ReadValue<Vector2>().y);

        input = Quaternion.Euler(0, cameraRoot.transform.localEulerAngles.y, 0) * input;

        Vector3 speedControl = new Vector3(speed, 1, speed);
        input = new Vector3(input.x * speedControl.x, input.y * speedControl.y, input.z * speedControl.z );

        playerRigidbody.velocity = input;

    }

    private void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (!isGrounded) return;

        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool GroundCheck()
    {
        Vector3 checkerPosition = groundChecker.transform.position;
        float checkerRadius = groundChecker.GetComponent<SphereCollider>().radius;

        bool isInGroundContact = Physics.CheckSphere(checkerPosition, checkerRadius, Ground);

        return isInGroundContact;

    }







}
