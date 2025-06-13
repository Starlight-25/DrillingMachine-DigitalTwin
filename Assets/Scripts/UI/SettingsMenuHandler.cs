using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingsMenuHandler : MonoBehaviour, ISettingsUpdater
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject MainUI;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;

    [SerializeField] private SettingsHandler SettingsHandler;

    [SerializeField] private TMP_Dropdown ScreenModeDropDown;
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
    
    
    
    
    
    public void UpdateFromSettings()
    {
        SetFreshRateDropDownValFromSettings();
        SetScreenModeFromSettings();
    }

    private void SetScreenModeFromSettings()
    {
        int screenMode = SettingsHandler.Settings.ScreenMode;
        ScreenModeDropDown.value = screenMode;
        ScreenModeDropDown.RefreshShownValue();
        SetScreenMode(screenMode);
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


    
    
    

    public void ReturnButtonClicked()
    {
        Time.timeScale = 1f;
        SettingsMenu.SetActive(false);
        MainUI.SetActive(true);
    }



    

    public void QuitButtonClicked() => SceneManager.LoadScene("MainMenu");





    public void UpdateFreshRate()
    {
        int val = FreshrateDropDown.value;
        Application.targetFrameRate = val == 4 ? -1 : int.Parse(FreshrateDropDown.options[val].text);
        SettingsHandler.Settings.FPS = Application.targetFrameRate;
        SettingsHandler.SaveSettingsData();
    }

    



    public void UpdateScreenMode()
    {
        int screenMode = ScreenModeDropDown.value;
        SetScreenMode(screenMode);
        SettingsHandler.Settings.ScreenMode = screenMode;
        SettingsHandler.SaveSettingsData();
    }

    private void SetScreenMode(int screenModeValue) => Screen.fullScreenMode = (FullScreenMode)screenModeValue;
}