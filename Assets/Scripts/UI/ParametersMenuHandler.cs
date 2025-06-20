using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ParametersMenuHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction ReturnInputAction;
    [SerializeField] private GameObject MainUI;
    
    [SerializeField] private Parameters Parameters;
    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600, 7200, 18000, 43200, 86400 };
    [SerializeField] private TextMeshProUGUI DrillingVelocityValueText;
    [SerializeField] private TextMeshProUGUI RotationVelocityValueText;

    [SerializeField] private WeightManagement WeightManagement;
    [SerializeField] private Slider LLDepthSlider;
    [SerializeField] private Slider BELDepthSlider;
    [SerializeField] private Slider ClayDepthSlider;
    [SerializeField] private Slider RLDepthSlider;
    private TextMeshProUGUI LLDepthValueText;
    private TextMeshProUGUI BELDepthValueText;
    private TextMeshProUGUI ClayDepthValueText;
    private TextMeshProUGUI RLDepthValueText;


    
    
    
    private void Start()
    {
        ReturnInputAction = PlayerInput.actions["Return"];
        
        LLDepthValueText = LLDepthSlider.transform.Find("Value Text (TMP)").GetComponent<TextMeshProUGUI>();
        BELDepthValueText = BELDepthSlider.transform.Find("Value Text (TMP)").GetComponent<TextMeshProUGUI>(); 
        ClayDepthValueText = ClayDepthSlider.transform.Find("Value Text (TMP)").GetComponent<TextMeshProUGUI>(); 
        RLDepthValueText = RLDepthSlider.transform.Find("Value Text (TMP)").GetComponent<TextMeshProUGUI>(); 
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }

    
    
    
    
    public void ReturnButtonClicked()
    {
        MainUI.SetActive(true);
        gameObject.SetActive(false);
    }


    
    
    
    public void UpdateTimeSpeedDropDown(int index)
    {
        Parameters.TimeAcceleration = timeAccelerationMap[index];
    }

    
    
    

    public void UpdateDrillingVelocitySlider(float value)
    {
        float velocity = value / 60;
        Parameters.DrillingVelocity = velocity;
        Parameters.HeightNavVelocity = 2 * velocity;
        DrillingVelocityValueText.text = $"{(int)value}cm/min";
    }


    
    
    
    public void UpdateRotationVelocitySlider(float value)
    {
        Parameters.RotationVelocity = value;
        RotationVelocityValueText.text = $"{(int)value}RPM";
    }

    
    
    

    public void UpdateLLDepthSlider(float value)
    {
        Parameters.LLDepth = value;
        LLDepthValueText.text = $"{(int)value}m";
        if (BELDepthSlider.value < value) UpdateBELDepthSlider(value);
        if (ClayDepthSlider.value < value) UpdateClayDepthSlider(value);
        if (RLDepthSlider.value < value) UpdateRLDepthSlider(value);
        HandleValueDepthSlider();
    }

    public void UpdateBELDepthSlider(float value)
    {
        Parameters.BELDepth = value;
        BELDepthValueText.text = $"{(int)value}m";
        if (LLDepthSlider.value > value) UpdateLLDepthSlider(value);
        if (ClayDepthSlider.value < value) UpdateClayDepthSlider(value);
        if (RLDepthSlider.value < value) UpdateRLDepthSlider(value);
        HandleValueDepthSlider();
    }

    public void UpdateClayDepthSlider(float value)
    {
        Parameters.ClayDepth = value;
        ClayDepthValueText.text = $"{(int)value}m";
        if (LLDepthSlider.value > value) UpdateLLDepthSlider(value);
        if (BELDepthSlider.value > value) UpdateBELDepthSlider(value);
        if (RLDepthSlider.value < value) UpdateRLDepthSlider(value);
        HandleValueDepthSlider();
    }

    public void UpdateRLDepthSlider(float value)
    {
        Parameters.RLDepth = value;
        RLDepthValueText.text = $"{(int)value}m";
        if (LLDepthSlider.value > value) UpdateLLDepthSlider(value);
        if (BELDepthSlider.value > value) UpdateBELDepthSlider(value);
        if (ClayDepthSlider.value > value) UpdateClayDepthSlider(value);
        HandleValueDepthSlider();
    }

    private void HandleValueDepthSlider()
    {
        LLDepthSlider.value = Parameters.LLDepth;
        BELDepthSlider.value = Parameters.BELDepth;
        ClayDepthSlider.value = Parameters.ClayDepth;
        RLDepthSlider.value = Parameters.RLDepth;
        TerrainLayer[] terrainLayers = Parameters.TerrainLayers;
        terrainLayers[1].Depth = LLDepthSlider.value;
        terrainLayers[2].Depth = BELDepthSlider.value;
        terrainLayers[3].Depth = ClayDepthSlider.value;
        terrainLayers[4].Depth = RLDepthSlider.value;
        WeightManagement.UpdateTerrainLayer();
    }
}