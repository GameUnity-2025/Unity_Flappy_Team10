using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    [Header("Refs")]
    public GameObject bird;
    public GameObject readyPic;
    public GameObject tipPic;
    public GameObject scoreMgr;
    public GameObject pipeSpawner;

    [Header("UI Buttons (optional)")]
    public Button rank_btn; // nếu có dùng, gán onClick ở Inspector

    private bool gameStarted = false;
    private BirdControl birdControl;

    void Awake()
    {
        if (bird != null) birdControl = bird.GetComponent<BirdControl>();
    }

    void Start()
    {
        gameStarted = false;
        // Mặc định: để BirdControl tự lo input start game (theo code BirdControl bạn đã gửi)
        // Nếu muốn GameMain bắt đầu bằng click đầu tiên, bật lại đoạn dưới và tắt logic ở BirdControl.
        // (Giữ nguyên hiện tại: không xử lý input ở GameMain)
    }

    // Không xử lý input ở đây nữa – tránh xung đột với BirdControl.Update()

    /// <summary>Bắt đầu game theo lệnh ngoài (nếu muốn dùng nút Play, v.v.)</summary>
    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        // Bật trạng thái inGame & nhảy lần đầu
        if (birdControl == null && bird != null) birdControl = bird.GetComponent<BirdControl>();
        if (birdControl != null)
        {
            birdControl.inGame = true;
            birdControl.JumpUp();
        }

        // Ẩn ready/tip
        HideUI();

        // Bắt đầu spawn ống
        if (pipeSpawner != null)
        {
            var spawner = pipeSpawner.GetComponent<PipeSpawner>();
            if (spawner != null) spawner.StartSpawning();
        }
    }

    /// <summary>Ẩn UI hướng dẫn</summary>
    public void HideUI()
    {
        if (readyPic != null)
        {
            var sr = readyPic.GetComponent<SpriteRenderer>();
            if (sr != null) sr.DOFade(0f, 0.2f);
        }
        if (tipPic != null)
        {
            var sr = tipPic.GetComponent<SpriteRenderer>();
            if (sr != null) sr.DOFade(0f, 0.2f);
        }
    }

    /// <summary>Hiện lại UI khi restart</summary>
    public void RestartGame()
    {
        gameStarted = false;

        if (readyPic != null)
        {
            var sr = readyPic.GetComponent<SpriteRenderer>();
            if (sr != null) sr.DOFade(1f, 0.2f);
        }
        if (tipPic != null)
        {
            var sr = tipPic.GetComponent<SpriteRenderer>();
            if (sr != null) sr.DOFade(1f, 0.2f);
        }
    }

    /// <summary>Gọi khi game over (nếu muốn quản lý ở cấp GameMain). BirdControl đã tự gọi panel rồi.</summary>
    public void GameOver()
    {
        // Dừng tạo ống
        if (pipeSpawner != null)
        {
            var spawner = pipeSpawner.GetComponent<PipeSpawner>();
            if (spawner != null) spawner.GameOver();
        }

        // Lấy điểm hiện tại
        int finalScore = 0;
        if (scoreMgr != null)
        {
            var sm = scoreMgr.GetComponent<ScoreMgr>();
            if (sm != null) finalScore = sm.GetScore();
        }

        // Lưu leaderboard nếu có
        var lb = FindObjectOfType<LeaderboardMgr>();
        if (lb != null)
        {
            lb.AddScore("Player", finalScore);

            var ui = FindObjectOfType<LeaderboardUI>();
            if (ui != null) ui.ForceUpdate();
        }

        Debug.Log($"Game Over! Final Score = {finalScore}");
    }
}
