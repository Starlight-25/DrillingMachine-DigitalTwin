using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void DMModelButtonClicked()
    {
        SceneManager.LoadScene("DrillingMachine");
    }
    
    
    public void QuitButtonClicked() => Application.Quit();
}
