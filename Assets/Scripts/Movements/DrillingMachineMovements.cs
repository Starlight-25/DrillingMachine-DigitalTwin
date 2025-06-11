using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillingMachineMovements : MonoBehaviour, ISettingsUpdater
{
    [SerializeField] private Transform Kelly;
    [SerializeField] private Transform SlipTable;
    [SerializeField] private Transform RotaryTable;
    [SerializeField] private Transform DrillBit;

    private Transform Selected;
    private bool STlocked = true;
    private bool RTlocked = false;

    [SerializeField] private Material BasicMaterial;
    [SerializeField] private Material HiglightMaterial;
    [SerializeField] private Material LockedMaterial;
    [SerializeField] private Material TransparentMaterial;

    [SerializeField] private PlayerInput PlayerInput;
    private InputAction SelectNoneInputAction;
    private InputAction SelectSTInputAction;
    private InputAction SelectRTInputAction;
    private InputAction HeightMovementsInputAction;
    private InputAction DLTDetailsVisibleInputAction;
    private InputAction LockEquipmentInputAction;

    [SerializeField] private SettingsHandler SettingsHandler;
    private int HeightNavigationSensitivity;

    [SerializeField] private MeshRenderer DLTDetailMeshRenderer;
    private bool isDLTDetailsTransparent = false;





    private void Start()
    {
        SettingsHandler.Add(this);
        UpdateFromSettings();

        SelectNoneInputAction = PlayerInput.actions["SelectNone"];
        SelectSTInputAction = PlayerInput.actions["SelectST"];
        SelectRTInputAction = PlayerInput.actions["SelectRT"];
        HeightMovementsInputAction = PlayerInput.actions["HeightMovement"];
        DLTDetailsVisibleInputAction = PlayerInput.actions["DLTDetailsVisible"];
        LockEquipmentInputAction = PlayerInput.actions["LockEquipment"];
    }





    private void Update()
    {
        if (SelectNoneInputAction.triggered) SelectEquipment(null);
        if (SelectSTInputAction.triggered) SelectEquipment(SlipTable);
        if (SelectRTInputAction.triggered) SelectEquipment(RotaryTable);
        MoveSelectedEquipment();
        
        if (DLTDetailsVisibleInputAction.triggered) SwitchDLTDetailsVisibility();
        if (LockEquipmentInputAction.triggered && Selected is not null) LockTable();
    }





    public void UpdateFromSettings()
    {
        HeightNavigationSensitivity = SettingsHandler.Settings.Sensibility.HeightNavigation;
    }





    private void SelectEquipment(Transform equipment)
    {
        MeshRenderer meshRenderer;
        Material[] materials;
        if (Selected is not null)
        {
            meshRenderer = Selected.GetComponent<MeshRenderer>();
            SwitchSecondMaterial(meshRenderer, meshRenderer.material);
        }

        Selected = equipment;
        if (Selected is null) return;
        meshRenderer = Selected.GetComponent<MeshRenderer>();
        if (Selected.name == "SlipTable" && STlocked || Selected.name == "RotaryTable" && RTlocked)
            SwitchSecondMaterial(meshRenderer, LockedMaterial);
        else SwitchSecondMaterial(meshRenderer, HiglightMaterial);
    }

    private void SwitchSecondMaterial(MeshRenderer meshRenderer, Material material)
    {
        Material[] materials = meshRenderer.materials;
        materials[1] = material;
        meshRenderer.materials = materials;
    }




    private void MoveSelectedEquipment()
    {
        if (Selected is null || STlocked == RTlocked && STlocked) return;
        float HeightMovVal = HeightMovementsInputAction.ReadValue<float>();
        if (HeightMovVal < 0f && !CanMoveDown() || HeightMovVal > 0f && !CanMoveUp()) return;
        Vector3 movement = Vector3.up * (HeightMovVal * Time.deltaTime * HeightNavigationSensitivity);
        Selected.position += movement;
        if (STlocked ^ RTlocked) MoveDM(movement);
    }

    private bool CanMoveDown()
    {
        Vector3 pos = Selected.position;
        if (pos.y <= 1f) return false;
        if (Vector3.Distance(pos, DrillBit.position) <= 15.3) return false;
        float distToRT = Vector3.Distance(pos, RotaryTable.position);
        if (distToRT != 0f && distToRT <= 3f) return false;
        return true;
    }

    private bool CanMoveUp()
    {
        Vector3 pos = Selected.position;
        if (Vector3.Distance(pos, transform.position) >= 23) return false;
        if (Vector3.Distance(pos, Kelly.position) >= 50f) return false;
        float distToST = Vector3.Distance(pos, SlipTable.position);
        if (distToST != 0f && distToST <= 3f) return false;
        return true;
    }





    private void MoveDM(Vector3 movement)
    {
        Kelly.position += movement;
        DrillBit.position += movement;
    }





    private void LockTable()
    {
        if (Selected.name == "SlipTable")
        {
            STlocked = !STlocked;
            SwitchSecondMaterial(Selected.GetComponent<MeshRenderer>(), STlocked ? LockedMaterial : HiglightMaterial);
        }
        else if (Selected.name == "RotaryTable")
        {
            RTlocked = !RTlocked;
            SwitchSecondMaterial(Selected.GetComponent<MeshRenderer>(), RTlocked ? LockedMaterial : HiglightMaterial);
        }
    }





    private void SwitchDLTDetailsVisibility()
    {
        DLTDetailMeshRenderer.material = isDLTDetailsTransparent ? BasicMaterial : TransparentMaterial;
        isDLTDetailsTransparent = !isDLTDetailsTransparent;
    }
}