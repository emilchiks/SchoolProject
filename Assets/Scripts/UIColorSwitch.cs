using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIColorSwitch : MonoBehaviour
{
    [Header("Arrays of UI Elements")]

    public Image[] uiHighContrastImages;
    public Image[] uiContrastImages;
    public Image[] uiDefaultImages;
    public TextMeshProUGUI[] texts;

    [Header("Color Settings")]
    public Color targetBaseColor = Color.black; // Цвет, на который меняются объекты
    public Color originalBaseColor = Color.white; // Исходный цвет объектов

    public Color targetContrastColor = Color.white;
    public Color originalContrastColor = Color.white;

    public Color targetHighContrastColor = Color.white;
    public Color originalHighContrastColor = Color.white;

    public Color targetTextColor = Color.white; // Цвет, на который меняются объекты
    public Color originalTextColor = Color.black; // Исходный цвет объектов
    public float duration = 2f; // Длительность анимации

    public bool isToggled = false; // Флаг переключения состояния

    public Color newBaseColor;
    public Color newContrastColor;
    public Color newHighContrastColor;
    public Color newTextColor;
    

    // Метод для смены цвета, вызывается кнопкой
    public void ToggleObjectColors()
    {
        newBaseColor = isToggled ? originalBaseColor : targetBaseColor;
        newContrastColor = isToggled ? originalContrastColor : targetContrastColor;
        newHighContrastColor = isToggled ? originalHighContrastColor : targetHighContrastColor;
        newTextColor = isToggled ? originalTextColor : targetTextColor;

        ChangeColors();

        isToggled = !isToggled; // Переключение состояния
    }

    public void ChangeColors()
    {
        // Изменение цвета UI-изображений
        foreach (Image uiDImage in uiDefaultImages)
        {
            if (uiDImage != null)
            {
                uiDImage.DOColor(newBaseColor, duration);
            }
        }

        foreach (Image uiCImage in uiContrastImages)
        {
            if (uiCImage != null)
            {
                uiCImage.DOColor(newContrastColor, duration);
            }
        }

        foreach (Image uiHCImage in uiHighContrastImages)
        {
            if (uiHCImage != null)
            {
                uiHCImage.DOColor(newHighContrastColor, duration);
            }
        }

        // Изменение цвета текстов
        foreach (TextMeshProUGUI text in texts)
        {
            if (text != null)
            {
                text.DOColor(newTextColor, duration);
            }
        }
    }

    public void MaterialFirst()
    {
        targetBaseColor = new Color(0.1f / 1.0f, 0.1f / 1.0f, 0.1f / 1.0f);
        targetContrastColor = new Color(0.2f / 1.0f, 0.2f / 1.0f, 0.2f / 1.0f);
        targetHighContrastColor = new Color(0.3f / 1.0f, 0.3f / 1.0f, 0.3f / 1.0f);

        originalBaseColor = new Color(1 / 1.0f, 1 / 1.0f, 1 / 1.0f);
        originalContrastColor = new Color(0.9f / 1.0f, 0.9f / 1.0f, 0.9f / 1.0f);
        originalHighContrastColor = new Color(0.8f / 1.0f, 0.8f / 1.0f, 0.8f / 1.0f);

        // Обновляем цвета для текущего состояния
        newBaseColor = isToggled ? targetBaseColor : originalBaseColor;
        newContrastColor = isToggled ? targetContrastColor : originalContrastColor;
        newHighContrastColor = isToggled ? targetHighContrastColor : originalHighContrastColor;
        newTextColor = isToggled ? targetTextColor : originalTextColor;

        ChangeColors();
    }
    public void MaterialTwo()
    {
        targetBaseColor = new Color(81 / 255f, 52 / 255f, 49 / 255f);
        targetContrastColor = new Color(117 / 255f, 48 / 255f, 65 / 255f);
        targetHighContrastColor = new Color(255 / 255f, 177 / 255f, 192 / 255f);

        originalBaseColor = new Color(255 / 255f, 248 / 255f, 247 / 255f);
        originalContrastColor = new Color(255 / 255f, 177 / 255f, 192 / 255f);
        originalHighContrastColor = new Color(146 / 255f, 71 / 255f, 89 / 255f);

        // Обновляем цвета для текущего состояния
        newBaseColor = isToggled ? targetBaseColor : originalBaseColor;
        newContrastColor = isToggled ? targetContrastColor : originalContrastColor;
        newHighContrastColor = isToggled ? targetHighContrastColor : originalHighContrastColor;
        newTextColor = isToggled ? targetTextColor : originalTextColor;

        ChangeColors();
    }
}
