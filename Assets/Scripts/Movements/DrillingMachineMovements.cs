using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillingMachineMovements : MonoBehaviour
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


    
    
    
    private void Start()
    {
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
}