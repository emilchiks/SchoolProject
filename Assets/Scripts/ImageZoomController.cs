using UnityEngine;
using UnityEngine.UI;

public class ImageZoomController : MonoBehaviour
{
    public GameObject imagePanel; // Панель с картинкой
    public RawImage image; // Компонент Image с картинкой
    public Button closeButton; // Кнопка для закрытия панели
    public float zoomSpeed = 0.1f; // Скорость масштабирования

    private RectTransform imageRect;
    private Vector2 initialTouchDistance; // Расстояние между пальцами
    private Vector3 initialScale; // Начальный масштаб изображения
    private bool isPinching = false; // Флаг для отслеживания жеста Pinch

    private void Start()
    {
        imagePanel.SetActive(false); // Скрываем панель с картинкой по умолчанию
        imageRect = image.GetComponent<RectTransform>();
        initialScale = imageRect.localScale;

        // Привязываем функцию закрытия к кнопке
        closeButton.onClick.AddListener(() => imagePanel.SetActive(false));
    }

    public void OpenImage()
    {
        imagePanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.touchCount == 2) // Проверяем, используется ли жест двумя пальцами
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (!isPinching)
            {
                // Сохраняем начальное расстояние между пальцами
                initialTouchDistance = touch1.position - touch2.position;
                initialScale = imageRect.localScale;
                isPinching = true;
            }
            else
            {
                // Текущее расстояние между пальцами
                Vector2 currentTouchDistance = touch1.position - touch2.position;
                float scaleFactor = currentTouchDistance.magnitude / initialTouchDistance.magnitude;

                // Масштабируем изображение
                imageRect.localScale = initialScale * scaleFactor;

                // Ограничиваем масштабирование
                imageRect.localScale = new Vector3(
                    Mathf.Clamp(imageRect.localScale.x, 0.5f, 3f),
                    Mathf.Clamp(imageRect.localScale.y, 0.5f, 3f),
                    1
                );
            }
        }
        else
        {
            isPinching = false; // Сбрасываем жест, если больше нет двух касаний
        }

        // Масштабирование колесиком мыши (для ПК)
        if (Input.mouseScrollDelta.y != 0)
        {
            float scaleFactor = 1 + Input.mouseScrollDelta.y * zoomSpeed;

            // Масштабируем изображение
            imageRect.localScale *= scaleFactor;

            // Ограничиваем масштабирование
            imageRect.localScale = new Vector3(
                Mathf.Clamp(imageRect.localScale.x, 0.5f, 3f),
                Mathf.Clamp(imageRect.localScale.y, 0.5f, 3f),
                1
            );
        }
    }
}
