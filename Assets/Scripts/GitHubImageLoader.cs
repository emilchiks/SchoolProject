using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class GitHubImageLoader : MonoBehaviour
{
    public string imageUrl = "https://raw.githubusercontent.com/emilchiks/Storage-For-School-Project/main/Images/Schedule/screentest.png";
    public RawImage displayImage;
    public RawImage miniImage;

    void Start()
    {
        StartCoroutine(DownloadImage(imageUrl));
    }

    private IEnumerator DownloadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                displayImage.texture = texture;
                miniImage.texture = texture;
            }
            else
            {
                Debug.LogError($"Ошибка загрузки изображения: {request.error}");
            }
        }
    }
}
