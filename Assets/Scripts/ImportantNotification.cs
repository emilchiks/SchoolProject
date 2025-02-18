using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImportantNotification : MonoBehaviour
{
    public GameObject ErrorSpawner;
    public Transform ErrorSpawnPlace;
    public GameObject ErrorObject;
    void Start()
    {
        MainMessage();
        Library();
    }

    public void MainMessage()
    {
        ErrorSpawner.SetActive(true);
        GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
        Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
        TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
        panelErrorText.text = "Данное приложение находится в разработке. Буду рад, если вы оставите отзыв или предложите какую-либо полезную функцию здесь:  <link=\"https://github.com/emilchiks/SchoolProject\"><u>github.com/emilchiks/SchoolProject<u></link>";
    }



    public void Library()
    {
        
        ErrorSpawner.SetActive(true);
        GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
        Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
        TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
        panelErrorText.text = "Данная версия библиотеки неполная и будет пополняться в будущем"; 
    }
}
