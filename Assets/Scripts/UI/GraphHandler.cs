using System;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class GraphHandler : MonoBehaviour
{
    private MainUIHandler MainUIHandler;
    [SerializeField] private DrillingMachineMovements DrillingMachineMovements;
    [SerializeField] private Parameters Parameters;
    
    [SerializeField] private LineChart lineChart;
    private Serie serie;
    private const int MaxVisiblePoints = 60;
    private List<SerieData> depthPoints = new List<SerieData>();
    private List<SerieData> drillBitPosPoints = new List<SerieData>();
    
    private float prevTime;
    private const float timeInterval = 1f;

    private string curSensor = "";
    
    
    
    
    
    private void Start()
    {
        MainUIHandler = GetComponent<MainUIHandler>();
        serie = lineChart.GetSerie(0);
        serie.ClearData();

        prevTime = Time.time;
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
        SerieData newDepthData = new SerieData();
        newDepthData.data = new List<double>(){ x, DrillingMachineMovements.GetDepth() };

        SerieData newDrillBitPosPoint = new SerieData();
        newDrillBitPosPoint.data = new List<double>() { x, DrillingMachineMovements.GetDrillBitHeight() };

        AddPointToList(newDepthData, newDrillBitPosPoint);

        switch (curSensor)
        {
            case "DrillBitPosition":
                ShowPoint(newDrillBitPosPoint);
                break;
            case "Depth":
                ShowPoint(newDepthData);
                break;
        }
        
    }

    
    
    
    
    private void AddPointToList(SerieData newDepthPoint, SerieData newDrillBitPosPoint)
    {
        depthPoints.Add(newDepthPoint);
        drillBitPosPoints.Add(newDrillBitPosPoint);
        if (depthPoints.Count > MaxVisiblePoints)
        {
            depthPoints.RemoveAt(0);
            drillBitPosPoints.RemoveAt(0);
        }
    }

    
    
    

    private void ShowPoint(SerieData newPoint)
    {
        serie.data.Add(newPoint);
        if (serie.data.Count > MaxVisiblePoints) serie.data.RemoveAt(0);
    }


    
    

    private void ChangeSensor()
    {
        serie.ClearData();
        switch (curSensor)
        {
            case "DrillBitPosition":
                serie.data.AddRange(drillBitPosPoints);
                break;
            case "Depth":
                serie.data.AddRange(depthPoints);
                break;
        }
    }

    
    
    

    public void HandleSensorsInteraction(string sensor)
    {
        Debug.Log(sensor);
        if (sensor == curSensor)
        {
            curSensor = "";
            lineChart.gameObject.SetActive(false);
        }
        else
        {
            curSensor = sensor;
            lineChart.gameObject.SetActive(true);
            ChangeSensor();
        }
    }
}