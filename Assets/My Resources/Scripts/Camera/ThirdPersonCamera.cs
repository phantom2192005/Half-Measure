using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction lookAction;

    public CinemachineFreeLook freeLookCamera;

    public float xSensitivityMouse = 2f;
    public float ySensitivityMouse = 1f;
    public float xSensitivityStick = 120f; // Stick thường nhỏ nên cần nhân lớn hơn
    public float ySensitivityStick = 80f;

    private bool isGamepadInput = false;

    void OnEnable()
    {
        var map = inputActions.FindActionMap("Camera");
        lookAction = map.FindAction("Look");
        lookAction.Enable();

        lookAction.performed += ctx =>
        {
            if (ctx.control.device is Gamepad)
                isGamepadInput = true;
            else if (ctx.control.device is Mouse)
                isGamepadInput = false;
        };
    }

    void Update()
    {
        if (lookAction == null) return;

        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        float xSens = isGamepadInput ? xSensitivityStick : xSensitivityMouse;
        float ySens = isGamepadInput ? ySensitivityStick : ySensitivityMouse;

        freeLookCamera.m_XAxis.Value += lookDelta.x * xSens * Time.deltaTime;
        freeLookCamera.m_YAxis.Value -= lookDelta.y * ySens * Time.deltaTime;
        freeLookCamera.m_YAxis.Value = Mathf.Clamp(freeLookCamera.m_YAxis.Value, 0f, 1f);
    }
}
