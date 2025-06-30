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
    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600 };
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private TextMeshProUGUI CurrentTimeText;
    
    
    
    
    
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
        TimeSlider.value = DrillingDataManager.Index;
        string date = DrillingData[curIndex].Date;
        CurrentTimeText.text = date.Replace(' ', '\n');
    }




    public void UpdateTimeSliderValueChanged(float val) => ReplayDMMovements.SetCurrentIndex((int)val);
}