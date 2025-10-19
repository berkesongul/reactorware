using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq; // LINQ kullanımı için

public class puzzle_grid : MonoBehaviour
{
    [Header("Puzzle Ayarları")]
    [Tooltip("9 parçayı (8 Kutu + 1 Boşluk) tutan liste.")]
    public List<Button> allTiles = new List<Button>(); // Editor'dan atanacak 8 Kutu + 1 Boşluk (Button olarak)

    [Tooltip("Kazanma sırası (0'dan 8'e). Örneğin: 0, 1, 2, 3, 4, 5, 6, 7, 8")]
    public int[] winningOrder = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    [Header("UI & Geri Bildirim")]
    public GameObject puzzlePanel;
    public Text messageText;
    public Button closeButton;

    [Header("Fiziksel Referanslar")]
    private RectTransform emptyTileTransform; // Boş kutunun RectTransform'u
    private int emptyTileIndex = 8; // Boş kutunun başlangıçtaki index'i (son kutu)

    public void puzzle_baslat()
    {
        // Başlangıçta UI'ı kapat
        puzzlePanel.SetActive(false);
        messageText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        // Boş kutunun transformunu bul
        if (allTiles.Count > 0)
        {
            // Son elementin (Boş Kutu) transformunu alır
            emptyTileTransform = allTiles[emptyTileIndex].GetComponent<RectTransform>();
        }
    }

    // Harita butonu gibi bir yerden çağrılır
    public void OpenPuzzle()
    {
        if (!puzzlePanel.activeSelf)
        {
            puzzlePanel.SetActive(true);
            messageText.text = "Kutuları Kaydırarak Resmi Tamamla!";
            closeButton.gameObject.SetActive(false);
            ShuffleTiles(20); // Oyuncuyu zorlamak için 20 kez karıştır
        }
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
    }

    // Her bir kutunun OnClick event'ine atanacak metod
    public void OnTileClick(int tileIndex)
    {
        if (CheckWinCondition()) return; // Zaten çözülmüşse hareket etme

        // Tıklanan kutu boş kutunun komşusu mu?
        if (IsAdjacentToEmpty(tileIndex))
        {
            SwapTiles(tileIndex, emptyTileIndex);

            // Swap işleminden sonra boş kutunun yeni yerini güncelle
            emptyTileIndex = GetTileCurrentIndex(emptyTileTransform);

            if (CheckWinCondition())
            {
                GameWon();
            }
        }
    }

    bool IsAdjacentToEmpty(int tileIndex)
    {
        // Mevcut 3x3 ızgaradaki konumlar.
        // Hiyerarşi sırasına göre index'ler (0-8) kullanılır.
        int x1 = tileIndex % 3;
        int y1 = tileIndex / 3;
        int x2 = emptyTileIndex % 3;
        int y2 = emptyTileIndex / 3;

        // X veya Y'de 1 birim fark var VE aynı anda her iki eksende hareket yok
        return (Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) == 1);
    }

    void SwapTiles(int tileIndex, int emptyIndex)
    {
        // Tıklanan ve boş kutuların pozisyonlarını anlık değiştir
        RectTransform tileTransform = allTiles[tileIndex].GetComponent<RectTransform>();

        Vector3 tempPos = tileTransform.anchoredPosition;
        tileTransform.anchoredPosition = emptyTileTransform.anchoredPosition;
        emptyTileTransform.anchoredPosition = tempPos;

        // allTiles listesindeki referansları da yer değiştir (mantık sırasını korumak için)
        Button tempTile = allTiles[tileIndex];
        allTiles[tileIndex] = allTiles[emptyIndex];
        allTiles[emptyIndex] = tempTile;
    }

    // Kutuları Karıştırma
    void ShuffleTiles(int moves)
    {
    
            for (int i = 0; i < moves; i++)
            {
                // Hareket edebilecek komşuları bul
                List<int> possibleMoves = new List<int>();
                for (int j = 0; j < allTiles.Count; j++)
                {
                    if (IsAdjacentToEmpty(j))
                    {
                        possibleMoves.Add(j);
                    }
                }

                if (possibleMoves.Count > 0)
                {
                    // Rastgele bir komşuyu seç
                    int tileToSwapIndex = possibleMoves[Random.Range(0, possibleMoves.Count)]; // Düzeltildi

                    // Boşlukla yer değiştir
                    SwapTiles(tileToSwapIndex, emptyTileIndex);

                    // Boş kutunun yeni yerini güncelle
                    emptyTileIndex = tileToSwapIndex; // Hata burada olabilir: tileToSwapIndex olmalıydı
                }
            }
    }

    // Kutunun listedeki anlık index'ini bulur (Swap sonrası güncellenen pozisyon için gereklidir)
    int GetTileCurrentIndex(RectTransform targetTransform)
    {
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].GetComponent<RectTransform>() == targetTransform)
            {
                return i;
            }
        }
        return -1;
    }


    bool CheckWinCondition()
    {
        // Kazanma sırasını kontrol et: Her kutunun doğru yerde olup olmadığı
        for (int i = 0; i < allTiles.Count - 1; i++) // Son boş kutu hariç
        {
            // allTiles[i] listesi, o anki kutu objesini tutar.
            // Bu kutunun isminden (Tile_0, Tile_1 vb.) orijinal ID'sini almalıyız.
            // Basitleştirmek için, kutunun Hiyerarşi'deki başlangıç index'i (name) ile kontrol edelim.

            // Eğer kutuların isimlendirmesi "Tile (0)", "Tile (1)" şeklinde ise:
            // Sadece bu kontrol, görsel sırayı kontrol etmek için yeterli olacaktır.
            if (allTiles[i].name != "Tile (" + winningOrder[i] + ")")
            {
                return false;
            }
        }
        return true;
    }

    void GameWon()
    {
        messageText.text = "Tebrikler! Puzzle Çözüldü!";
        messageText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        // İsteğe bağlı: Boş kutunun yerini, son kutunun resmiyle doldur.
        // allTiles[emptyTileIndex].GetComponent<Image>().enabled = true; 
    }
}