using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingsMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject MainUI;

    [SerializeField] private PlayerInput UIInput;
    private InputAction ReturnInputAction;

    
    
    

    private void Start()
    {
        ReturnInputAction = UIInput.actions["Return"];
    }

    
    
    

    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }

    
    
    

    public void ReturnButtonClicked()
    {
        SettingsMenu.SetActive(false);
        MainUI.SetActive(true);
    }



    

    public void QuitButtonClicked() => SceneManager.LoadScene("MainMenu");
}