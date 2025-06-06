using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform DrillingMachine;
    
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction MouseRotation;
    private InputAction LeftClick;
    private InputAction MouseScroll;

    private float distance;
    private float height;
    private float RotationSensitivity = 60f;
    private float ZoomSensitivity = 120f;
    
    private float x;
    private float y;

    
    
    
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        MouseRotation = PlayerInput.actions["MouseRotation"];
        LeftClick = PlayerInput.actions["LeftClick"];
        MouseScroll = PlayerInput.actions["MouseScroll"];

        distance = Vector3.Distance(transform.position, DrillingMachine.position);
        height = transform.position.y;
        
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    
    
    
    
    private void Update()
    {
        if (LeftClick.IsPressed())
        {
            HandleCamRotation();
            MouseLock();
        }
        else MouseLock(false);
        HandleZoom();
    }


    
    

    private void HandleCamRotation()
    {
        Vector2 MouseRotVal = MouseRotation.ReadValue<Vector2>();
        x += MouseRotVal.x * RotationSensitivity * Time.deltaTime;
        y -= MouseRotVal.y * RotationSensitivity * Time.deltaTime;
        y = Mathf.Clamp(y, -90, 90);
        
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        
        Vector3 negDistance = Vector3.back * distance;
        Vector3 DMHeight = DrillingMachine.position + Vector3.up * height;
        Vector3 position = rotation * negDistance + DMHeight;

        transform.rotation = rotation;
        transform.position = position;
    }

    
    
    

    private void MouseLock(bool locked = true)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }





    private void HandleZoom()
    {
        float ScrollValue = MouseScroll.ReadValue<float>();
        distance += ScrollValue * ZoomSensitivity * Time.deltaTime;
        distance = Mathf.Clamp(distance, 0, 100);
        
        Vector3 negDistance = Vector3.back * distance;
        Vector3 DMHeight = DrillingMachine.position + Vector3.up * height;
        transform.position = transform.rotation * negDistance + DMHeight;
    }
}