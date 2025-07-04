using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CreditsMenuHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private TextMeshProUGUI CreditsText;

    [SerializeField] private PlayerInput MainMenuInput;
    private InputAction ReturnInputAction;


    
    
    
    private void Start()
    {
        ReturnInputAction = MainMenuInput.actions["Return"];
        CreditsText.text = Resources.Load<TextAsset>("credits").text;
    }
    
    
    
    

    private void Update()
    {
        if (ReturnInputAction.triggered) ReturnButtonClicked();
    }

    
    
    

    public void ReturnButtonClicked()
    {
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }



    

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(CreditsText, eventData.position, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = CreditsText.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();
            Application.OpenURL(linkId);
        }
    }
}