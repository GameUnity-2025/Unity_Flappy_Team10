using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour
{
    public Text currentScoreText;
    public Text bestScoreText;
    public Button homeButton;
    public Button playAgainButton;

    private int currentScore = 0;
    private int bestScore = 0;

    void Start()
    {
        // Ẩn panel khi bắt đầu
        gameObject.SetActive(false);

        // Đọc điểm cao nhất từ PlayerPrefs
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Gắn sự kiện cho các nút
        if (homeButton != null)
            homeButton.onClick.AddListener(OnHomeButtonClick);

        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
    }

    public void ShowGameOver(int score)
    {
        currentScore = score;

        // Cập nhật điểm cao nhất
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        // Cập nhật UI
        if (currentScoreText != null)
            currentScoreText.text = currentScore.ToString();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();


        // Hiện panel ở giữa màn hình
        gameObject.SetActive(true);
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, 130f);
        rt.localScale = new Vector3(3.7f, 4f, 1f); // ✅ Scale theo đúng bạn muốn

    }

    void OnHomeButtonClick()
    {
        // Về trang chính (Scene menu hoặc scene đầu tiên)
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Hoặc tên scene menu của bạn
    }

    void OnPlayAgainButtonClick()
    {
        Time.timeScale = 1f;
        GameObject bird = GameObject.Find("bird"); // Thay "bird" bằng tên chính xác của GameObject chim
        if (bird != null)
        {
            bird.GetComponent<BirdControl>().RestartBird();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}