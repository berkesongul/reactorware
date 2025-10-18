using UnityEngine;
using UnityEngine.UI; // Legacy Text bileşenini kullanmak için bu kütüphane ZORUNLUDUR!
using System.Collections;

public class Game_ : MonoBehaviour
{

    [Header("UI Referansı")]
    // Unity Editor'dan atanacak Legacy Text bileşeni
    public Text stabilityText;

    [Header("Ayarlar")]
    // Başlangıç stabilite değeri
    public int baslangicStabilite = 99;

    // Stabilite düşüş aralığı (saniye)
    public float dususAraligi = 2.0f;

    private int mevcutStabilite;


    public GameObject simon_says_panel;
    public GameObject simon_says_button;


    void Start()
    {
        // 1. Mevcut stabiliteyi başlangıç değerine ayarla
        mevcutStabilite = baslangicStabilite;

        // UI'ı başlangıç değeriyle hemen güncelle
        GuncelStabiliteUI();

        // 2. Sayacı başlat
        StartCoroutine(StabiliteyiDusurCoroutine());
    }

    // Stabilite değerini güncelleyen fonksiyon
    void GuncelStabiliteUI()
    {
        // Text bileşenini kontrol et
        if (stabilityText != null)
        {
            // UI metnini yeni değerle güncelle
            stabilityText.text = mevcutStabilite.ToString();
        }
    }

    // Coroutine (Eşzamanlı çalışan fonksiyon) kullanarak stabiliteyi düşür
    IEnumerator StabiliteyiDusurCoroutine()
    {
        // Sonsuz döngü: Stabilite 0'ın üstünde olduğu sürece düşmeye devam et
        while (mevcutStabilite > 0)
        {
            // Belirtilen saniye kadar bekle
            yield return new WaitForSeconds(dususAraligi);

            // 1 birim düşür
            mevcutStabilite--;

            // UI'ı güncelle
            GuncelStabiliteUI();

            // Eğer stabilite sıfır olduysa
            if (mevcutStabilite <= 0)
            {
                mevcutStabilite = 0; // Negatife düşmesini engelle
                // OYUN BİTTİ veya DİĞER AKSİYONLAR buraya eklenebilir.
                Debug.Log("Stabilite sıfıra ulaştı! Oyun bitti (veya karakter öldü).");
                break; // Döngüyü sonlandır
            }
        }
    }

    public void Simon_PaneliAcKapat()
    {

        simon_says_panel.SetActive(!simon_says_panel.activeSelf);
    }
    

    public void Simon_Buton_AcKapa()
    {
        simon_says_button.SetActive(!simon_says_button.activeSelf);
    }
}