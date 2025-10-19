using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Legacy Text kullanıyorsanız
using UnityEngine.SceneManagement;

public class SimonGameManager : MonoBehaviour
{
    [Header("UI & Buton Referansları")]
    public SimonButton[] simonButtons;
    public Text scoreText;
    public Text messageText;

    [Header("Kazanma Ekranı")]
    public Text winMessageText;      // Editörden atanacak: "KAZANDIN!" mesajı
    public Button continueButton;    // Editörden atanacak: Devam etme butonu

    [Header("Oyun Ayarları")]
    public float flashDelay = 0.5f;
    public float roundDelay = 1.0f;
    public int winScore = 3; // Kazanma skoru olarak 3 belirlendi

    // Oyun Durumu Değişkenleri
    private List<int> sequence = new List<int>();
    private int playerSequenceIndex;
    private int score = 0;

    [HideInInspector] public bool isPlayerTurn = false;
    [HideInInspector] public bool IsFlashing { get; private set; } = false;

    void Awake()
    {
        // UI elementlerini başlangıçta kapat
        if (winMessageText != null) winMessageText.gameObject.SetActive(false);
        if (continueButton != null) continueButton.gameObject.SetActive(false);
    }

    void Start()
    {
        if (simonButtons.Length != 9)
        {
            Debug.LogError("Simon Buttons dizisi 9 buton içermiyor. Lütfen Inspector'dan kontrol edin!");
            return;
        }

        for (int i = 0; i < simonButtons.Length; i++)
        {
            simonButtons[i].gameManager = this;
            simonButtons[i].buttonID = i;
        }

        UpdateScoreUI();
    }

    public void StartGame()
    {
        score = 0;
        sequence.Clear();
        // Kazanma UI'ını kapat
        if (winMessageText != null) winMessageText.gameObject.SetActive(false);
        if (continueButton != null) continueButton.gameObject.SetActive(false);
        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        // KAZANMA KONTROLÜ
        if (score >= winScore)
        {
            GameWon();
            yield break; // Coroutine'i burada sonlandır
        }
        // KAZANMA KONTROLÜ BİTİŞ

        UpdateUI("Simon Söylüyor...");
        isPlayerTurn = false;
        IsFlashing = true;

        int nextColor = Random.Range(0, simonButtons.Length);
        sequence.Add(nextColor);

        yield return new WaitForSeconds(roundDelay);

        foreach (int buttonID in sequence)
        {
            yield return StartCoroutine(simonButtons[buttonID].Flash());
            yield return new WaitForSeconds(flashDelay);
        }

        IsFlashing = false;
        playerSequenceIndex = 0;
        UpdateUI("Sıra Sende!");
        isPlayerTurn = true;
    }

    public void PlayerChose(int chosenID)
    {
        if (IsFlashing || !isPlayerTurn) return;

        // 1. Doğru mu kontrol et
        if (chosenID == sequence[playerSequenceIndex])
        {
            playerSequenceIndex++;

            // 2. Sırayı tamamladı mı?
            if (playerSequenceIndex == sequence.Count)
            {
                score++;
                UpdateScoreUI();
                isPlayerTurn = false;
                StartCoroutine(NextRound()); // Bir sonraki raunda geç (veya GameWon'a gider)
            }
        }
        else
        {
            // 3. Yanlış! Oyun bitti
            GameOver();
        }
    }

    // YENİ FONKSİYON: Oyunu Kazandın
    void GameWon()
    {
        isPlayerTurn = false;
        UpdateUI("KAZANDIN!"); // Mesaj alanına KAZANDIN yaz

        // Kazanma ekranı elementlerini görünür yap
        if (winMessageText != null) winMessageText.gameObject.SetActive(true);
        if (continueButton != null) continueButton.gameObject.SetActive(true);
    }

    // YANLIŞ CEVAP: Oyun Bitti
    void GameOver()
    {
        UpdateUI($"Oyun Bitti! Skorun: {score}");
        isPlayerTurn = false;
        SceneManager.LoadScene(3);

    }

    // YENİ FONKSİYON: Devam Butonu için (Haritaya Dönüş vb.)
    public void ContinueGame()
    {
        // Örneğin: Minigame'in bulunduğu sahneden, haritanın bulunduğu sahneye geç
        // Lütfen doğru sahne index'ini Build Settings'den kontrol edin!
    continueButton.gameObject.SetActive(true); 
    }

    void UpdateUI(string message)
    {
        if (messageText != null) messageText.text = message;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Skor: " + score.ToString();
    }
}