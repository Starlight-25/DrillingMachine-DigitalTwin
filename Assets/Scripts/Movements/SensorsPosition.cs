using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorsPosition : MonoBehaviour
{
    [SerializeField] private Transform Sensors;
    [SerializeField] private Transform DrillBitPosSensor;
    [SerializeField] private Transform DepthSensor;

    [SerializeField] private new Transform camera;
    private DrillingMachineMovements DrillingMachineMovements;
    [SerializeField] private GraphHandler GraphHandler;

    private InputAction LeftClickInputAction;
    
    
    
    
    
    private void Start()
    {
        DrillingMachineMovements = GetComponent<DrillingMachineMovements>();

        LeftClickInputAction = GetComponent<PlayerInput>().actions["LeftClick"];
    }

    
    
    

    private void Update()
    {
        DrillBitSensorPos();
        DepthSensorPos();
        SensorOrientation();
        HandleSensorsRayCast();
    }

    
    
    
    
    private void DrillBitSensorPos()
    {
        Vector3 pos = DrillBitPosSensor.position;
        pos.y = DrillingMachineMovements.GetDrillBitHeight();
        DrillBitPosSensor.position = pos;
    }

    private void DepthSensorPos()
    {
        Vector3 pos = DepthSensor.position;
        pos.y = DrillingMachineMovements.GetDepth();
        DepthSensor.position = pos;
    }




    private void SensorOrientation()
    {
        Sensors.LookAt(camera);
        var angles = Sensors.rotation.eulerAngles;
        angles.x = 0f;
        Sensors.rotation = Quaternion.Euler(angles);
    }


    


    private void HandleSensorsRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150f, LayerMask.GetMask("Sensors")) && LeftClickInputAction.triggered)
        {
            GraphHandler.HandleSensorsInteraction(hit.transform.name);
        }
    }
}