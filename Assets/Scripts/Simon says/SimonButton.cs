using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimonButton : MonoBehaviour
{
    // GameManager'a referans (GameManager Start'ta atayacak)
    [HideInInspector] public SimonGameManager gameManager;

    // Butonun rengini/kimliğini temsil eden benzersiz ID (0'dan 8'e kadar)
    [HideInInspector] public int buttonID;

    // Yanıp sönme görseli için Image bileşeni
    private Image buttonImage;

    // Ses için AudioSource (Buton tıklamasıyla çalacak sesi tutar)
    private AudioSource buttonAudio;

    [Header("Görsel ve Ses Ayarları")]
    public Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Soluk renk
    public Color highlightColor = Color.white; // Parlak renk

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonAudio = GetComponent<AudioSource>();

        // Başlangıçta butonu varsayılan renge ayarla
        if (buttonImage != null)
        {
            buttonImage.color = defaultColor;
        }
    }

    // GameManager tarafından çağrılır: Butonun yanıp sönmesini sağlar
    public IEnumerator Flash()
    {
        // 1. Parlak renk ve sesi aç
        if (buttonImage != null) buttonImage.color = highlightColor;
        if (buttonAudio != null) buttonAudio.Play();

        // 2. Kısa bir süre bekle (Yanık kalma süresi)
        yield return new WaitForSeconds(0.5f);

        // 3. Varsayılan renge geri dön
        if (buttonImage != null) buttonImage.color = defaultColor;

        // 4. Kısa bir bekleme süresi daha (sıradaki buton flaş etmeden önce)
        yield return new WaitForSeconds(0.1f);
    }

    // **********************************************
    // UNITY EDITOR'DAKİ BUTONUN ONCLICK EVENT'İNE BU FONKSİYON ATANMALIDIR!
    // **********************************************
    public void buton_click()
    {
        // Yalnızca oyun sırası oyuncudayken tıklama kabul et
        if (gameManager != null && gameManager.isPlayerTurn && !gameManager.IsFlashing)
        {
            // Tıklama geri bildirimi için flaş başlat (Bu, flaşın hemen başlamasını sağlar)
            StartCoroutine(Flash());

            // GameManager'a seçimi bildir
            gameManager.PlayerChose(buttonID);
        }
    }
}