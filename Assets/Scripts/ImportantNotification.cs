using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImportantNotification : MonoBehaviour
{
    public GameObject ErrorSpawner;
    public Transform ErrorSpawnPlace;
    public GameObject ErrorObject;
    public int MainMInt;
    public int LibMInt;

    void Start()
    {
        MainMInt = PlayerPrefs.GetInt("MMI");
        LibMInt = PlayerPrefs.GetInt("LMI");
        MainMessage();
    }

    public void MainMessage()
    {
        if(MainMInt == 0)
        {
            MainMInt = 1;
            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = "Данное приложение находится в разработке. Буду рад, если вы оставите отзыв или предложите какую-либо полезную функцию здесь:  <link=\"https://github.com/emilchiks/SchoolProject\"><u>github.com/emilchiks/SchoolProject<u></link>";
            PlayerPrefs.SetInt("MMI", MainMInt);
            PlayerPrefs.Save();
        }
    }



    public void Library()
    {
        if(LibMInt == 0)
        {
            LibMInt = 1;
            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = "Данная версия библиотеки неполная и будет пополняться в будущем";
            PlayerPrefs.SetInt("LMI",LibMInt);
            PlayerPrefs.Save();
        }
    }
}
