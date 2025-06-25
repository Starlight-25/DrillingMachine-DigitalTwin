using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class GraphHandler : MonoBehaviour
{
    private MainUIHandler MainUIHandler;
    [SerializeField] private DrillingMachineMovements DrillingMachineMovements;
    [SerializeField] private WeightManagement WeightManagement;
    [SerializeField] private Parameters Parameters;
    
    [SerializeField] private LineChart lineChart;
    private Serie serie;
    private const int MaxVisiblePoints = 60;
    private Dictionary<string, int> SensorIndexMap;
    private List<List<SerieData>> ListPoints = new List<List<SerieData>>();
    
    private float prevTime;
    private const float timeInterval = 1f;

    private int curSensor = -1;
    
    
    
    
    
    private void Start()
    {
        MainUIHandler = GetComponent<MainUIHandler>();
        serie = lineChart.GetSerie(0);
        serie.ClearData();

        prevTime = Time.time;

        SensorIndexMap = new Dictionary<string, int>()
        {
            { "Socket Depth", 0 },
            { "Drill Bit Position", 1 },
            { "Rotary Table Temperature", 2 },
            { "Slip Table Temperature", 3 },
            { "Rotary Table Position", 4 },
            { "Slip Table Position", 5 }
        };
        
        for (int _ = 0; _ < SensorIndexMap.Count; _++)
            ListPoints.Add(new List<SerieData>());
    }

    
    
    
    
    private void Update()
    {
        float curTime = Time.time;
        if (curTime - prevTime >= timeInterval)
        {
            prevTime = curTime;
            AddPoint(MainUIHandler.GetCurTime() - 3600);
        }
    }


    
    

    private void AddPoint(float x)
    {
        List<SerieData> newData = CreatePoints(x);
        AddPointToList(newData);
        if (curSensor != -1) ShowPoint(newData[curSensor]);
    }

    private List<SerieData> CreatePoints(float x)
    {
        List<SerieData> newDatas = new List<SerieData>();
        for (int _ = 0; _ < ListPoints.Count; _++)
        {
            SerieData newData = new SerieData();
            newData.data = new List<double>() { x, 0 }; 
            newDatas.Add(newData);
        }
        
        newDatas[0].data[1] = DrillingMachineMovements.GetDepth(); // depth
        
        newDatas[1].data[1] = DrillingMachineMovements.GetDrillBitHeight(); // DB height

        newDatas[2].data[1] = Parameters.WaterTemperature +
                              Parameters.RotationVelocity * WeightManagement.GetWeightNeeded() / 20; // RT Temp

        newDatas[3].data[1] = Parameters.WaterTemperature + Parameters.DrillingVelocity * 60 *
            WeightManagement.GetWeightNeeded() * 3 / (DrillingMachineMovements.GetIsDrilling() ? 1 : 2); // ST Temp

        newDatas[4].data[1] = DrillingMachineMovements.GetRTHeight();

        newDatas[5].data[1] = DrillingMachineMovements.GetSTHeight();
        
        return newDatas;
    }
    
    private void AddPointToList(List<SerieData> newData)
    {
        for (int i = 0; i < newData.Count; i++)
            ListPoints[i].Add(newData[i]);
        
        if (ListPoints[0].Count > MaxVisiblePoints)
            foreach (List<SerieData> listPoint in ListPoints)
                listPoint.RemoveAt(0);
    }
    
    private void ShowPoint(SerieData newPoint)
    {
        serie.data.Add(newPoint);
        if (serie.data.Count > MaxVisiblePoints) serie.data.RemoveAt(0);
    }


    
    

    private void ChangeSensor()
    {
        serie.data.Clear();
        serie.data.AddRange(ListPoints[curSensor]);
    }

    
    
    

    public void HandleSensorsInteraction(string sensor)
    {
        int selectedSensor = SensorIndexMap[sensor];
        if (selectedSensor == curSensor)
        {
            curSensor = -1;
            lineChart.gameObject.SetActive(false);
        }
        else
        {
            curSensor = selectedSensor;
            Title title = lineChart.EnsureChartComponent<Title>();
            title.text = sensor;
            lineChart.EnsureChartComponent<YAxis>().axisName.name = selectedSensor < 2 ? "Depth (m)" :
                selectedSensor < 4 ? "Temperature (°C)" : "Height (m)"; 
            lineChart.gameObject.SetActive(true);
            ChangeSensor();
        }
    }
}