using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class Commit
{
    public string sha;
    public CommitDetails commit;
}

[System.Serializable]
public class CommitDetails
{
    public CommitAuthor author;
    public string message;
}

[System.Serializable]
public class CommitAuthor
{
    public string name;
    public string email;
    public string date;
}

public class GitHubChangeChecker : MonoBehaviour
{
    public string apiUrl = "https://api.github.com/repos/emilchiks/Storage-For-School-Project/commits";

    // Интервал проверки (в секундах)
    public float checkInterval = 60f;

    public GameObject ErrorSpawner;
    public Transform ErrorSpawnPlace;
    public GameObject ErrorObject;

    public Transform MessageSpawnPlace;
    public GameObject MessageObject;

    public GameObject MessageIcon;

    void Start()
    {
        StartCoroutine(CheckForChanges());
        InvokeRepeating("StartChecking", checkInterval, checkInterval);
    }

    private void StartChecking()
    {
        StartCoroutine(CheckForChanges());
    }

    private IEnumerator CheckForChanges()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();
    
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
    
            // Десериализация массива JSON
            Commit[] commits = JsonUtility.FromJson<Wrapper>($"{{\"items\":{jsonResponse}}}").items;
    
            bool hasNewCommits = false;
    
            foreach (var commit in commits)
            {
                NotifyUser(commit);
    
                if (!HasCommitBeenNotified(commit.sha))
                {
                    hasNewCommits = true;
                    MarkCommitAsNotified(commit.sha);
                }
            }
    
            // Включаем иконку, если есть новые коммиты
            MessageIcon.SetActive(hasNewCommits);
        }
        else
        {
            Debug.LogError("Ошибка проверки изменений: " + request.error);
    
            ErrorSpawner.SetActive(true);
            GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);
            Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
            TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
            panelErrorText.text = "Ошибка проверки изменений: " + request.error;
        }
    }

    private void NotifyUser(Commit commit)
    {
        // Получаем полное сообщение коммита
        string fullMessage = commit.commit.message;
    
        // Разделяем заголовок и описание (если есть)
        string[] messageParts = fullMessage.Split(new[] { "\n\n" }, System.StringSplitOptions.None);
    
        // Если нет описания, пропускаем этот коммит
        if (messageParts.Length < 2) return;
    
        string description = messageParts[1]; // Берём только описание
    
        // Проверяем, заканчивается ли описание на ']'
        if (!description.EndsWith("]")) return;
    
        Debug.Log($"Описание изменений: {description}");
        
        GameObject messageInstance = Instantiate(MessageObject, MessageSpawnPlace);
        Transform messageTextTransform = messageInstance.transform.Find("MessageText");
        TextMeshProUGUI panelMessageText = messageTextTransform.GetComponent<TextMeshProUGUI>();
        //panelMessageText.text = $"Изменения: \n{description}";
        panelMessageText.text = $"<b>Изменения:</b> \n{description}";
    
        MessageIcon.SetActive(true);
    }




    private bool HasCommitBeenNotified(string sha)
    {
        // Проверка, хранится ли SHA в PlayerPrefs
        return PlayerPrefs.HasKey(sha);
    }

    private void MarkCommitAsNotified(string sha)
    {
        // Сохраняем SHA как ключ в PlayerPrefs
        PlayerPrefs.SetInt(sha, 1);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class Wrapper
    {
        public Commit[] items;
    }
}
