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
    private Func<double>[] SensorValueMap;
    private string[] YAxisNameMap;
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
            { "Drill Bit Velocity", 0 },
            { "Rotary Table Temperature", 1 },
            { "Rotary Table Load", 2 },
            { "Slip Table Temperature", 3 },
            { "Slip Table Load", 4 },
            { "Depth", 5 }
        };
        SensorValueMap = new[]
        {
            (Func<double>)(() => DrillingData[DrillingDataManager.Index].DrillingVelocity),
            (Func<double>)(() => DrillingData[DrillingDataManager.Index].RT_Temp),
            (Func<double>)(() => DrillingData[DrillingDataManager.Index].RT_Load),
            (Func<double>)(() => DrillingData[DrillingDataManager.Index].ST_Temp),
            (Func<double>)(() => DrillingData[DrillingDataManager.Index].ST_Load),
            (Func<double>)(() => ReplayDMMovements.GetDepth())
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
        int curIndex = DrillingDataManager.Index;
        if (curIndex > prevIndex)
        {
            prevIndex = curIndex;
            DateTime curTime = DateTime.ParseExact(DrillingData[curIndex].Date, dateFormat,
                CultureInfo.InvariantCulture);
            int sec = (int)(curTime - startTime).TotalSeconds;
            AddPoint( sec - 3600);
        }
        else if (curIndex < prevIndex)
        {
            prevIndex = curIndex;
            SetPoints();
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
        for (int i = 0; i < ListPoints.Count; i++)
        {
            SerieData newData = new SerieData();
            newData.data = new List<double>() { x, SensorValueMap[i]() }; 
            newDatas.Add(newData);
        }
        
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
        if (curSensor != -1) serie.data.AddRange(ListPoints[curSensor]);
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





    private void SetPoints()
    {
        CreateRangePoints();
        ChangeSensor();
    }

    private void CreateRangePoints()
    {
        ListPoints = new List<List<SerieData>>();
        for (int _ = 0; _ < SensorIndexMap.Count; _++)
            ListPoints.Add(new List<SerieData>());

        int curIndex = DrillingDataManager.Index;
        int startIndex = curIndex >= MaxVisiblePoints ? 0 : curIndex - MaxVisiblePoints;

        for (int i = 0; i < ListPoints.Count; i++)
        {
            List<SerieData> listPoint = ListPoints[i];
            for (int _ = startIndex; _ < curIndex; _++)
            {
                SerieData newData = new SerieData();
                DateTime curTime = DateTime.ParseExact(DrillingData[DrillingDataManager.Index].Date, dateFormat,
                    CultureInfo.InvariantCulture);
                int x = (int)(curTime - startTime).TotalSeconds;
                newData.data = new List<double>() { x, SensorValueMap[i]() }; 
                listPoint.Add(newData);
            }
        }
    }
}