using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    public float GetDepth() => DrillingDataManager.Index != 0
        ? DrillingData.GetRange(0, DrillingDataManager.Index).Min(data => data.DrillBit_Height) / 1000
        : 0;
    public float GetDepth(int index) => index != 0
        ? DrillingData.GetRange(0, index).Min(data => data.DrillBit_Height) / 1000
        : 0;
    
    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
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





    public bool IsPaused() => paused;
    public void PauseButtonClicked() => paused = !paused;


    
    
    
    private void Move()
    {
        if (paused || DrillingDataManager.Index >= DrillingData.Count - 1) return;
        t += Time.deltaTime * Parameters.TimeAcceleration / timeInterval;

        (DrillingDataCSV curData, DrillingDataCSV nextData) = (DrillingData[DrillingDataManager.Index],
            DrillingData[DrillingDataManager.Index + 1]);
        MoveDrillingMachine(curData, nextData);
        MoveST(curData, nextData);
        MoveRT(curData, nextData);
        RotateDrillingMachine(curData, nextData);
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
        DateTime curTime =
            DateTime.ParseExact(DrillingData[DrillingDataManager.Index].Date, dateFormat, CultureInfo.InvariantCulture);
        DateTime nextTime =
            DateTime.ParseExact(DrillingData[DrillingDataManager.Index + 1].Date, dateFormat, CultureInfo.InvariantCulture);
        timeInterval = (float)(nextTime - curTime).TotalSeconds;
    }

    
    
    
    
    private void MoveDrillingMachine(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curData.DrillBit_Height / 1000),
            Vector3.up * (nextData.DrillBit_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        (DrillBit.position, Kelly.position) = (pos, pos);
    }

    private void MoveST(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curData.ST_Height / 1000),
            Vector3.up * (nextData.ST_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        SlipTable.position = pos;
    }

    private void MoveRT(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        (Vector3 startPos, Vector3 nextPos) = (Vector3.up * (curData.RT_Height / 1000),
            Vector3.up * (nextData.RT_Height / 1000));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        RotaryTable.position = pos;
    }

    private void RotateDrillingMachine(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        (float starRot, float nextRot) = (curData.DrillBit_Rotation, nextData.DrillBit_Rotation);
        float rotation = Mathf.Lerp(starRot, nextRot, t) * 360 / 60 * Time.deltaTime * Parameters.TimeAcceleration;
        DrillBit.Rotate(0,0, rotation);
        Kelly.Rotate(0,0, rotation);
    }
}