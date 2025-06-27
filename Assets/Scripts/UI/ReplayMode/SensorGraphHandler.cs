using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using XCharts.Runtime;

public class SensorGraphHandler : MonoBehaviour
{
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    [SerializeField] private ReplayDMMovements ReplayDMMovements;
    [SerializeField] private LineChart lineChart;
    private Serie serie;
    private const int MaxVisiblePoints = 60;
    private Dictionary<string, int> SensorIndexMap;
    private List<List<SerieData>> ListPoints = new List<List<SerieData>>();
    
    private DateTime startTime;
    private const string dateFormat = "dd/MM/yyyy HH:mm";
    private int prevIndex = -1;
    private int curSensor = -1;
    
    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
        startTime = DateTime.ParseExact(DrillingData[0].Date, dateFormat, CultureInfo.InvariantCulture);
        
        serie = lineChart.GetSerie(0);
        serie.ClearData();

        SensorIndexMap = new Dictionary<string, int>()
        {
            { "Weight On Bit", 0 },
            { "Drill Bit Velocity", 1 },
            { "Rotary Table Temperature", 2 },
            { "Rotary Table Load", 3 },
            { "Slip Table Temperature", 4 },
            { "Slip Table Load", 5 },
        };
        
        for (int _ = 0; _ < SensorIndexMap.Count; _++)
            ListPoints.Add(new List<SerieData>());
    }

    
    
    
    
    private void Update()
    {
        int curIndex = DrillingDataManager.Index;
        if (curIndex > prevIndex)
        {
            prevIndex = DrillingDataManager.Index;
            DateTime curTime = DateTime.ParseExact(DrillingData[DrillingDataManager.Index].Date, dateFormat,
                CultureInfo.InvariantCulture);
            int sec = (int)(curTime - startTime).TotalSeconds;
            AddPoint( sec - 3600);
        }
        else if (curIndex < prevIndex)
        {
            
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

        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        
        newDatas[0].data[1] = curDrillingData.WeightOnBit;
        
        newDatas[1].data[1] = curDrillingData.DrillingVelocity;
        
        newDatas[2].data[1] = curDrillingData.RT_Temp;
        
        newDatas[3].data[1] = curDrillingData.RT_Load;
        
        newDatas[4].data[1] = curDrillingData.ST_Temp;
        
        newDatas[5].data[1] = curDrillingData.ST_Load;
        
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