using UnityEngine;

public class Character : MonoBehaviour
{
    // Hızı Unity Editor'dan ayarlayabilmeniz için public yaptık.
    public float hareketHizi = 5.0f;

    private Rigidbody2D rb;

    void Start()
    {
        // Karakter üzerindeki Rigidbody2D bileşenini al. 
        // Hareket için Rigidbody kullanmak en iyisidir.
        rb = GetComponent<Rigidbody2D>();

        // Yerçekimini devre dışı bırakın, aksi takdirde karakter aşağı düşer.
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        // 1. Karakteri Sürekli Fareye Doğru Döndürme
        FareyeBak();
    }

    void FixedUpdate()
    {
        // 2. WASD ile Hareket Etme (Fizik işlemleri FixedUpdate'te yapılmalıdır)
        WASDIleHareket();
    }

    // --- Karakteri Fareye Doğru Döndürme Fonksiyonu ---
    void FareyeBak()
    {
        // 1. Fare pozisyonunu ekran koordinatlarından dünya koordinatlarına dönüştür.
        Vector3 farePozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. Karakterden fareye doğru olan yönü hesapla.
        Vector2 bakmaYonu = new Vector2(
            farePozisyonu.x - transform.position.x,
            farePozisyonu.y - transform.position.y
        );

        // 3. Hesaplanan yöne göre açıyı bul.
        // Mathf.Atan2 radyan cinsinden değer döndürür, Dereceye çeviriyoruz.
        float aci = Mathf.Atan2(bakmaYonu.y, bakmaYonu.x) * Mathf.Rad2Deg;

        // 4. Karakterin rotasyonunu bu açıya ayarla.
        // Z ekseni etrafında döndürüyoruz, 2D için bu geçerlidir.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, aci));
    }

    // --- WASD ile Hareket Etme Fonksiyonu ---
    void WASDIleHareket()
    {
        // 1. WASD girişlerini al. (Unity'nin varsayılan "Horizontal" ve "Vertical" ayarları WASD ve Ok tuşlarını içerir)
        float yatayGiris = Input.GetAxisRaw("Horizontal"); // A veya D
        float dikeyGiris = Input.GetAxisRaw("Vertical");   // W veya S

        // 2. Yön vektörünü oluştur.
        Vector2 hareketYonu = new Vector2(yatayGiris, dikeyGiris).normalized;

        // 3. Rigidbody'nin hızını ayarla.
        // .normalized() çapraz hareketlerde daha hızlı gitmeyi engeller.
        // Time.fixedDeltaTime, fizik güncellemeleri arasındaki sabit zaman dilimidir.
        rb.linearVelocity = hareketYonu * hareketHizi;
    }
}