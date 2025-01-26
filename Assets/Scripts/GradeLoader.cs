using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GradeLoader : MonoBehaviour
{
    // URL к XML-файлу на GitHub
    private string xmlUrl = "https://raw.githubusercontent.com/emilchiks/Storage-For-School-Project/main/Books/Library.xml";

    // Список кнопок для классов
    [SerializeField] private List<GradeButton> gradeButtons;

    // Стартовая функция Unity
    private async void Start()
    {
        try
        {
            // Загружаем XML
            string xmlContent = await DownloadXmlAsync(xmlUrl);

            // Разбираем XML и получаем список grades
            List<string> grades = ParseGradesFromXml(xmlContent);

            // Выводим классы в консоль
            foreach (string gradeName in grades)
            {
                Debug.Log("Новый класс:" + gradeName);
            }

            // Активируем кнопки для найденных grades
            ActivateGradeButtons(grades);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка: {ex.Message}");
        }
    }

    // Асинхронная загрузка XML-файла
    private async Task<string> DownloadXmlAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

    // Парсинг XML и получение списка grades
    private List<string> ParseGradesFromXml(string xmlContent)
    {
        List<string> gradeGroups = new List<string>();
        XDocument xmlDoc = XDocument.Parse(xmlContent);

        // Ищем все элементы GradeGroup
        foreach (XElement gradeGroup in xmlDoc.Descendants("ClassGroup"))
        {
            XAttribute nameAttr = gradeGroup.Attribute("name");
            if (nameAttr != null)
            {
                gradeGroups.Add(nameAttr.Value);
            }
        }

        return gradeGroups;
    }

    // Активация кнопок для найденных grades
    private void ActivateGradeButtons(List<string> grades)
    {
        foreach (GradeButton gradeButton in gradeButtons)
        {
            if (grades.Contains(gradeButton.gradeName))
            {
                gradeButton.button.interactable = true; // Включить кнопку
            }
            else
            {
                gradeButton.button.interactable = false; // Отключить кнопку
            }
        }
    }

    [Serializable]
    public class GradeButton
    {
        public string gradeName; // Название класса (например, "9", "10")
        public Button button; // Ссылка на кнопку
    }
}
