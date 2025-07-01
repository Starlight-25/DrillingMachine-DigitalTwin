using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorBarSensor : MonoBehaviour
{
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    
    [SerializeField] private MeshRenderer DrillBitMesh;
    [SerializeField] private Gradient colorGradient;
    private int prevIndex = -1;
    private float maxValue;
    private float minValue;
    private SensorType sensorType = SensorType.DBTorque;
    
    private enum SensorType
    {
        DBTorque,
        WeightOnBit
    }

    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
        ChangeSensor();
    }


    
    
    
    private void Update()
    {
        if (prevIndex != DrillingDataManager.Index)
        {
            prevIndex = DrillingDataManager.Index;
            SetDBColor();
        }
    }

    
    
    

    public void DBTorqueDataButtonClicked()
    {
        sensorType = SensorType.DBTorque;
        ChangeSensor();
    }
    
    
    
    
    
    public void WeightOnBitDataButtonClicked()
    {
        sensorType = SensorType.WeightOnBit;
        ChangeSensor();
    }

    
    
    

    private void ChangeSensor()
    {
        if (sensorType == SensorType.DBTorque)
        {
            minValue = DrillingData.Min(data => data.DB_Torque);
            maxValue = DrillingData.Max(data => data.DB_Torque);
        }
        else
        {
            minValue = DrillingData.Min(data => data.WeightOnBit);
            maxValue = DrillingData.Max(data => data.WeightOnBit);
        }
        //change in UI
    }
    
    
    
    
    
    private void SetDBColor()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        float t = Mathf.InverseLerp(minValue, maxValue, sensorType == SensorType.DBTorque ? curDrillingData.DB_Torque : curDrillingData.WeightOnBit);
        DrillBitMesh.material.color = colorGradient.Evaluate(t);
    }
}