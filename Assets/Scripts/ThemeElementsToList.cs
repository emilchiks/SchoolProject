using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ThemeElementsToList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MainText;
    [SerializeField] private Image IconImage;
    [SerializeField] private Image Background;
    [SerializeField] private Image IconBackground;
    [SerializeField] private UIColorSwitch themeScript;
    [SerializeField] private string category;

    public RectTransform layoutGroupTransform;

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
                themeScript.AddImageToList(IconImage, "HighContrast");
                themeScript.AddTextToList(MainText);
        
                themeScript.LoadConfig();

                UpdateLayout();
            }
        }
    }

    public void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupTransform);
    }
}
