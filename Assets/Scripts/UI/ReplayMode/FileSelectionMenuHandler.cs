using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FileSelectionMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject DrillingMachine;
    [SerializeField] private GameObject ConfimButton;
    [SerializeField] private TextMeshProUGUI SelectedFileText;
    
    [SerializeField] private PlayerInput PlayerInput;
    private InputAction ReturnInputAction;

    [SerializeField] private DrillingDataManager DrillingDataManager;
    
    
    
    
    
    private void Start()
    {
        ReturnInputAction = PlayerInput.actions["Return"];
        Time.timeScale = 0f;
    }


    
    
    
    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }


    
    

    public void ReturnButtonClicked() => SceneManager.LoadScene("MainMenu");





    public void OpenCSVFileButtonClicked()
    {
        string path = OpenFileDialog();
        if (DrillingDataManager.Load(path))
        {
            SelectedFileText.text = $"Selected File: {Path.GetFileName(path)}";
            ConfimButton.SetActive(true);
        }
        else SelectedFileText.text = "Invalid CSV file format";
    }

    private string OpenFileDialog()
    {
        var extensions = new[] {
            new ExtensionFilter("CSV Files", "csv")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Drilling Data", "", extensions, false);
        return paths.Length != 0 ? paths[0] : "";
    }



    

    public void ConfirmButtonClicked()
    {
        Time.timeScale = 1f;
        MainUI.SetActive(true);
        DrillingMachine.SetActive(true);
        gameObject.SetActive(false);
    }
}
