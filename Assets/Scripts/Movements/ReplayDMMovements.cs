using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ReplayDMMovements : MonoBehaviour
{
    [SerializeField] private Transform DLT_B;
    [SerializeField] private Transform DLT_C;
    [SerializeField] private Transform Kelly;
    [SerializeField] private Transform DrillBit;
    [SerializeField] private Transform SlipTable;
    [SerializeField] private Transform RotaryTable;
    
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    private float t = 0f;
    private float timeInterval;
    [SerializeField] private Parameters Parameters;
    private bool paused = true;
    
    private const float EquipmentStarPos = 40f;
    private const string dateFormat = "dd/MM/yyyy HH:mm";


    
    
    

    public void SetCurrentIndex(int index)
    {
        DrillingDataManager.Index = index;
        SetTimeFields();
    }
    
    
    
    
    
    private void Start()
    {
        //DrillingDataManager.Load(@"C:\\Users\\linje\\EPITA\\Res\\DrillDigitalTwin\\Data\\DrillingReplayExample.csv");
        DrillingData = DrillingDataManager.DrillingData;
        // DrillingData = new List<DrillingDataCSV>();
        // DrillingData.Add(new DrillingDataCSV()
        // {
        //     Date = "01/01/2020 00:00", DLT_B = 0, DLT_C = 0, DM = 0, DrillBit_Height = 0, DrillingVelocity = 0, RT_Height = 0, RT_Load = 0, RT_Temp = 0, ST_Height = 0, ST_Load = 0, ST_Temp = 0, WeightOnBit = 0
        // });
        // DrillingData.Add(new DrillingDataCSV()
        // {
        //     Date = "01/01/2020 00:01", DLT_B = 0, DLT_C = 0, DM = 0, DrillBit_Height = 10, DrillingVelocity = 0, RT_Height = 0, RT_Load = 0, RT_Temp = 0, ST_Height = 0, ST_Load = 0, ST_Temp = 0, WeightOnBit = 0
        // });
        SetTimeFields();
        //InitEquipmentPostition();
    }

    
    
    
    
    private void InitEquipmentPostition()
    {
        DLT_B.position = Vector3.up * EquipmentStarPos;
        DLT_C.position = Vector3.up * EquipmentStarPos;
        Kelly.position = Vector3.up * EquipmentStarPos;
        DrillBit.position = Vector3.up * EquipmentStarPos;
        SlipTable.position = Vector3.up * EquipmentStarPos;
        RotaryTable.position = Vector3.up * EquipmentStarPos;
    }

    
    
    

    private void Update()
    {
        Move();
    }


    
    
    
    public void PauseButtonClicked() => paused = !paused;


    
    
    
    private void Move()
    {
        if (paused || DrillingDataManager.Index >= DrillingData.Count - 1) return;
        t += Time.deltaTime * Parameters.TimeAcceleration / timeInterval;
        MoveDrillingMachine();
        MoveST();
        MoveRT();
        if (t >= 1f)
        {
            t = 0f;
            if (DrillingDataManager.Index >= DrillingData.Count - 1)
            {
                paused = true;
                return;
            }
            DrillingDataManager.Index++;
            SetTimeFields();
        }
    }

    
    
    
    
    private void SetTimeFields()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        DateTime curTime =
            DateTime.ParseExact(curDrillingData.Date, dateFormat, CultureInfo.InvariantCulture);
        DateTime nextTime =
            DateTime.ParseExact(DrillingData[DrillingDataManager.Index + 1].Date, dateFormat, CultureInfo.InvariantCulture);
        timeInterval = (float)(nextTime - curTime).TotalSeconds;
    }

    
    
    
    
    private void MoveDrillingMachine()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curDrillingData.DrillBit_Height / 1000),
            Vector3.up * (DrillingData[DrillingDataManager.Index + 1].DrillBit_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        (DrillBit.position, Kelly.position) = (pos, pos);
    }

    private void MoveST()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curDrillingData.ST_Height / 1000),
            Vector3.up * (DrillingData[DrillingDataManager.Index + 1].ST_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        SlipTable.position = pos;
    }

    private void MoveRT()
    {
        DrillingDataCSV curDrillingData = DrillingData[DrillingDataManager.Index];
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curDrillingData.RT_Height / 1000),
            Vector3.up * (DrillingData[DrillingDataManager.Index + 1].RT_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        RotaryTable.position = pos;
    }
}