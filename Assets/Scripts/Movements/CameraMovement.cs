using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform DrillingMachine;
    
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction MouseRotation;
    private InputAction LeftClick;
    
    private Vector3 negDistance;
    private float height;
    public float Sensitivity = 120f;
    
    private float x;
    private float y;

    
    
    
    
    private void Start()
    {
        MouseRotation = PlayerInput.actions["MouseRotation"];
        LeftClick = PlayerInput.actions["LeftClick"];
        
        negDistance = Vector3.back * Vector3.Distance(transform.position, DrillingMachine.position);
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
    }


    
    

    private void HandleCamRotation()
    {
        Vector2 MouseRotVal = MouseRotation.ReadValue<Vector2>();
        x += MouseRotVal.x * Sensitivity * Time.deltaTime;
        y -= MouseRotVal.y * Sensitivity * Time.deltaTime;
        y = Mathf.Clamp(y, -90, 90);
        
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        
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
}