using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AboutTabLinks : MonoBehaviour
{
    public TextMeshProUGUI GitHubLinkText;
    public TextMeshProUGUI GitHubIsuesLinkText;
    void Start()
    {
        GitHubLinkText.text = "<link=\"https://github.com/emilchiks/SchoolProject\"><u>GitHub<u></link>";
        GitHubIsuesLinkText.text = "<link=\"https://github.com/emilchiks/SchoolProject/issues\"><u>Сообщить об ошибке<u></link>";
    }
}
