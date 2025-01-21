using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;

public class BookRequester : MonoBehaviour
{
    public string apiUrl = "https://api.github.com/repos/emilchiks/Storage-For-School-Project/contents/Books";

    // Токен доступа (опционально, если репозиторий приватный)
    [SerializeField] private string authToken = "";


    public Transform contentParent; // Родитель для спавна (например, ScrollView Content)
    public GameObject bookPrefab; // Префаб для книги



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
                        // Проверяем, содержит ли объект поле "type"
                        if (item["type"] != null && item["type"].ToString() == "file")
                        {
                            string fileName = item["name"]?.ToString();
                            string downloadUrl = item["download_url"]?.ToString();
                
                            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(downloadUrl))
                            {
                                Debug.Log("File: " + fileName);
                
                                // Спавним префаб
                                SpawnBookPrefab(fileName, downloadUrl);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Skipping item without type or not a file.");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error parsing JSON: " + ex.Message);
                    Debug.Log("Full API Response: " + request.downloadHandler.text);

                }
            }
            else
            {
                Debug.LogError("Error fetching files: " + request.error);
                Debug.Log("Response Text: " + request.downloadHandler.text);
            }
        }
    }

    private void SpawnBookPrefab(string fileName, string downloadUrl)
    {
        GameObject bookInstance = Instantiate(bookPrefab, contentParent);
    
        // Найти элементы
        Transform fileNameTextTransform = bookInstance.transform.Find("FileNameText");
        Transform downloadButtonTransform = bookInstance.transform.Find("DownloadButton");
        Transform openFileButtonTransform = bookInstance.transform.Find("OpenFileButton");
    
        if (fileNameTextTransform == null || downloadButtonTransform == null || openFileButtonTransform == null)
        {
            Debug.LogError("Prefab is missing required components (FileNameText, DownloadButton, or OpenFileButton).");
            return;
        }
    
        Text fileNameText = fileNameTextTransform.GetComponent<Text>();
        Button downloadButton = downloadButtonTransform.GetComponent<Button>();
        Button openFileButton = openFileButtonTransform.GetComponent<Button>();
    
        if (fileNameText != null)
        {
            fileNameText.text = fileName;
        }
    
        if (downloadButton != null)
        {
            downloadButton.onClick.AddListener(() => StartCoroutine(DownloadFile(downloadUrl, fileName)));
        }
    
        if (openFileButton != null)
        {
            openFileButton.onClick.AddListener(() => OpenFile(fileName));
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
    
    private void OpenFile(string fileName)
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
    }
}