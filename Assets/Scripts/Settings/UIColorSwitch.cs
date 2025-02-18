using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIColorSwitch : MonoBehaviour
{
    [Header("Arrays of UI Elements")]
    public List<Image> uiHighContrastImages = new List<Image>();
    public List<Image> uiContrastImages = new List<Image>();
    public List<Image> uiDefaultImages = new List<Image>();
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public List<Image> NonMaterialImages = new List<Image>();

    [Header("Color Settings")]
    public Color targetBaseColor;
    public Color originalBaseColor;
    public Color targetContrastColor;
    public Color originalContrastColor;
    public Color targetHighContrastColor;
    public Color originalHighContrastColor;
    public Color targetTextColor;
    public Color originalTextColor;
    public float duration = 2f;
    public bool isToggled = false;
    public Toggle ThemeToggle;

    public Color newBaseColor;
    public Color newContrastColor;
    public Color newHighContrastColor;
    public Color newTextColor;

    public int currentTheme;
    /*
    0 - White
    1- Dark
    */
    public int currentColor;
    /*
    0 - clear
    1 - pink
    2 - blue
    3 - green
    4 - orange
    5 - purple
    */

    void Start()
    {
        LoadConfig();
    }

    public void LoadConfig()
    {
        currentTheme = PlayerPrefs.GetInt("currentTheme");
        currentColor = PlayerPrefs.GetInt("currentColor");

        if(currentTheme == 1)
        {
            ThemeToggle.isOn = true;
        }

        switch (currentColor)
        {
            case 0:
                MaterialFirst();
                break;
            case 1:
                MaterialTwo();
                break;
            case 2:
                MaterialThree();
                break;
            case 3:
                MaterialFour();
                break;
            case 4:
                MaterialFive();
                break;
        }
    }

    public void ToggleObjectColors()
    {
        newBaseColor = isToggled ? originalBaseColor : targetBaseColor;
        newContrastColor = isToggled ? originalContrastColor : targetContrastColor;
        newHighContrastColor = isToggled ? originalHighContrastColor : targetHighContrastColor;
        newTextColor = isToggled ? originalTextColor : targetTextColor;

        ChangeColors();
        isToggled = !isToggled;

        if(isToggled == true)
        {
            currentTheme = 1;
            PlayerPrefs.SetInt("currentTheme", currentTheme);
            PlayerPrefs.Save();
        }
        else
        {
            currentTheme = 0;
            PlayerPrefs.SetInt("currentTheme", currentTheme);
            PlayerPrefs.Save();
        }
    }

    public void ChangeColors()
    {
        foreach (Image uiDImage in uiDefaultImages)
            if (uiDImage != null) uiDImage.DOColor(newBaseColor, duration);

        foreach (Image uiCImage in uiContrastImages)
            if (uiCImage != null) uiCImage.DOColor(newContrastColor, duration);

        foreach (Image uiHCImage in uiHighContrastImages)
            if (uiHCImage != null) uiHCImage.DOColor(newHighContrastColor, duration);

        foreach (TextMeshProUGUI text in texts)
            if (text != null) text.DOColor(newTextColor, duration);
            
        foreach (Image uiNMImage in NonMaterialImages)
            if (uiNMImage != null) uiNMImage.DOColor(newTextColor, duration);
    }

    public void MaterialFirst()
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = Color.white;

        originalBaseColor = Color.white;
        originalContrastColor = new Color(0.9f, 0.9f, 0.9f);
        originalHighContrastColor = Color.black;

        currentColor = 0;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();

        UpdateColors();
    }

    public void MaterialTwo()
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = new Color(255 / 255f, 186 / 255f, 203 / 255f); // #FFBACB - Яркий акцент
    
        originalBaseColor = new Color(255 / 255f, 240 / 255f, 245 / 255f); // #FFF0F5 - Светлый фон
        originalContrastColor = new Color(255 / 255f, 210 / 255f, 220 / 255f); // #FFD2DC - Контраст
        originalHighContrastColor = new Color(191 / 255f, 40 / 255f, 95 / 255f); // #BF285F - Глубокий розовый

        currentColor = 1;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();
    
        UpdateColors();
    }

    public void MaterialThree() // Oceanic Theme
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = new Color(173 / 255f, 232 / 255f, 244 / 255f);

        originalBaseColor = new Color(255 / 255f, 250 / 255f, 244 / 255f);
        originalContrastColor = new Color(200 / 255f, 230 / 255f, 233 / 255f);
        originalHighContrastColor = new Color(150 / 255f, 200 / 255f, 220 / 255f);

        currentColor = 2;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();

        UpdateColors();
    }

    public void MaterialFour() // Forest Theme
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = new Color(129 / 255f, 199 / 255f, 132 / 255f);

        originalBaseColor = new Color(240 / 255f, 255 / 255f, 240 / 255f);
        originalContrastColor = new Color(200 / 255f, 225 / 255f, 200 / 255f);
        originalHighContrastColor = new Color(150 / 255f, 190 / 255f, 150 / 255f);

        currentColor = 3;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();

        UpdateColors();
    }

    public void MaterialFive() // Sunset Theme
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = new Color(255 / 255f, 204 / 255f, 128 / 255f);

        originalBaseColor = new Color(255 / 255f, 235 / 255f, 205 / 255f);
        originalContrastColor = new Color(255 / 255f, 204 / 255f, 128 / 255f);
        originalHighContrastColor = new Color(255 / 255f, 171 / 255f, 89 / 255f);

        currentColor = 4;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();

        UpdateColors();
    }

    public void MaterialSix() // Розовая Material You
    {
        targetBaseColor = new Color(0.1f, 0.1f, 0.1f);
        targetContrastColor = new Color(0.2f, 0.2f, 0.2f);
        targetHighContrastColor = new Color(235 / 255f, 175 / 255f, 255 / 255f); // #EBAFFF - Светлый акцент
    
        originalBaseColor = new Color(250 / 255f, 240 / 255f, 255 / 255f); // #FAF0FF - Светлый фон
        originalContrastColor = new Color(225 / 255f, 200 / 255f, 250 / 255f); // #E1C8FA - Контраст
        originalHighContrastColor = new Color(130 / 255f, 45 / 255f, 165 / 255f); // #822DA5 - Глубокий пурпурный

        currentColor = 5;
        PlayerPrefs.SetInt("currentColor", currentColor);
        PlayerPrefs.Save();
    
        UpdateColors();
    }


    private void UpdateColors()
    {
        newBaseColor = isToggled ? targetBaseColor : originalBaseColor;
        newContrastColor = isToggled ? targetContrastColor : originalContrastColor;
        newHighContrastColor = isToggled ? targetHighContrastColor : originalHighContrastColor;
        newTextColor = isToggled ? targetTextColor : originalTextColor;
        ChangeColors();
    }

    public void AddImageToList(Image image, string category)
    {
        switch (category)
        {
            case "HighContrast":
                uiHighContrastImages.Add(image);
                break;
            case "Contrast":
                uiContrastImages.Add(image);
                break;
            case "Default":
                uiDefaultImages.Add(image);
                break;
            case "NonMaterial":
                NonMaterialImages.Add(image);
                break;
            default:
                Debug.LogWarning("Неизвестная категория: " + category);
                break;
        }
    }

    public void AddTextToList(TextMeshProUGUI textElement)
    {
        if (textElement != null)
        {
            texts.Add(textElement);
        }
        else
        {
            Debug.LogWarning("Попытка добавить null объект в список текстов.");
        }
    }
}
