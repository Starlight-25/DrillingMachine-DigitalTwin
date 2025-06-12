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
        if (!CanMove(HeightMovVal > 0f)) return;
        Vector3 movement = Vector3.up * (HeightMovVal * Time.deltaTime * HeightNavigationSensitivity);
        Selected.position += movement;
        if ((Selected.name == "SlipTable" && STlocked) ^ (Selected.name == "RotaryTable" && RTlocked)) MoveDM(movement);
    }

    private bool CanMove(bool up)
    {
        Vector3 STpos = SlipTable.position;
        Vector3 RTpos = RotaryTable.position;
        bool isSTSelected = Selected.name == "SlipTable";
        if (up && !CanMoveUp(STpos, RTpos, isSTSelected)) return false;
        if (!up && !CanMoveDown(STpos, RTpos, isSTSelected)) return false;
        return true;
    }

    private bool CanMoveUp(Vector3 STpos, Vector3 RTpos, bool isSTSelected)
    {
        if (
            isSTSelected && (
                STpos.y > 23f ||
                !RTlocked && STlocked && Vector3.Distance(RTpos, DrillBit.position) <= 15.3 ||
                !STlocked && Vector3.Distance(STpos, Kelly.position) >= 49f
                ) ||
            !isSTSelected && Vector3.Distance(RTpos, STpos) <= 3f
            )
            return false;
        // if (isSTSelected && STpos.y > 23f) return false;
        // if (!isSTSelected && Vector3.Distance(RTpos, STpos) <= 3f) return false;
        // if (isSTSelected && !RTlocked && STlocked && Vector3.Distance(RTpos, DrillBit.position) <= 15.3) return false;
        // if (isSTSelected && !STlocked && Vector3.Distance(STpos, Kelly.position) >= 49f) return false;
        return true;
    }

    private bool CanMoveDown(Vector3 STpos, Vector3 RTpos, bool isSTSelected)
    {
        if (
            isSTSelected && Vector3.Distance(RTpos, STpos) <= 3f ||
            !isSTSelected && (
                RTpos.y <= 1f ||
                !RTlocked && Vector3.Distance(RTpos, DrillBit.position) <= 15.3 ||
                RTlocked && Vector3.Distance(STpos, Kelly.position) >= 49f
                )
            )
            return false;
        // if (!isSTSelected && RTpos.y <= 1f) return false;
        // if (isSTSelected && Vector3.Distance(RTpos, STpos) <= 3f) return false;
        // if (!isSTSelected && !RTlocked && Vector3.Distance(RTpos, DrillBit.position) <= 15.3) return false;
        // if (!isSTSelected && RTlocked && Vector3.Distance(STpos, Kelly.position) >= 49f) return false;
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