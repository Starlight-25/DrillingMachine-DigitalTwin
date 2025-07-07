using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplayDayNightCycle : MonoBehaviour
{
    private Transform Light;
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    
    private const string dateFormat = "dd/MM/yyyy HH:mm";
    private DateTime startTime;
    
    
    
    

    private void Start()
    {
        Light = transform;
        DrillingData = DrillingDataManager.DrillingData;

        startTime = DateTime.ParseExact(DrillingData[0].Date, dateFormat, null);
    }

    
    
    
    
    private void Update()
    {
        float timeInHour = GetTimeInHour();
        LightRotation(timeInHour);
    }

    
    
    
    
    private float GetTimeInHour()
    {
        DateTime curTime = DateTime.ParseExact(DrillingData[DrillingDataManager.Index].Date, dateFormat, null);
        return (float)(curTime - startTime).TotalHours % 24f;
    }
    
    
    
    
    
    private void LightRotation(float timeInHour)
    {
        float sunAngle = timeInHour / 24f * 360f - 90f;
        Light.rotation = Quaternion.Euler(sunAngle, 0, 0);
    }
}