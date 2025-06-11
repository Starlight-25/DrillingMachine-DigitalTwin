using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillingMachineMovements : MonoBehaviour, ISettingsUpdater
{
    [SerializeField] private Transform DrillBit;
    [SerializeField] private Transform Kelly;
    [SerializeField] private Transform RotaryTable;
    [SerializeField] private Transform SlipTable;

    private Transform Selected;

    [SerializeField] private Material HiglightMaterial;

    [SerializeField] private PlayerInput PlayerInput;
    private InputAction SelectNoneInputAction;
    private InputAction SelectSTInputAction;
    private InputAction SelectRTInputAction;
    private InputAction HeightMovementsInputAction;

    [SerializeField] private SettingsHandler SettingsHandler;
    private int HeightNavigationSensitivity;





    private void Start()
    {
        SettingsHandler.Add(this);
        UpdateFromSettings();

        SelectNoneInputAction = PlayerInput.actions["SelectNone"];
        SelectSTInputAction = PlayerInput.actions["SelectST"];
        SelectRTInputAction = PlayerInput.actions["SelectRT"];
        HeightMovementsInputAction = PlayerInput.actions["HeightMovement"];
    }





    private void Update()
    {
        if (SelectNoneInputAction.triggered) SelectEquipment(null);
        if (SelectSTInputAction.triggered) SelectEquipment(SlipTable);
        if (SelectRTInputAction.triggered) SelectEquipment(RotaryTable);
        MoveSelectedEquipment();
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
            materials = meshRenderer.materials;
            materials[1] = meshRenderer.material;
            meshRenderer.materials = materials;
        }

        Selected = equipment;
        if (Selected is null) return;
        meshRenderer = Selected.GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
        materials[1] = HiglightMaterial;
        meshRenderer.materials = materials;
    }




    private void MoveSelectedEquipment()
    {
        if (Selected is null) return;
        float HeightMovVal = HeightMovementsInputAction.ReadValue<float>();
        if (HeightMovVal < 0f && !CanMoveDown() || HeightMovVal > 0f && !CanMoveUp()) return;
        Selected.position += Vector3.up * (HeightMovVal * Time.deltaTime * HeightNavigationSensitivity);
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
}