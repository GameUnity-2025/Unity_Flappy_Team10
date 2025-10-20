using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour
{
    [Header("Panel container (optional)")]
    [Tooltip("Để trống nếu dùng chính GameObject này làm panel")]
    public GameObject panel;

    [Header("UI")]
    public Text currentScoreText;
    public Text bestScoreText;

    [Header("Buttons")]
    public Button playButton;        // nếu gán, sẽ reload GameScene
    public Button playAgainButton;   // alias cho playButton
    public Button homeButton;        // về StartScene

    [Header("Animation")]
    public bool useScaleAnim = true;
    public float animDuration = 0.3f;

    private const string BEST_SCORE_KEY = "BestScore";

    void Start()
    {
        // Ẩn panel khi bắt đầu
        var target = panel != null ? panel : gameObject;
        target.SetActive(false);

        // Gắn handler nút
        if (playButton != null) playButton.onClick.AddListener(OnPlayAgain);
        if (playAgainButton != null) playAgainButton.onClick.AddListener(OnPlayAgain);
        if (homeButton != null) homeButton.onClick.AddListener(OnHome);
    }

    /// <summary>Gọi khi chim chết, truyền vào điểm hiện tại</summary>
    public void ShowGameOver(int currentScore)
    {
        // Đọc & cập nhật best score
        int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            PlayerPrefs.Save();
        }

        // Cập nhật UI
        if (currentScoreText != null) currentScoreText.text = currentScore.ToString();
        if (bestScoreText != null) bestScoreText.text = bestScore.ToString();

        // Hiển thị panel
        var target = panel != null ? panel : gameObject;
        target.SetActive(true);

        // Animation (nếu bật)
        if (useScaleAnim)
        {
            var t = target.transform;
            t.localScale = Vector3.zero;
            t.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack);
        }
    }

    private void OnPlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    private void OnHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
