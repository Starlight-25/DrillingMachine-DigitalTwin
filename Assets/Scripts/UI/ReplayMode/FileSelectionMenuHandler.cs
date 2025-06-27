using System;
using System.IO;
using TMPro;
using UnityEditor;
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
        string path = EditorUtility.OpenFilePanel("Open Drilling Data", "", "csv");
        if (DrillingDataManager.Load(path))
        {
            SelectedFileText.text = $"Selected File: {Path.GetFileName(path)}";
            ConfimButton.SetActive(true);
        }
        else SelectedFileText.text = "Invalid CSV file format";
    }



    

    public void ConfirmButtonClicked()
    {
        Time.timeScale = 1f;
        MainUI.SetActive(true);
        DrillingMachine.SetActive(true);
        gameObject.SetActive(false);
    }
}
