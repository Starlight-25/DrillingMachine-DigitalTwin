using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReplayMainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject SetitngsMenu;
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction ReturnInputAction;
    
    
    
    
    
    private void Start()
    {
        ReturnInputAction = PlayerInput.actions["Return"];
    }

    
    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) SettingsButtonClicked();
    }

    
    
    
    
    public void SettingsButtonClicked()
    {
        Time.timeScale = 0f;
        SetitngsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}