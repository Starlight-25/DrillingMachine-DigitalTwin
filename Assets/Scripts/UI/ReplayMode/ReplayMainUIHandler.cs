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
    [SerializeField] private ReplayDMMovements ReplayDMMovements;

    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600 };
    [SerializeField] private Slider TimeSlider;
    
    
    
    
    
    private void Start()
    {
        ReturnInputAction = PlayerInput.actions["Return"];
        PauseInputAction = PlayerInput.actions["Pause"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
        if (PauseInputAction.triggered) PauseButtonClicked();
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
}