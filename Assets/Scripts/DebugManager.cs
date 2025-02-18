using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public GameObject Console;
    public Toggle ConsoleToggle;

    public void ToggleConsole()
    {
        if(ConsoleToggle.isOn == true)
        {
            Console.SetActive(true);
        }
        else
        {
            Console.SetActive(false);
        }
    }
}
