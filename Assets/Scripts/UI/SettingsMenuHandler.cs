using System;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuHandler : MonoBehaviour, ISettingsUpdater
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject MainUI;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;

    [SerializeField] private SettingsHandler SettingsHandler;

    [SerializeField] private TMP_Dropdown FreshrateDropDown;
    
    
    

    private void Start()
    {
        SettingsHandler.Add(this);
        UpdateFromSettings();

        ReturnInputAction = UIInput.actions["Return"];
    }

    
    
    

    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }

    
    
    

    public void ReturnButtonClicked()
    {
        Time.timeScale = 1f;
        SettingsMenu.SetActive(false);
        MainUI.SetActive(true);
    }



    

    public void QuitButtonClicked() => SceneManager.LoadScene("MainMenu");





    public void UpdateFreshRate(int val)
    {
        Application.targetFrameRate = val == 4 ? -1 : int.Parse(FreshrateDropDown.options[val].text);
        SettingsHandler.Settings.FPS = Application.targetFrameRate;
        SettingsHandler.SaveSettingsData();
    }

    
    
    
    
    public void UpdateFromSettings()
    {
        SetFreshRateDropDownValFromSettings();
    }

    private void SetFreshRateDropDownValFromSettings()
    {
        int fps = SettingsHandler.Settings.FPS;
        if (fps == -1)
        {
            FreshrateDropDown.value = 4;
            FreshrateDropDown.RefreshShownValue();
            return;
        }

        string fpsString = fps.ToString();
        for (int i = 0; i < FreshrateDropDown.options.Count; i++)
        {
            if (FreshrateDropDown.options[i].text == fpsString)
            {
                FreshrateDropDown.value = i;
                FreshrateDropDown.RefreshShownValue();
                return;
            }
        }
    }
}