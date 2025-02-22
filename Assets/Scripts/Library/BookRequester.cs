using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BookRequester : MonoBehaviour
{
    public string apiUrl = "https://api.github.com/repos/emilchiks/Storage-For-School-Project/contents/Books/Library?ref=main";

    // Токен доступа (опционально, если репозиторий приватный)
    [SerializeField] private string authToken = "";

    public GameObject bookPrefab; // Префаб для книги

    public Transform container1Grade;
    public Transform container2Grade;
    public Transform container3Grade;
    public Transform container4Grade;
    public Transform container5Grade;
    public Transform container6Grade;
    public Transform container7Grade;
    public Transform container8Grade;
    public Transform container9Grade;

    public Transform container10thGrade; // Контейнер для 10 класса
    public Transform container11thGrade; // Контейнер для 11 класса
    public Transform containerUnknownGrade; // Контейнер для неизвестного класса


    public GameObject ErrorSpawner;
    public Transform ErrorSpawnPlace;
    public GameObject ErrorObject;

    public UIColorSwitch themeScript;
    public string category;
    

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
                                Transform targetContainer = className switch
                                {
                                    "1" => container1Grade,
                                    "2" => container2Grade,
                                    "3" => container3Grade,
                                    "4" => container4Grade,
                                    "5" => container5Grade,
                                    "6" => container6Grade,
                                    "7" => container7Grade,
                                    "8" => container8Grade,
                                    "9" => container9Grade,
                                    "10" => container10thGrade,
                                    "11" => container11thGrade,
                                    _ => containerUnknownGrade
                                };
                                SpawnBookPrefab(fileName, downloadUrl, targetContainer);
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

                    ErrorSpawner.SetActive(true);
                    GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
                    Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
                    TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
                    panelErrorText.text = "Error parsing JSON: " + ex.Message;
                }
            }
            else
            {
                Debug.LogError("Error fetching files: " + request.error);

                ErrorSpawner.SetActive(true);
                GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
                Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
                TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
                panelErrorText.text = "Error fetching files: " + request.error;
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

            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = $"Error parsing class from file name {fileName}";
        }
    }

    private void SpawnBookPrefab(string fileName, string downloadUrl, Transform container)
    {
        GameObject bookInstance = Instantiate(bookPrefab, container);

        Transform fileNameTextTransform = bookInstance.transform.Find("FileNameText");
        Transform yearTextTransform = bookInstance.transform.Find("YearText");
        Transform downloadButtonTransform = bookInstance.transform.Find("DownloadButton");
        Transform coverImageTransform = bookInstance.transform.Find("CoverImage");
        Transform plateImageTransform = bookInstance.transform.Find("Plate");
        Transform downloadIconTransform = bookInstance.transform.Find("DownloadButtonIcon");
        Transform openButtonTransform = bookInstance.transform.Find("OpenButton");
        Transform openIconTransform = bookInstance.transform.Find("OpenButtonIcon");

        if (fileNameTextTransform == null || yearTextTransform == null || downloadButtonTransform == null || coverImageTransform == null)
        {
            Debug.LogError("Prefab is missing required components.");
            return;

            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = "Prefab is missing required components.";
        }

        TextMeshProUGUI fileNameText = fileNameTextTransform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI yearText = yearTextTransform.GetComponent<TextMeshProUGUI>();
        Button downloadButton = downloadButtonTransform.GetComponent<Button>();
        Image coverImage = coverImageTransform.GetComponent<Image>();
        Image plateImage = plateImageTransform.GetComponent<Image>();
        Image downloadIcon = downloadIconTransform.GetComponent<Image>();
        Button openButton = openButtonTransform.GetComponent<Button>();
        Image openIcon = openIconTransform.GetComponent<Image>();

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

            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = $"Error parsing file name {fileName}: {ex.Message}";
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
            downloadButton.onClick.AddListener(() => Application.OpenURL(downloadUrl));
        }
        if (openButton != null)
        {
            // Формируем URL для просмотра файла на GitHub
            string githubViewUrl = downloadUrl.Replace("https://raw.githubusercontent.com/", "https://github.com/")
                                              .Replace("/main/", "/blob/main/");
            
            // Назначаем обработчик кнопки
            openButton.onClick.AddListener(() => Application.OpenURL(githubViewUrl));
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

        if(fileNameText != null && yearText != null && plateImage != null && downloadIcon != null && coverImage != null && openIcon != null)
        {
            themeScript.AddImageToList(plateImage, "Contrast");
            themeScript.AddImageToList(downloadIcon, "HighContrast");
            themeScript.AddImageToList(openIcon, "HighContrast");
            themeScript.AddImageToList(coverImage, "HighContrast");
            themeScript.AddTextToList(fileNameText);
            themeScript.AddTextToList(yearText);

            themeScript.LoadConfig();
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

                ErrorSpawner.SetActive(true);
                GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
                Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
                TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
                panelErrorText.text = $"Error downloading file {fileName}: {request.error}";
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
    }
    */