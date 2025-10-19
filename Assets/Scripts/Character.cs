using UnityEngine;

public class Character : MonoBehaviour
{
    // Hızı Unity Editor'dan ayarlayabilmeniz için public yaptık.
    public float hareketHizi = 5.0f;

    private Rigidbody2D rb;
    private Animator animator; // Animator referansı

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Animator bileşenini al
        animator = GetComponent<Animator>();

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
        float aci = Mathf.Atan2(bakmaYonu.y, bakmaYonu.x) * Mathf.Rad2Deg;

        // 4. Karakterin rotasyonunu bu açıya ayarla.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, aci));
    }

    // --- WASD ile Hareket Etme Fonksiyonu ---
    void WASDIleHareket()
    {
        // 1. WASD girişlerini al.
        float yatayGiris = Input.GetAxisRaw("Horizontal");
        float dikeyGiris = Input.GetAxisRaw("Vertical");

        // 2. Yön vektörünü oluştur.
        Vector2 hareketYonu = new Vector2(yatayGiris, dikeyGiris).normalized;

        // Rigidbody'nin hızını ayarla.
        rb.linearVelocity = hareketYonu * hareketHizi;

        // <<< YENİ: ANİMATÖRÜ BOOL PARAMETRESİ İLE GÜNCELLEME >>>
        if (animator != null)
        {
            // Hareket Vektörünün Büyüklüğünü (Magnitude) hesapla.
            // 0'dan büyükse karakter hareket ediyor demektir.
            bool hareketEdiyor = hareketYonu.magnitude > 0.1f;

            // Animator'daki "kosuyor_mu" bool parametresini ayarla.
            // true (1) ise Walking, false (0) ise Idle oynayacak.
            animator.SetBool("kosuyor_mu", hareketEdiyor);
        }
        // <<< YENİ KOD SONU >>>
    }
}