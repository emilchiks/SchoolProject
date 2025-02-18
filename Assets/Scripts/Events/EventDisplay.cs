using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image eventImage; // Добавляем Image компонент
    [SerializeField] private Sprite[] eventSprites; // Массив спрайтов
    [SerializeField] private Image Background;
    [SerializeField] private Image IconBackground;
    [SerializeField] private Image IconObject;
    [SerializeField] private UIColorSwitch themeScript;
    [SerializeField] private string category;

    void Start()
    {
        GameObject obj = GameObject.Find("ThemeManager");
        if (obj != null)
        {
            themeScript = obj.GetComponent<UIColorSwitch>();
            if (themeScript != null)
            {
                themeScript.AddImageToList(Background, "Contrast");
                themeScript.AddImageToList(IconBackground, "Default");
                themeScript.AddImageToList(IconObject, "NonMaterial");
                themeScript.AddTextToList(dateText);
                themeScript.AddTextToList(titleText);
                themeScript.AddTextToList(descriptionText);
        
                themeScript.LoadConfig();
            }
        }
    }


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
