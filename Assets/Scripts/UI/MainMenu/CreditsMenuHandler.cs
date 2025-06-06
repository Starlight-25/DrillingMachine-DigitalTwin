using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private GameObject MainMenu;

    [SerializeField] private PlayerInput MainMenuInput;
    private InputAction ReturnInputAction;


    
    
    
    private void Start() => ReturnInputAction = MainMenuInput.actions["Return"];
    
    
    
    

    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }

    
    
    

    public void ReturnButtonClicked()
    {
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }
}