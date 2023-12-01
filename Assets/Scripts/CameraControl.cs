using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    PlayerInput input;
    InputAction look;

    Vector3 currentRotation;

    [SerializeField]
    float mouseSpeed = 40f;

    void Awake()
    {
        input = new PlayerInput();
        look = input.Player.Look;

        currentRotation = transform.localEulerAngles;
    }

    private void OnEnable()
    {
        look.Enable();

        look.performed += OnLook;
    }

    private void OnDisable()
    {
        look.performed -= OnLook;

        look.Disable();
    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        Vector2 mouseMove = callbackContext.ReadValue<Vector2>();

        currentRotation += new Vector3(-mouseMove.y, mouseMove.x, 0f) * mouseSpeed * Time.deltaTime;
        currentRotation = new Vector3(Mathf.Clamp(currentRotation.x, -90, 90), currentRotation.y, 0f);

        transform.localEulerAngles = currentRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
