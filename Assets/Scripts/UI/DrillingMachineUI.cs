using UnityEngine;
using UnityEngine.SceneManagement;

public class DrillingMachineUI : MonoBehaviour
{
    public void ReturnButtonClicked() => SceneManager.LoadScene("MainMenu");
}