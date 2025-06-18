using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ParameterMenu;
    [SerializeField] private RectTransform ParameterButton;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;
    private InputAction ShowParameter;
    
    [SerializeField] private TextMeshProUGUI DateText;
    private DateTime startTime = new DateTime(2025, 1, 1, 0, 0, 0);
    private double curTime = 0;
    [SerializeField] private TextMeshProUGUI ExcavatedDepthText;

    [SerializeField] private Parameters Parameters;
    
    
    
    
    
    private void Start()
    {
        ReturnInputAction = UIInput.actions["Return"];
        ShowParameter = UIInput.actions["ShowParameters"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
        if (ShowParameter.triggered)
        UpdateTimeDate();
    }

    
    
    
    
    public void SettingsButtonClicked()
    {
        Time.timeScale = 0f;
        MainUI.SetActive(false);
        SettingsMenu.SetActive(true);
    }



    public void ParametterButtonClicked()
    {
        ParameterMenu.SetActive(!ParameterMenu.activeInHierarchy);
        ParameterButton.position += Vector3.right * (ParameterMenu.activeInHierarchy ? -500 : 500);
    }





    public void UpdateExcavatedDepthText(float depth) => ExcavatedDepthText.text = $"Socket depth: {string.Format("{0:0.##}", -depth)}m";

    
    
    
    
    private void UpdateTimeDate()
    {
        curTime += Time.deltaTime * Parameters.TimeAcceleration;
        DateText.text = startTime.AddSeconds(curTime).ToString("dd/MM/yyyy\nHH:mm:ss");
    }
}