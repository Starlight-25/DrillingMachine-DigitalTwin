using TMPro;
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

    [SerializeField] private TMP_Dropdown ScreenModeDropDown;
    [SerializeField] private TMP_Dropdown FreshrateDropDown;
    
    [SerializeField] private Slider MouseSensibilitySlider;
    private TextMeshProUGUI MouseSensiValueText;
    [SerializeField] private Slider ScrollSensibilitySlider;
    private TextMeshProUGUI ScrollSensiValueText;
    [SerializeField] private Slider HeightNavSensibilitySlider;
    private TextMeshProUGUI HeightNavSensiValueText;
    
    
    
    

    private void Start()
    {
        MouseSensiValueText = MouseSensibilitySlider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>();
        ScrollSensiValueText = ScrollSensibilitySlider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>();
        HeightNavSensiValueText = HeightNavSensibilitySlider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>();

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
        Sensibility sensibility = SettingsHandler.Settings.Sensibility;
        SetMouseSliderValue(sensibility);
        SetScrollSliderValue(sensibility);
        SetHeightNavSliderValue(sensibility);
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

    private void SetMouseSliderValue(Sensibility sensibility)
    {
        MouseSensibilitySlider.value = sensibility.MouseRotation;
        MouseSensiValueText.text = sensibility.MouseRotation.ToString();
    }

    private void SetScrollSliderValue(Sensibility sensibility)
    {
        ScrollSensibilitySlider.value = sensibility.Zoom;
        ScrollSensiValueText.text = sensibility.Zoom.ToString();
    }

    private void SetHeightNavSliderValue(Sensibility sensibility)
    {
        HeightNavSensibilitySlider.value = sensibility.HeightNavigation;
        HeightNavSensiValueText.text = sensibility.HeightNavigation.ToString();
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





    public void UpdateMouseSensibility()
    {
        Sensibility sensibility = SettingsHandler.Settings.Sensibility;
        sensibility.MouseRotation = (int)MouseSensibilitySlider.value;
        MouseSensiValueText.text = sensibility.MouseRotation.ToString();
        SettingsHandler.SaveSettingsData();
        SettingsHandler.ApplySettings();
    }
    
    
    
    
    
    public void UpdateScrollSensibility()
    {
        Sensibility sensibility = SettingsHandler.Settings.Sensibility;
        sensibility.Zoom = (int)ScrollSensibilitySlider.value;
        ScrollSensiValueText.text = sensibility.Zoom.ToString();
        SettingsHandler.SaveSettingsData();
        SettingsHandler.ApplySettings();
    }



    

    public void UpdateHeightNavSensibility()
    {
        Sensibility sensibility = SettingsHandler.Settings.Sensibility;
        sensibility.HeightNavigation = (int)HeightNavSensibilitySlider.value;
        HeightNavSensiValueText.text = sensibility.HeightNavigation.ToString();
        SettingsHandler.SaveSettingsData();
        SettingsHandler.ApplySettings();
    }
}