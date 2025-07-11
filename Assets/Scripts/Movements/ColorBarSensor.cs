using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField] private Transform ColorBar;
    private TextMeshProUGUI TitleText;
    private TextMeshProUGUI MinText;
    private TextMeshProUGUI MaxText;
    
    private enum SensorType
    {
        DBTorque,
        WeightOnBit
    }

    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
        TitleText = ColorBar.GetChild(0).GetComponent<TextMeshProUGUI>();
        MinText = ColorBar.GetChild(1).GetComponent<TextMeshProUGUI>();
        MaxText = ColorBar.GetChild(2).GetComponent<TextMeshProUGUI>();
        
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
        
        ChangeColorBarText();
    }

    
    
    

    private void ChangeColorBarText()
    {
        TitleText.text = sensorType == SensorType.DBTorque ? "Drill Bit Torque (N.m)" : "Weight On Bit (tons)";
        MinText.text = minValue.ToString("F2");
        MaxText.text = maxValue.ToString("F2");
    }
    
    
    
    
    
    private void SetDBColor()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        float t = Mathf.InverseLerp(minValue, maxValue, sensorType == SensorType.DBTorque ? curDrillingData.DB_Torque : curDrillingData.WeightOnBit);
        DrillBitMesh.material.color = colorGradient.Evaluate(t);
    }
}