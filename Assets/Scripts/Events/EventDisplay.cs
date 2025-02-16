using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image eventImage; // Добавляем Image компонент
    [SerializeField] private Sprite[] eventSprites; // Массив спрайтов

    public void SetEventData(string date, string title, string description, int imageIndex)
    {
        dateText.text = date;
        titleText.text = title;
        descriptionText.text = description;

        // Устанавливаем изображение по индексу
        if (imageIndex >= 0 && imageIndex < eventSprites.Length)
        {
            eventImage.sprite = eventSprites[imageIndex];
        }
        else
        {
            Debug.LogWarning($"Неверный индекс изображения: {imageIndex}");
        }
    }
}
