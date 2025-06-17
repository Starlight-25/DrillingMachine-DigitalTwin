using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject SettingsMenu;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;
    
    [SerializeField] private TextMeshProUGUI ExcavatedDepthText;
    [SerializeField] private TMP_Dropdown TimeSpeedDropDown;
    [SerializeField] private TextMeshProUGUI DateText;
    private int TimeAcceleration = 1;
    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600, 7200, 18000, 43200, 86400 };
    private DateTime startTime = new DateTime(2025, 1, 1, 0, 0, 0);
    private double curTime = 0;
    
    
    
    
    
    private void Start()
    {
        ReturnInputAction = UIInput.actions["Return"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
        UpdateTimeDate();
    }

    
    
    
    
    public void SettingsButtonClicked()
    {
        Time.timeScale = 0f;
        MainUI.SetActive(false);
        SettingsMenu.SetActive(true);
    }





    public void UpdateExcavatedDepthText(float depth) => ExcavatedDepthText.text = $"Excavated depth: {string.Format("{0:0.##}", -depth)}m";





    public void UpdateTimeSpeedDropDown()
    {
        TimeAcceleration = timeAccelerationMap[TimeSpeedDropDown.value];
    }

    
    
    
    
    private void UpdateTimeDate()
    {
        curTime += Time.deltaTime * TimeAcceleration;
        DateText.text = startTime.AddSeconds(curTime).ToString("dd/MM/yyyy\nHH:mm:ss");
    }
}