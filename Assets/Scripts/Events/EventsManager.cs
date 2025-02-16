using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq; // Используем Newtonsoft.Json для парсинга JSON

public class EventsManager : MonoBehaviour
{
    [SerializeField] private GameObject eventPrefab; // Префаб для событий
    public GameObject SpawnPoint;
    private string repoUrl = "https://api.github.com/repos/emilchiks/Storage-For-School-Project/contents/Events?ref=main";

    void Start()
    {
        StartCoroutine(LoadEvents());
    }

    IEnumerator LoadEvents()
    {
        UnityWebRequest request = UnityWebRequest.Get(repoUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        List<string> xmlUrls = ExtractXmlFileUrls(json);

        foreach (string xmlUrl in xmlUrls)
        {
            StartCoroutine(LoadAndParseXml(xmlUrl));
        }
    }

    List<string> ExtractXmlFileUrls(string json)
    {
        List<string> xmlUrls = new List<string>();
        try
        {
            JArray filesArray = JArray.Parse(json); // Парсим JSON как массив

            foreach (JObject file in filesArray)
            {
                string fileName = file["name"]?.ToString();
                if (fileName != null && fileName.EndsWith(".xml"))
                {
                    string downloadUrl = file["download_url"]?.ToString();
                    if (!string.IsNullOrEmpty(downloadUrl))
                    {
                        xmlUrls.Add(downloadUrl);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка парсинга JSON: " + e.Message);
        }

        return xmlUrls;
    }

    IEnumerator LoadAndParseXml(string xmlUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(xmlUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка загрузки XML: " + request.error);
            yield break;
        }

        string xmlData = request.downloadHandler.text;
        CreateEventFromXml(xmlData);
    }

    void CreateEventFromXml(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);
    
        XmlNode eventNode = xmlDoc.SelectSingleNode("event");
        if (eventNode == null) return;
    
        string date = eventNode.SelectSingleNode("date")?.InnerText;
        string title = eventNode.SelectSingleNode("title")?.InnerText;
        string description = eventNode.SelectSingleNode("description")?.InnerText;
    
        // Получаем imageIndex
        int imageIndex = 0; // По умолчанию
        XmlNode imageNode = eventNode.SelectSingleNode("image");
        if (imageNode != null && int.TryParse(imageNode.InnerText, out int parsedIndex))
        {
            imageIndex = parsedIndex;
        }
    
        SpawnEventPrefab(date, title, description, imageIndex);
    }


    void SpawnEventPrefab(string date, string title, string description, int imageIndex)
    {
        GameObject eventObject = Instantiate(eventPrefab, SpawnPoint.transform);
        EventDisplay display = eventObject.GetComponent<EventDisplay>();
        if (display != null)
        {
            display.SetEventData(date, title, description, imageIndex);
        }
    }

}
