using UnityEngine;
using UnityEngine.InputSystem;

public class SensorsPosition : MonoBehaviour
{
    [SerializeField] private Transform Sensors;
    [SerializeField] private Transform DepthSensor;
    [SerializeField] private Transform DrillBitPosSensor;
    [SerializeField] private Transform RTTempSensor;
    [SerializeField] private Transform STTempSensor;
    private MeshRenderer[] SensorMeshes;

    private Color SensorColor = new Color(0x00 / 255f, 0xFF / 255f, 0xFF / 255f); //00FFFF
    private Color SensorHoverColor = new Color(0x00 / 255f, 0xFF / 255f, 0x80 / 255f); //00FF80

    private new Camera camera;
    private DrillingMachineMovements DrillingMachineMovements;
    [SerializeField] private GraphHandler GraphHandler;

    private InputAction LeftClickInputAction;

    private int SensorLayer;
    
    
    
    
    
    private void Start()
    {
        SensorMeshes = new MeshRenderer[Sensors.childCount];
        for (int i = 0; i < SensorMeshes.Length; i++)
            SensorMeshes[i] = Sensors.GetChild(i).GetComponent<MeshRenderer>();
        
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
        STTempSensorPos();
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

    private void STTempSensorPos()
    {
        Vector3 pos = STTempSensor.position;
        pos.y = DrillingMachineMovements.GetSTHeight();
        STTempSensor.position = pos;
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
        if (Physics.Raycast(ray, out hit, 150f, SensorLayer))
        {
            ChangeSensorHoverMaterial(hit.transform);
            if (LeftClickInputAction.triggered) GraphHandler.HandleSensorsInteraction(hit.transform.name);
        }
        else ChangeSensorHoverMaterial();
    }

    private void ChangeSensorHoverMaterial(Transform sensor = null)
    {
        if (sensor is not null) sensor.GetComponent<MeshRenderer>().material.color = SensorHoverColor;
        else foreach (MeshRenderer sensorMesh in SensorMeshes) sensorMesh.material.color = SensorColor;
    }
}