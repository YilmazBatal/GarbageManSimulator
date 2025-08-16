using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler , IDeselectHandler
{
    [SerializeField] private Color hoverColor = new Color(159f / 255f, 68f / 255f, 75f / 255f);
    private TextMeshProUGUI buttonText;
    void Start()
    {
        buttonText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        buttonText.color = Color.white;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.color = Color.white;
    }
}
