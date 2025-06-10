using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour, ISettingsUpdater
{
    [SerializeField] private Transform DrillingMachine;
    
    [SerializeField] private PlayerInput DMCameraInput;
    private InputAction MouseRotation;
    private InputAction LeftClick;
    private InputAction MouseScroll;
    private InputAction HeightNavigation;

    private Quaternion rotation;
    private float distance;
    private float height;
    
    [SerializeField] private SettingsHandler SettingsHandler;
    private int RotationSensitivity;
    private int ZoomSensitivity;
    private int HeightNavigationSensitivity;
    
    private float x;
    private float y;

    
    
    
    
    private void Start()
    {
        SettingsHandler.Add(this);
        UpdateFromSettings();
        
        MouseRotation = DMCameraInput.actions["MouseRotation"];
        LeftClick = DMCameraInput.actions["LeftClick"];
        MouseScroll = DMCameraInput.actions["MouseScroll"];
        HeightNavigation = DMCameraInput.actions["HeightNavigation"];

        rotation = transform.rotation;
        distance = Vector3.Distance(transform.position, DrillingMachine.position);
        height = transform.position.y;
        
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    
    
    
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        bool overUI = EventSystem.current.IsPointerOverGameObject();
        if (LeftClick.IsPressed() && !overUI)
        {
            HandleCamRotation();
            MouseLock();
        }
        else MouseLock(false);
        HandleZoom();
        HandleHeight();
        UpdatePositionRotation();
    }


    
    

    public void UpdateFromSettings()
    {
        Sensibility sensibility = SettingsHandler.Settings.Sensibility;
        RotationSensitivity = sensibility.MouseRotation;
        ZoomSensitivity = sensibility.Zoom;
        HeightNavigationSensitivity = sensibility.HeightNavigation;
    }
    
    
    
    

    private void HandleCamRotation()
    {
        Vector2 MouseRotVal = MouseRotation.ReadValue<Vector2>();
        x += MouseRotVal.x * RotationSensitivity * Time.fixedDeltaTime;
        y -= MouseRotVal.y * RotationSensitivity * Time.fixedDeltaTime;
        y = Mathf.Clamp(y, -90, 90);
        
        rotation = Quaternion.Euler(y, x, 0);
    }

    
    
    

    private void MouseLock(bool locked = true)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }





    private void HandleZoom()
    {
        float ScrollValue = MouseScroll.ReadValue<float>();
        distance += ScrollValue * ZoomSensitivity * Time.fixedDeltaTime;
        distance = Mathf.Clamp(distance, 0, 100);
    }





    private void HandleHeight()
    {
        float HeightNavValue = HeightNavigation.ReadValue<float>();
        height += HeightNavValue * HeightNavigationSensitivity * Time.fixedDeltaTime;
    }


    
    

    private void UpdatePositionRotation()
    {
        Vector3 negDistance = Vector3.back * distance;
        Vector3 DMHeight = DrillingMachine.position + Vector3.up * height;
        Vector3 position = rotation * negDistance + DMHeight;

        transform.rotation = rotation;
        transform.position = position;
    }
}