using UnityEngine;
using UnityEngine.InputSystem;

public class ReplayMainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject SetitngsMenu;
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction ReturnInputAction;
    private InputAction PauseInputAction;

    [SerializeField] private ReplayDMMovements ReplayDMMovements;
    
    
    
    
    
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
}