using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRateChanger : MonoBehaviour
{
    public Toggle forOld;
    public Toggle forNormal;
    public Toggle forNew;

    public int fpsInt;

    void Start()
    {
        int fpsInt = PlayerPrefs.GetInt("fpsInt");
        if(fpsInt == 0)
        {
            forOld.isOn = true;
            forNormal.isOn = false;
            forNew.isOn = false;
            forOldPhones();
        }
        else if(fpsInt == 1)
        {
            forOld.isOn = false;
            forNormal.isOn = true;
            forNew.isOn = false;
            forNormalPhones();
        }
        else if(fpsInt == 2)
        {
            forOld.isOn = false;
            forNormal.isOn = false;
            forNew.isOn = true;
            forNewPhones();
        }
    }

    public void forOldPhones()
    {
        Application.targetFrameRate = 30;
        fpsInt = 0;
        PlayerPrefs.SetInt("fpsInt", fpsInt);
        PlayerPrefs.Save(); // Сохраняем изменения
    }
    
    public void forNormalPhones()
    {
        Application.targetFrameRate = 60;
        fpsInt = 1;
        PlayerPrefs.SetInt("fpsInt", fpsInt);
        PlayerPrefs.Save();
    }
    
    public void forNewPhones()
    {
        Application.targetFrameRate = 120;
        fpsInt = 2;
        PlayerPrefs.SetInt("fpsInt", fpsInt);
        PlayerPrefs.Save();
    }
}
