using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabManager : MonoBehaviour
{
    public int defTab;
    public GameObject Tab_1;
    public GameObject Tab_2;
    public GameObject Tab_3;
    public GameObject Tab_4;

    public TextMeshProUGUI MainHeader;

    void Start()
    {
        int defTab = PlayerPrefs.GetInt("defTab");

        if(defTab == 0)
        {
            Tab_1.SetActive(true);
            Tab_2.SetActive(false);
            Tab_3.SetActive(false);
            Tab_4.SetActive(false);
            ClickTabOne();
        }
        if(defTab == 1)
        {
            Tab_1.SetActive(false);
            Tab_2.SetActive(true);
            Tab_3.SetActive(false);
            Tab_4.SetActive(false);
            ClickTabTwo();
        }
        if(defTab == 2)
        {
            Tab_1.SetActive(false);
            Tab_2.SetActive(false);
            Tab_3.SetActive(true);
            Tab_4.SetActive(false);
            ClickTabThree();
        }
        if(defTab == 3)
        {
            Tab_1.SetActive(false);
            Tab_2.SetActive(false);
            Tab_3.SetActive(false);
            Tab_4.SetActive(true);
            ClickTabFour();
        }
    }

    public void ChangeOne()
    {
        defTab = 0;
        PlayerPrefs.SetInt("defTab", defTab);
        PlayerPrefs.Save();
    }
    public void ChangeTwo()
    {
        defTab = 1;
        PlayerPrefs.SetInt("defTab", defTab);
        PlayerPrefs.Save();
    }
    public void ChangeThree()
    {
        defTab = 2;
        PlayerPrefs.SetInt("defTab", defTab);
        PlayerPrefs.Save();
    }
    public void ChangeFour()
    {
        defTab = 3;
        PlayerPrefs.SetInt("defTab", defTab);
        PlayerPrefs.Save();
    }


    public void ClickTabOne()
    {
        MainHeader.text = "Расписание";
    }
    public void ClickTabTwo()
    {
        MainHeader.text = "Мероприятия и конкурсы";
    }
    public void ClickTabThree()
    {
        MainHeader.text = "Библиотека";
    }
    public void ClickTabFour()
    {
        MainHeader.text = "Настройки";
    }
}
