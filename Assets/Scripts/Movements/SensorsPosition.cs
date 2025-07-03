using UnityEngine;
using UnityEngine.InputSystem;

public class SensorsPosition : MonoBehaviour
{
    [SerializeField] private Transform Sensors;
    [SerializeField] private Transform DepthSensor;
    [SerializeField] private Transform DrillBitPosSensor;
    [SerializeField] private Transform[] RTSensors;
    [SerializeField] private Transform[] STSensors;
    private MeshRenderer[] SensorMeshes;

    private Color SensorColor = new Color(0x00 / 255f, 0xFF / 255f, 0xFF / 255f); //00FFFF
    private Color SensorHoverColor = new Color(0x00 / 255f, 0xFF / 255f, 0x40 / 255f); //00FF80

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
        RTSensorsPos();
        STSensorsPos();
        HandleSensorsRayCast();
    }

    
    
    
    
    private void ChangeSensorPosition(Transform sensor, float height)
    {
        Vector3 pos = sensor.position;
        pos.y = height;
        sensor.position = pos;
    }





    private void DrillBitSensorPos() =>
        ChangeSensorPosition(DrillBitPosSensor, DrillingMachineMovements.GetDrillBitHeight());

    private void DepthSensorPos() => ChangeSensorPosition(DepthSensor, DrillingMachineMovements.GetDepth());

    private void RTSensorsPos()
    {
        float RTHeight = DrillingMachineMovements.GetRTHeight();
        foreach (Transform rtSensor in RTSensors)
            ChangeSensorPosition(rtSensor, RTHeight);
    }

    private void STSensorsPos()
    {
        float STHeight = DrillingMachineMovements.GetSTHeight();
        foreach (Transform stSensor in STSensors)
            ChangeSensorPosition(stSensor, STHeight);
    }




    private void SensorOrientation()
    {
        Sensors.LookAt(camera.transform);
        Vector3 angles = Sensors.rotation.eulerAngles;
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





    public void ChangeSensorsVisibility(bool visible)
    {
        foreach (MeshRenderer sensorMesh in SensorMeshes)
            sensorMesh.enabled = visible;
    }
}