using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOnlineLibrary : MonoBehaviour
{
    public SampleWebView webScript;
    public GameObject webPlate;
    public string  url = "https://uchebnik.mos.ru/main";

    public void SetUrl()
    {
        webScript.Url = url;
        webPlate.SetActive(true);
    }
}
