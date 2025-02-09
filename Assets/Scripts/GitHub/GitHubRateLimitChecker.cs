using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GitHubRateLimitChecker : MonoBehaviour
{
    // Вставьте свой персональный токен доступа GitHub
    private string githubToken = "your_github_token_here";

    void Start()
    {
        StartCoroutine(CheckRateLimit());
    }

    IEnumerator CheckRateLimit()
    {
        string url = "https://api.github.com/rate_limit";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Устанавливаем заголовок для аутентификации
            request.SetRequestHeader("Authorization", $"Bearer {githubToken}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Ошибка: {request.error}");
            }
            else
            {
                // Парсим ответ
                string jsonResponse = request.downloadHandler.text;
                RateLimitResponse rateLimit = JsonUtility.FromJson<RateLimitResponse>(jsonResponse);

                if (rateLimit.rate.remaining > 0)
                {
                    Debug.Log($"Оставшиеся запросы: {rateLimit.rate.remaining}");
                }
                else
                {
                    Debug.LogWarning("Лимит запросов исчерпан! Попробуйте позже.");
                }
            }
        }
    }

    [System.Serializable]
    public class RateLimit
    {
        public int limit;
        public int remaining;
        public int reset;
    }

    [System.Serializable]
    public class RateLimitResponse
    {
        public RateLimit rate;
    }
}
