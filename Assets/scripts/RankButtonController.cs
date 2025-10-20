using UnityEngine;

public class RankButtonController : MonoBehaviour
{
    [Header("Leaderboard Panel (kéo vô đây)")]
    public GameObject leaderboardPanel;

    private bool isOpen = false;

    private void Start()
    {
        // Ẩn bảng khi bắt đầu
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);
    }

   
    private void OnMouseDown()
    {
        if (leaderboardPanel != null)
        {
            isOpen = !isOpen;
            leaderboardPanel.SetActive(isOpen);
            Debug.Log("📊 Leaderboard " + (isOpen ? "Opened" : "Closed"));
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán Leaderboard Panel!");
        }
    }

    // 🟢 Hàm này sẽ được gọi từ LeaderboardUI khi Back
    public void ResetState()
    {
        isOpen = false;
        Debug.Log("🔄 RankButtonController state reset!");
    }
}

