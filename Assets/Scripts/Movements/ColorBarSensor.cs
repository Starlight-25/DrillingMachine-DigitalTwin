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
    private TextMeshProUGUI[] IndicatorsText;
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
        IndicatorsText = new[]
        {
            ColorBar.GetChild(3).GetComponent<TextMeshProUGUI>(), ColorBar.GetChild(4).GetComponent<TextMeshProUGUI>(),
            ColorBar.GetChild(5).GetComponent<TextMeshProUGUI>(), ColorBar.GetChild(6).GetComponent<TextMeshProUGUI>()
        };
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
            minValue = DrillingData.Min(data => data.DM_Torque);
            maxValue = DrillingData.Max(data => data.DM_Torque);
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
        MinText.text = minValue.ToString("F0");
        MaxText.text = maxValue.ToString("F0");

        float interval = (maxValue - minValue) / 5f;
        for (int i = 0; i < IndicatorsText.Length; i++)
            IndicatorsText[i].text = (minValue + (i + 1) * interval).ToString("F0");
    }
    
    
    
    
    
    private void SetDBColor()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        float t = Mathf.InverseLerp(minValue, maxValue, sensorType == SensorType.DBTorque ? curDrillingData.DM_Torque : curDrillingData.WeightOnBit);
        DrillBitMesh.material.color = colorGradient.Evaluate(t);
    }
}