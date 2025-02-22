using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebDebug : MonoBehaviour
{
    public SampleWebView webScript;
    public GameObject webPlate;
    public string url;

    public void SetUrl()
    {
        webScript.Url = url;
        webPlate.SetActive(true);
    }
}
