using UnityEngine;
using UnityEngine.InputSystem;

public class SensorsPosition : MonoBehaviour
{
    [SerializeField] private Transform Sensors;
    [SerializeField] private Transform DepthSensor;
    [SerializeField] private Transform DrillBitPosSensor;
    [SerializeField] private Transform RTTempSensor;

    private new Camera camera;
    private DrillingMachineMovements DrillingMachineMovements;
    [SerializeField] private GraphHandler GraphHandler;

    private InputAction LeftClickInputAction;

    private int SensorLayer;
    
    
    
    
    
    private void Start()
    {
        camera = Camera.main;
        DrillingMachineMovements = GetComponent<DrillingMachineMovements>();

        LeftClickInputAction = GetComponent<PlayerInput>().actions["LeftClick"];
        SensorLayer = LayerMask.GetMask("Sensors");
    }

    
    
    

    private void Update()
    {
        DrillBitSensorPos();
        DepthSensorPos();
        SensorOrientation();
        RTTempSensorPos();
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

    private void RTTempSensorPos()
    {
        Vector3 pos = RTTempSensor.position;
        pos.y = DrillingMachineMovements.GetRTHeight();
        RTTempSensor.position = pos;
    }




    private void SensorOrientation()
    {
        Sensors.LookAt(camera.transform);
        var angles = Sensors.rotation.eulerAngles;
        angles.x = 0f;
        Sensors.rotation = Quaternion.Euler(angles);
    }


    


    private void HandleSensorsRayCast()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150f, SensorLayer) && LeftClickInputAction.triggered)
        {
            GraphHandler.HandleSensorsInteraction(hit.transform.name);
        }
    }
}