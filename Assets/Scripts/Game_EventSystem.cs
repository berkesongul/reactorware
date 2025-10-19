using UnityEngine;
using UnityEngine.UI; // Legacy Text bileşenini kullanmak için bu kütüphane ZORUNLUDUR!
using System.Collections;

public class Game_ : MonoBehaviour
{


    [Header("UI Referansı")]
    // Unity Editor Legacy Text bileşeni
    public Text stabilityText;

    [Header("Ayarlar")]
    // Başlangıç stabilite değeri
    public int baslangicStabilite = 99;

    // Stabilite düşüş aralığı (saniye)
    public float dususAraligi = 2.0f;

    private int mevcutStabilite;

    //tüm butonların gameobject çağırımları
    public GameObject simon_says_panel;
    public GameObject simon_says_button;
    public GameObject game2_button;
    public GameObject game3_button;
    public GameObject game4_button;
    public GameObject game5_button;
    public GameObject game2_panel;
    public GameObject game3_panel;
    public GameObject game4_panel;
    public GameObject game5_panel;

    public int time;
    public bool zaman_ilerlesin_mi = false;

    public bool Simon_bitti_mi = false;
    public bool game_2_bitti_mi= false;
    public bool game_3_bitti_mi = false;
    public bool game_4_bitti_mi = false;
    public bool game_5_bitti_mi = false;

    public void simon_says_baslat()
    {
        Simon_Buton_AcKapa();
        Simon_PaneliAcKapat();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();

    }

    public void simon_says_kapat()
    {
        Simon_PaneliAcKapat();

        Simon_Buton_AcKapa();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();
    }

    public void game2_Ac_Kapa()
    {
        game2_Panel_AcKapa();

        Simon_Buton_AcKapa();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();
        
    }

    public void game3_Ac_Kapa()
    {
        game3_Panel_AcKapa();

        Simon_Buton_AcKapa();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();
    }

    public void game4_Ac_Kapa()
    {
        game4_Panel_AcKapa();

        Simon_Buton_AcKapa();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();
    }

    public void game5_Ac_Kapa()
    {
        game5_Panel_AcKapa();

        Simon_Buton_AcKapa();
        game2_Buton_AcKapa();
        game3_Buton_AcKapa();
        game4_Buton_AcKapa();
        game5_Buton_AcKapa();
    }

    public void Simon_PaneliAcKapat()
    {

        simon_says_panel.SetActive(!simon_says_panel.activeSelf);
    }


    public void Simon_Buton_AcKapa()
    {
        simon_says_button.SetActive(!simon_says_button.activeSelf);
    }

    public void game2_Buton_AcKapa()
    {
        game2_button.SetActive(!game3_button.activeSelf);
    }

    public void game3_Buton_AcKapa()
    {
        game3_button.SetActive(!game3_button.activeSelf);
    }

    public void game4_Buton_AcKapa()
    {
        game4_button.SetActive(!game4_button.activeSelf);
    }

    public void game5_Buton_AcKapa()
    {
        game4_button.SetActive(!game5_button.activeSelf);
    }

    public void game2_Panel_AcKapa()
    {
        game2_panel.SetActive(!game2_panel.activeSelf);
    }

    public void game3_Panel_AcKapa()
    {
        game3_panel.SetActive(!game3_panel.activeSelf);
    }

    public void game4_Panel_AcKapa()
    {
        game4_panel.SetActive(!game4_panel.activeSelf);
    }

    public void game5_Panel_AcKapa()
    {
        game5_panel.SetActive(!game5_panel.activeSelf);
    }

    void Start()
    {
        zaman_ilerlesin_mi = true;

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


    IEnumerator Zaman_sayaci()
    {

        if (zaman_ilerlesin_mi == true)
        {
            yield return new WaitForSeconds(1);
            time = time + 1;

        }
    }

    // Coroutine (Eşzamanlı çalışan fonksiyon) kullanarak stabiliteyi düşür
    IEnumerator StabiliteyiDusurCoroutine()
    {
        if (zaman_ilerlesin_mi == true)
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
    }
}