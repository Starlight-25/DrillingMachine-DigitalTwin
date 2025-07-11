using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private TextMeshProUGUI ButtonText;
    private Color BasicColor = new Color(0xF0 / 255f, 0x8A / 255f, 0x04 / 255f); //f08a04
    private Color HoverColor = new Color(0x22 / 255f, 0x4c / 255f, 0x5a / 255f); //224c5a
    
    
    
    
    
    private void Start() => ButtonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    
    
    

    public void OnPointerEnter(PointerEventData eventData) => ButtonText.color = HoverColor;

    public void OnPointerExit(PointerEventData eventData) => ButtonText.color = BasicColor;

    public void OnPointerDown(PointerEventData eventData) => ButtonText.color = HoverColor;

    public void OnPointerUp(PointerEventData eventData) => ButtonText.color = BasicColor;
}