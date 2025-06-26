using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject CreditsMenu;
    
    
    
    
    
    public void DrillingModeButtonClicked() => SceneManager.LoadScene("DrillingMachine");





    public void ReplayModeButtonClicked() => SceneManager.LoadScene("Replay");



    

    public void SettingsButtonClicked()
    {
        SettingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }


    


    public void CreditsButtonClicked()
    {
        CreditsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    
    
    
    
    
    public void QuitButtonClicked() => Application.Quit();
}
