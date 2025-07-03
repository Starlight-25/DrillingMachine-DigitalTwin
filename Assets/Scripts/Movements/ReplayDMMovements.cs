using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReplayDMMovements : MonoBehaviour
{
    [SerializeField] private Transform DLT_B;
    [SerializeField] private Transform DLT_C;
    [SerializeField] private Transform Kelly;
    [SerializeField] private Transform DrillBit;
    [SerializeField] private Transform SlipTable;
    [SerializeField] private Transform RotaryTable;
    private MeshRenderer[] DLT_BMesh;
    private MeshRenderer DLT_CMesh;
    private MeshRenderer[] DMMesh;
    
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    
    private InputAction DLTDetailsVisibleInputAction;
    private MeshRenderer DLTDetailsMeshRenderer;
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
        
        DLTDetailsVisibleInputAction = GetComponent<PlayerInput>().actions["DLTDetailsVisible"];
        DLTDetailsMeshRenderer = DLT_B.GetChild(0).GetComponent<MeshRenderer>();

        DLT_BMesh = new[] { DLT_B.GetComponent<MeshRenderer>(), DLTDetailsMeshRenderer };
        DLT_CMesh = DLT_C.GetComponent<MeshRenderer>();
        DMMesh = new[]
        {
            Kelly.GetComponent<MeshRenderer>(), DrillBit.GetComponent<MeshRenderer>(),
            RotaryTable.GetComponent<MeshRenderer>(), SlipTable.GetComponent<MeshRenderer>()
        };
        
        InitEquipmentPostition();
    }

    
    
    
    
    private void InitEquipmentPostition()
    {
        foreach (var mesh in DLT_BMesh)
            mesh.enabled = false;
        foreach (var mesh in DMMesh)
            mesh.enabled = false;
        DLT_CMesh.enabled = false;
        
        DLT_B.position = Vector3.up * EquipmentStarPos;
        DLT_C.position = Vector3.up * EquipmentStarPos;
        Kelly.position = Vector3.up * EquipmentStarPos;
        DrillBit.position = Vector3.up * EquipmentStarPos;
        SlipTable.position = Vector3.up * EquipmentStarPos;
        RotaryTable.position = Vector3.up * EquipmentStarPos;
    }

    
    
    

    private void Update()
    {
        if (DLTDetailsVisibleInputAction.triggered) SwitchDLTDetailsVisibility();
        Move();
    }





    public bool IsPaused() => paused;
    public void PauseButtonClicked() => paused = !paused;



    

    private void SwitchDLTDetailsVisibility() => DLTDetailsMeshRenderer.enabled = !DLTDetailsMeshRenderer.enabled;
    
    
    
    
    
    private void Move()
    {
        if (paused || DrillingDataManager.Index >= DrillingData.Count - 1) return;
        t += Time.deltaTime * Parameters.TimeAcceleration / timeInterval;

        (DrillingDataCSV curData, DrillingDataCSV nextData) = (DrillingData[DrillingDataManager.Index],
            DrillingData[DrillingDataManager.Index + 1]);
        
        if (!IsInstalled(curData)) MoveInstallation(curData, nextData);
        else MoveDrill(curData, nextData);
        
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


    
    
    
    private bool IsInstalled(DrillingDataCSV data) => data.DLT_B == 1f && data.DLT_C == 1f && data.DM == 1f;


    
    
    
    private void MoveInstallation(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        MoveDLTB(curData, nextData);
        MoveDLTC(curData, nextData);
        MoveDM(curData, nextData);
    }
    
    private Vector3 GetPosInstallation(float x) => Vector3.up * (EquipmentStarPos * (1 - x));
    
    private void MoveDLTB(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        bool visible = curData.DLT_B != 0f;
            foreach (MeshRenderer mesh in DLT_BMesh)
                mesh.enabled = visible;

        (Vector3 startPos, Vector3 nextPos) = (GetPosInstallation(curData.DLT_B), GetPosInstallation(nextData.DLT_B));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        DLT_B.position = pos;
    }

    private void MoveDLTC(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        DLT_CMesh.enabled = curData.DLT_C != 0f;
        
        (Vector3 startPos, Vector3 nextPos) = (GetPosInstallation(curData.DLT_C), GetPosInstallation(nextData.DLT_C));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        DLT_C.position = pos;
    }

    private void MoveDM(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        bool visible = curData.DM != 0f;
        foreach (MeshRenderer mesh in DMMesh)
            mesh.enabled = visible;

        (Vector3 startPos, Vector3 nextPos) = (GetPosInstallation(curData.DM), GetPosInstallation(nextData.DM));
        Vector3 pos = Vector3.Lerp(startPos, nextPos, t);
        (DrillBit.position, Kelly.position) = (pos, pos);
        (RotaryTable.position, SlipTable.position) = (pos + Vector3.up * 16, pos + Vector3.up * 22);
    }

    
    
    
    
    private void MoveDrill(DrillingDataCSV curData, DrillingDataCSV nextData)
    {
        MoveDrillingMachine(curData, nextData);
        MoveST(curData, nextData);
        MoveRT(curData, nextData);
        RotateDrillingMachine(curData, nextData);
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