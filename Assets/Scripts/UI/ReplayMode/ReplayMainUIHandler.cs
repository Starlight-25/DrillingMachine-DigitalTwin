using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReplayMainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject SetitngsMenu;
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction ReturnInputAction;
    private InputAction PauseInputAction;

    [SerializeField] private Parameters Parameters;
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    [SerializeField] private ReplayDMMovements ReplayDMMovements;
    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600, 7200, 18000, 43200, 86400 };
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private TextMeshProUGUI CurrentTimeText;
    [SerializeField] private TextMeshProUGUI SocketDepthText;
    
    
    
    
    private void Start()
    {
        DrillingData = DrillingDataManager.DrillingData;
        TimeSlider.maxValue = DrillingData.Count - 2;
        
        ReturnInputAction = PlayerInput.actions["Return"];
        PauseInputAction = PlayerInput.actions["Pause"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
        if (PauseInputAction.triggered) PauseButtonClicked();
        UpdateTimeSliderAndText();
        UpdateSocketDepthText(ReplayDMMovements.GetDepth(DrillingDataManager.Index));
    }

    
    
    
    
    public void SettingsButtonClicked()
    {
        Time.timeScale = 0f;
        SetitngsMenu.SetActive(true);
        gameObject.SetActive(false);
    }


    


    public void PauseButtonClicked()
    {
        ReplayDMMovements.PauseButtonClicked();
    }
    
    
    
    
    
    public void UpdateTimeSpeedDropDown(int index) => Parameters.TimeAcceleration = timeAccelerationMap[index];




    private void UpdateTimeSliderAndText()
    {
        int curIndex = DrillingDataManager.Index;
        TimeSlider.value = curIndex;
        DrillingDataCSV curData = DrillingData[curIndex];
        string date = curData.Date;
        CurrentTimeText.text = date.Replace(' ', '\n');
        ReplayDMMovements.SetEquipmentPosition(curData);
    }




    public void UpdateTimeSliderValueChanged(float val) => ReplayDMMovements.SetCurrentIndex((int)val);
    
    
    
    
    
    public void UpdateSocketDepthText(float depth) => SocketDepthText.text = $"Socket depth: {string.Format("{0:0.##}", -depth)}m";
}