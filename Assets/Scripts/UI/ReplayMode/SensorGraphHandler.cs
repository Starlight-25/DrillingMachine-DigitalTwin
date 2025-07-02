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
    private Dictionary<string, int> SensorIndexMap;
    private Func<int, double>[] SensorValueMap;
    private string[] YAxisNameMap;
    private List<List<SerieData>> ListPoints = new List<List<SerieData>>();
    
    private DateTime startTime;
    private const string dateFormat = "dd/MM/yyyy HH:mm";
    private int prevIndex = -1;
    private int curIndex;
    private int curSensor = -1;
    
    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
        startTime = DateTime.ParseExact(DrillingData[0].Date, dateFormat, CultureInfo.InvariantCulture);
        
        serie = lineChart.GetSerie(0);
        serie.ClearData();

        SensorIndexMap = new Dictionary<string, int>()
        {
            { "Drill Bit Velocity", 0 },
            { "Rotary Table Temperature", 1 },
            { "Rotary Table Load", 2 },
            { "Slip Table Temperature", 3 },
            { "Slip Table Load", 4 },
            { "Depth", 5 }
        };
        SensorValueMap = new[]
        {
            (Func<int, double>)((index) => DrillingData[index].DrillingVelocity),
            (Func<int, double>)((index) => DrillingData[index].RT_Temp),
            (Func<int, double>)((index) => DrillingData[index].RT_Load),
            (Func<int, double>)((index) => DrillingData[index].ST_Temp),
            (Func<int, double>)((index) => DrillingData[index].ST_Load),
            (Func<int, double>)((index) => ReplayDMMovements.GetDepth(index))
        };
        YAxisNameMap = new[]
        {
            "Velocity (mm/s)",
            "Temperature (°C)",
            "Load (tons)",
            "Temperature (°C)",
            "Load (tons)",
            "Depth (m)"
        };
        
        for (int _ = 0; _ < SensorIndexMap.Count; _++)
            ListPoints.Add(new List<SerieData>());
    }

    
    
    
    
    private void Update()
    {
        curIndex = DrillingDataManager.Index;
        if (curIndex > prevIndex)
        {
            if (curIndex - prevIndex == 1) AddPoint();
            else AddRangePoints();
        }
        else if (curIndex < prevIndex) DeleteRangePoints();
        prevIndex = curIndex;
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
            lineChart.EnsureChartComponent<YAxis>().axisName.name = YAxisNameMap[curSensor];
            lineChart.gameObject.SetActive(true);
            ChangeSensor();
        }
    }
    
    
    
    
    
    private void ChangeSensor()
    {
        serie.data.Clear();
        if (curSensor != -1) serie.data.AddRange(ListPoints[curSensor]);
    }

    
    
    

    private void AddPoint()
    {
        List<SerieData> newData = CreatePoints();
        AddPointToList(newData);
        if (curSensor != -1) ShowPoint(newData[curSensor]);
    }

    private List<SerieData> CreatePoints()
    {
        DateTime curTime = DateTime.ParseExact(DrillingData[curIndex].Date, dateFormat,
            CultureInfo.InvariantCulture);
        int sec = (int)(curTime - startTime).TotalSeconds - 3600;

        List<SerieData> newDatas = new List<SerieData>();
        for (int i = 0; i < ListPoints.Count; i++)
        {
            SerieData newData = new SerieData();
            newData.data = new List<double>() { sec, SensorValueMap[i](curIndex) }; 
            newDatas.Add(newData);
        }
        
        return newDatas;
    }
    
    private void AddPointToList(List<SerieData> newData)
    {
        for (int i = 0; i < newData.Count; i++)
            ListPoints[i].Add(newData[i]);
    }
    
    private void ShowPoint(SerieData newPoint) => serie.data.Add(newPoint);


    
    

    private void AddRangePoints()
    {
        int startIndex = ListPoints[0].Count;
        for (int i = 0; i < ListPoints.Count; i++)
        {
            List<SerieData> list = ListPoints[i];
            for (int j = startIndex; j <= curIndex; j++)
            {
                DateTime curTime = DateTime.ParseExact(DrillingData[j].Date, dateFormat,
                    CultureInfo.InvariantCulture);
                int sec = (int)(curTime - startTime).TotalSeconds - 3600;
                
                SerieData newData = new SerieData();
                newData.data = new List<double>() { sec, SensorValueMap[i](j) }; 
                list.Add(newData);
            }
        }
        
        ChangeSensor();
    }



    

    private void DeleteRangePoints()
    {
        int startIndex = curIndex + 1;
        int numToRemove = ListPoints[0].Count - startIndex;
        foreach (List<SerieData> list in ListPoints) 
            list.RemoveRange(startIndex, numToRemove);

        ChangeSensor();
    }
}