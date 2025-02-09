using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;

public class BookRequester : MonoBehaviour
{
    public string apiUrl = "https://api.github.com/repos/emilchiks/Storage-For-School-Project/contents/Books/Library?ref=main";

    // Токен доступа (опционально, если репозиторий приватный)
    [SerializeField] private string authToken = "";

    public GameObject bookPrefab; // Префаб для книги

    public Transform container10thGrade; // Контейнер для 10 класса
    public Transform container11thGrade; // Контейнер для 11 класса
    public Transform containerUnknownGrade; // Контейнер для неизвестного класса

   


    void Start()
    {
        StartCoroutine(GetFileNames(apiUrl));
    }

    private IEnumerator GetFileNames(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Установить заголовок User-Agent
            request.SetRequestHeader("User-Agent", "Unity-Request");
    
            // Добавление заголовка Authorization, если используется токен
            if (!string.IsNullOrEmpty(authToken))
            {
                request.SetRequestHeader("Authorization", "token " + authToken);
            }
    
            // Отправить запрос и дождаться ответа
            yield return request.SendWebRequest();
    
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response Text: " + request.downloadHandler.text);
    
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    JArray contentArray = JArray.Parse(jsonResponse);
    
                    foreach (var item in contentArray)
                    {
                        string fileName = item["name"]?.ToString();
                        string downloadUrl = item["download_url"]?.ToString();
    
                        if (item["type"] != null && item["type"].ToString() == "file")
                        {
                            if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".pdf"))
                            {
                                Debug.Log("PDF File: " + fileName);
                                string className = GetClassName(fileName);
                                if (className == "10")
                                {
                                    SpawnBookPrefab(fileName, downloadUrl, container10thGrade);
                                }
                                else if (className == "11")
                                {
                                    SpawnBookPrefab(fileName, downloadUrl, container11thGrade);
                                }
                                else
                                {
                                    SpawnBookPrefab(fileName, downloadUrl, containerUnknownGrade);
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"Skipping non-PDF file: {fileName}");
                            }
                        }
                        else if (item["type"] != null && item["type"].ToString() == "dir")
                        {
                            // Если это каталог, рекурсивно обрабатываем его
                            string newUrl = item["url"]?.ToString();
                            StartCoroutine(GetFileNames(newUrl)); // Рекурсивный вызов для обхода подкаталогов
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error parsing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error fetching files: " + request.error);
            }
        }
    }

    

    
    private string GetClassName(string fileName)
    {
        try
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string[] parts = baseName.Split('_');
            return parts.Length > 0 ? parts[0] : "";
        }
        catch
        {
            Debug.LogError($"Error parsing class from file name {fileName}");
            return "";
        }
    }

    private void SpawnBookPrefab(string fileName, string downloadUrl, Transform container)
    {
        GameObject bookInstance = Instantiate(bookPrefab, container);

        Transform fileNameTextTransform = bookInstance.transform.Find("FileNameText");
        Transform yearTextTransform = bookInstance.transform.Find("YearText");
        Transform downloadButtonTransform = bookInstance.transform.Find("DownloadButton");
        Transform coverImageTransform = bookInstance.transform.Find("CoverImage");

        if (fileNameTextTransform == null || yearTextTransform == null || downloadButtonTransform == null || coverImageTransform == null)
        {
            Debug.LogError("Prefab is missing required components.");
            return;
        }

        TextMeshProUGUI fileNameText = fileNameTextTransform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI yearText = yearTextTransform.GetComponent<TextMeshProUGUI>();
        Button downloadButton = downloadButtonTransform.GetComponent<Button>();
        Image coverImage = coverImageTransform.GetComponent<Image>();

        string bookName = "";
        string year = "";
        string index = "";

        try
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string[] parts = baseName.Split('_');

            if (parts.Length == 4)
            {
                bookName = parts[1];
                year = parts[2];
                index = parts[3];
            }
            else
            {
                Debug.LogWarning($"Unexpected file name format: {fileName}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing file name {fileName}: {ex.Message}");
        }

        if (fileNameText != null)
        {
            fileNameText.text = bookName;
        }

        if (yearText != null)
        {
            yearText.text = year;
        }

        if (downloadButton != null)
        {
            downloadButton.onClick.AddListener(() => StartCoroutine(DownloadFile(downloadUrl, fileName)));
        }

        Sprite coverSprite = Resources.Load<Sprite>("Covers/" + index);
        if (coverSprite != null)
        {
            coverImage.sprite = coverSprite;
        }
        else
        {
            Debug.LogWarning("Cover not found: " + index);
        }
    }

    private IEnumerator DownloadFile(string url, string fileName)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string filePath = Path.Combine(Application.persistentDataPath, fileName);
                File.WriteAllBytes(filePath, request.downloadHandler.data);
                Debug.Log("File saved to: " + filePath);
            }
            else
            {
                Debug.LogError($"Error downloading file {fileName}: {request.error}");
            }
        }
    }
}
    
    /*private void OpenFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
    
        if (File.Exists(filePath))
        {
            Application.OpenURL("file://" + filePath);
            Debug.Log("Opening file: " + filePath);
        }
        else
        {
            Debug.LogError("File does not exist: " + filePath);
        }
    }*/