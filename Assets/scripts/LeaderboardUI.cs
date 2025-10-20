using UnityEngine;
using TMPro;
using UnityEngine.UI; // 🟢 Thêm để dùng Button
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Danh sách Text hiển thị tên người chơi")]
    public TextMeshProUGUI[] playerTexts;

    [Header("Danh sách Text hiển thị điểm số")]
    public TextMeshProUGUI[] scoreTexts;

    [Header("Nút quay lại và màn hình chính")]
    public Button backButton;             // 🟢 Nút Back
    public GameObject leaderboardPanel;   // 🟢 Panel hiện tại (Top Scores)
    public GameObject mainMenuPanel;      // 🟢 Panel menu chính

    private void OnEnable()
    {
        StartCoroutine(DelayUpdate());
    }

    private void Start()
    {
        // Gán sự kiện click cho nút back
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);
    }

    private IEnumerator DelayUpdate()
    {
        yield return null;
        UpdateLeaderboardUI();
    }

    public void UpdateLeaderboardUI()
    {
        if (LeaderboardMgr.Instance == null)
        {
            Debug.LogWarning("⚠️ LeaderboardMgr chưa được khởi tạo!");
            return;
        }

        var data = LeaderboardMgr.Instance.leaderboard;
        if (data == null)
        {
            Debug.LogWarning("⚠️ Leaderboard data null!");
            return;
        }

        int length = Mathf.Min(playerTexts.Length, scoreTexts.Length);

        for (int i = 0; i < length; i++)
        {
            if (i < data.Count)
            {
                playerTexts[i].text = data[i].playerName;
                scoreTexts[i].text = data[i].score.ToString();
            }
            else
            {
                playerTexts[i].text = "-";
                scoreTexts[i].text = "0";
            }
        }

        Debug.Log($"✅ Leaderboard UI updated ({data.Count} entries)");
    }

    public void ForceUpdate()
    {
        StartCoroutine(DelayUpdate());
    }

   public void OnBackClicked()
{
    leaderboardPanel?.SetActive(false);
    gameObject.SetActive(false);

    // Gọi RankButtonController để reset biến
    var rankBtn = GameObject.Find("rank_btn");
    if (rankBtn != null)
    {
        var controller = rankBtn.GetComponent<RankButtonController>();
        if (controller != null)
            controller.ResetState();
    }

    // Bật lại màn hình chính
    if (mainMenuPanel != null)
        mainMenuPanel.SetActive(true);
}
    public void HideLeaderboard()
{
    gameObject.SetActive(false); // Ẩn bảng điểm
    Debug.Log("🏁 Leaderboard closed!");
}

}
