using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentDateRequester : MonoBehaviour
{
    public TextMeshProUGUI dateText;

    void Start()
    {
        if (dateText != null)
        {
            StartCoroutine(UpdateDateEveryMinute());
        }
    }

    IEnumerator UpdateDateEveryMinute()
    {
        while (true)
        {
            dateText.text = System.DateTime.Now.ToString("dd.MM.yyyy");
            yield return new WaitForSeconds(60);
        }
    }
}
