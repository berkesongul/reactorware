
using UnityEngine;
using System.Collections; // Coroutine için
using UnityEngine.UI; // Text (Legacy) bileşenini kullanmak için

public class End_text_anim : MonoBehaviour
{
    [Header("Animasyon Ayarları")]
    [Tooltip("Text'in ulaşacağı minimum ölçek boyutu (örneğin 0.9).")]
    [Range(0.1f, 10.0f)]
    public float minScale = 0.9f;

    [Tooltip("Text'in ulaşacağı maksimum ölçek boyutu (örneğin 1.1).")]
    [Range(0.1f, 10.0f)]
    public float maxScale = 1.1f;

    [Tooltip("Bir büyüme veya küçülme döngüsünün toplam süresi (saniye).")]
    public float animationDuration = 1.0f;

    private Text targetText; // Text (Legacy) bileşeni
    private RectTransform rectTransform; // Ölçeklendirmek için RectTransform

    void Start()
    {
        targetText = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("TextScaler: Bu objede RectTransform bulunamadı: " + gameObject.name);
            enabled = false; // Script'i devre dışı bırak
            return;
        }

        // Animasyonu başlat
        StartCoroutine(ScaleText());
    }

    IEnumerator ScaleText()
    {
        while (true) // Sonsuz döngü
        {
            float timer = 0f;

            // 1. MinScale'den MaxScale'e Büyü (Fade In)
            while (timer < animationDuration / 2f)
            {
                timer += Time.deltaTime;
                float currentScale = Mathf.Lerp(minScale, maxScale, timer / (animationDuration / 2f));
                rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
                yield return null; // Bir sonraki frame'e kadar bekle
            }

            timer = 0f; // Zamanlayıcıyı sıfırla

            // 2. MaxScale'den MinScale'e Küçül (Fade Out)
            while (timer < animationDuration / 2f)
            {
                timer += Time.deltaTime;
                float currentScale = Mathf.Lerp(maxScale, minScale, timer / (animationDuration / 2f));
                rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
                yield return null; // Bir sonraki frame'e kadar bekle
            }
        }
    }
}