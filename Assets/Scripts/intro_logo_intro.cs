using UnityEngine;

public class intro_logo_intro : MonoBehaviour
{
    [Header("Animasyon Ayarları")]
    [Tooltip("Animasyonun hızı. Yüksek değer daha hızlı atış demektir.")]
    public float pulseSpeed = 2.0f;

    [Tooltip("Maksimum büyüme boyutu. (Örn: 0.1, %10 daha büyük demektir)")]
    public float pulseMagnitude = 0.1f;

    private Vector3 initialScale; // Logonun başlangıç ölçeğini tutar.

    void Start()
    {
        // Script'in eklendiği objenin başlangıç ölçeğini kaydet
        initialScale = transform.localScale;
    }

    void Update()
    {
        // 1. Sinüs dalgası ile yumuşak salınım değeri hesapla
        // Mathf.Sin(Time.time * pulseSpeed) = -1 ile +1 arasında yumuşakça değişir.
        float sinValue = Mathf.Sin(Time.time * pulseSpeed);

        // 2. Bu değeri 0 ile 1 arasına taşı (Offset ve Bölme)
        // (sinValue + 1) / 2 = 0 ile 1 arasında yumuşak bir salınım verir.
        float normalizedPulse = (sinValue + 1f) / 2f;

        // 3. Normalized değeri kullanarak toplam ölçek büyütme miktarını hesapla
        // Bu, pulseMagnitude * 0 (küçük) ile pulseMagnitude * 1 (büyük) arasında değişir.
        float scaleAmount = 1f + (normalizedPulse * pulseMagnitude);

        // 4. Yeni ölçeği uygula
        // Başlangıç ölçeğini hesaplanan miktarla çarparak büyütme/küçültme sağlar.
        transform.localScale = initialScale * scaleAmount;
    }
}