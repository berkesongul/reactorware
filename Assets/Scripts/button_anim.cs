using UnityEngine;
using System.Collections; // Coroutine için
using UnityEngine.UI; // Buton bileşeni için (isteğe bağlı, sadece tıklama dinleyicisi için)

public class Button : MonoBehaviour
{
    [Header("Gerekli Referanslar")]
    // Butonun CanvasGroup bileşeni. Editor'dan atanmalı.
    public CanvasGroup targetCanvasGroup;

    [Header("Animasyon Ayarları")]
    [Tooltip("Animasyonun döngü süresi (bir kez açılıp kapanması).")]
    public float donguSuresi = 2.0f;

    [Tooltip("Minimum opaklık seviyesi (0.0 tamamen şeffaf, 1.0 tamamen opak).")]
    [Range(0.0f, 1.0f)]
    public float minAlpha = 0.3f;

    [Tooltip("Maksimum opaklık seviyesi (0.0 tamamen şeffaf, 1.0 tamamen opak).")]
    [Range(0.0f, 1.0f)]
    public float maxAlpha = 1.0f;

    private bool yanipSonuyor = false;

    void Start()
    {
        // CanvasGroup atanmamışsa hata mesajı ver ve çalışmayı durdur.
        if (targetCanvasGroup == null)
        {
            targetCanvasGroup = GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
            {
                Debug.LogError("ButtonFlicker: Bu objede CanvasGroup bulunamadı veya atanmadı: " + gameObject.name);
                enabled = false; // Script'i devre dışı bırak
                return;
            }
        }

        // Animasyonu hemen başlat
        StartFlicker();
    }

    // Yanıp sönme animasyonunu başlatır
    public void StartFlicker()
    {
        if (!yanipSonuyor)
        {
            yanipSonuyor = true;
            StartCoroutine(FlickerCoroutine());
        }
    }

    // Yanıp sönme animasyonunu durdurur ve opaklığı maxAlpha'ya getirir.
    public void StopFlicker()
    {
        if (yanipSonuyor)
        {
            yanipSonuyor = false;
            StopAllCoroutines(); // Tüm coroutine'leri durdur
            targetCanvasGroup.alpha = maxAlpha; // Tamamen görünür yap
        }
    }

    // Opaklığı açıp kapatan Coroutine
    IEnumerator FlickerCoroutine()
    {
        while (yanipSonuyor)
        {
            float timer = 0f;

            // Opaklığı minimumdan maksimuma artır (fade in)
            while (timer < donguSuresi / 2f)
            {
                timer += Time.deltaTime;
                targetCanvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, timer / (donguSuresi / 2f));
                yield return null; // Bir sonraki frame'e kadar bekle
            }

            timer = 0f; // Zamanlayıcıyı sıfırla

            // Opaklığı maksimumdan minimuma düşür (fade out)
            while (timer < donguSuresi / 2f)
            {
                timer += Time.deltaTime;
                targetCanvasGroup.alpha = Mathf.Lerp(maxAlpha, minAlpha, timer / (donguSuresi / 2f));
                yield return null; // Bir sonraki frame'e kadar bekle
            }

            // Döngü tamamlandığında bir sonraki tekrara geçmeden önce beklemeye gerek yok, 
            // direk yeniden başlayacaktır.
        }
    }
}