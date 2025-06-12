using UnityEngine;
using UnityEngine.InputSystem;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject SettingsMenu;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;

    
    
    
    
    private void Start()
    {
        ReturnInputAction = UIInput.actions["Return"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
    }

    
    
    
    
    public void SettingsButtonClicked()
    {
        Time.timeScale = 0f;
        MainUI.SetActive(false);
        SettingsMenu.SetActive(true);
    }
}