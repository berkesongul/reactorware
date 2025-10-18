using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    // Editörden atanacak hedef (karakter) objesi.
    public Transform target;

    [Tooltip("Kameranın hedeften ne kadar geride duracağı (Z ekseni). Negatif olmalı.")]
    public float mesafeZ = -10f;

    [Header("Takip Ayarları")]
    [Tooltip("Takip hızı. 0'a yakın yavaş, yüksek hızlı takip.")]
    [Range(0.01f, 1f)]
    public float yumusaklikFaktoru = 0.125f;

    [Tooltip("Karakterin üstünden veya altından ne kadar offset olsun.")]
    public Vector3 offset = Vector3.zero;

    private Vector3 hedefPozisyon;

    // Karakterin hareketinden sonra çalışır, bu yüzden daha pürüzsüzdür.
    void LateUpdate()
    {
        // Hedef atanmışsa devam et
        if (target == null)
        {
            Debug.LogError("SmoothCameraFollow: Takip edilecek hedef (Target) atanmamış!");
            return;
        }

        // 1. Kameranın ulaşmak istediği nihai pozisyonu hesapla (X ve Y'yi hedeften al, Z'yi sabit mesafeden al)
        hedefPozisyon = target.position + offset;
        hedefPozisyon.z = mesafeZ; // Kamera Z mesafesini korur

        // 2. Mevcut pozisyondan hedef pozisyona yumuşak geçiş yap
        // Vector3.Lerp: İki nokta arasında 'yumuşaklıkFaktoru' kadar interpole eder.
        // Bu, smooth (pürüzsüz) takip etkisini yaratır.
        transform.position = Vector3.Lerp(transform.position, hedefPozisyon, yumusaklikFaktoru);

        // Not: Eğer 3D bir oyun yapıyorsanız ve kameranın karaktere bakmasını istiyorsanız,
        // buraya Quaternion.Slerp ile yumuşak bir rotation kodu ekleyebilirsiniz.
    }
}