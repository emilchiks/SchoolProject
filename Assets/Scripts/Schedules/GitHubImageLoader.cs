using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GitHubImageLoader : MonoBehaviour
{
    public string imageUrl = "https://raw.githubusercontent.com/emilchiks/Storage-For-School-Project/main/Images/Schedule/screentest.png";
    public RawImage displayImage;
    public RawImage miniImage;

    public GameObject ErrorSpawner;
    public Transform ErrorSpawnPlace;
    public GameObject ErrorObject;

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

                ErrorSpawner.SetActive(true);

                GameObject errorInstance = Instantiate(ErrorObject, ErrorSpawnPlace);

                Transform errorTextTransform = errorInstance.transform.Find("ErrorText");
                TextMeshProUGUI panelErrorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
                panelErrorText.text = $"Ошибка загрузки изображения: {request.error}";
            }
        }
    }
}
