using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public Text currentScoreText;   // Text hiển thị điểm hiện tại
    public Text bestScoreText;      // Text hiển thị điểm cao nhất
    public GameObject panel;        // Panel GameOver
    public Button restartButton;    // Nút chơi lại
    public Button homeButton;       // Nút về trang chính

    private int currentScore;
    private int bestScore;

    void Start()
    {
        panel.SetActive(false); // Ẩn panel khi bắt đầu

        // Gán sự kiện khi bấm nút
        restartButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(GoToMainMenu);
    }

    // Hàm gọi khi game over
    public void ShowGameOverPanel(int score)
    {
        panel.SetActive(true);
        currentScore = score;

        // Lấy điểm cao nhất cũ
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Cập nhật nếu điểm hiện tại cao hơn
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        // Cập nhật text hiển thị
        currentScoreText.text = "Score: " + currentScore.ToString();
        bestScoreText.text = "Best: " + bestScore.ToString();
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // đổi "MainMenu" thành tên scene menu của bạn
    }
}
