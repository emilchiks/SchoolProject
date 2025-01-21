using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

            foreach (var commit in commits)
            {
                if (!HasCommitBeenNotified(commit.sha))
                {
                    NotifyUser(commit);
                    MarkCommitAsNotified(commit.sha);
                }
            }
        }
        else
        {
            Debug.LogError("Ошибка проверки изменений: " + request.error);
        }
    }

    private void NotifyUser(Commit commit)
    {
        Debug.Log($"Новый коммит от {commit.commit.author.name}: {commit.commit.message}");
        // Здесь можно добавить отображение уведомления в UI
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
