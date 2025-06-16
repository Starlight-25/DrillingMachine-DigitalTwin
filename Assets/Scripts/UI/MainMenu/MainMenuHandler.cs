using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject CreditsMenu;
    
    
    
    
    
    public void DMModelButtonClicked() => SceneManager.LoadScene("DrillingMachine");



    

    public void SettingsButtonClicked()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }


    


    public void CreditsButtonClicked()
    {
        CreditsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }
    
    
    
    
    
    public void QuitButtonClicked() => Application.Quit();
}
