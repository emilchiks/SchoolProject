using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Получаем индекс кликнутой ссылки
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, null);

        if (linkIndex != -1)
        {
            // Извлекаем данные ссылки
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];

            // Открываем ссылку в браузере
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
