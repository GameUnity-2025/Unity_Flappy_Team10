using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

public class LeaderboardMgr : MonoBehaviour
{
    public static LeaderboardMgr Instance;

    [Header("Danh sách bảng điểm")]
    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    private const string LEADERBOARD_KEY = "LeaderboardData";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard(); // 🟢 Load dữ liệu thật khi game khởi động
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(string playerName, int score)
    {
        leaderboard.Add(new LeaderboardEntry { playerName = playerName, score = score });

        // Sắp xếp điểm từ cao -> thấp
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        // Giữ tối đa 6 dòng
        if (leaderboard.Count > 6)
            leaderboard.RemoveRange(6, leaderboard.Count - 6);

        SaveLeaderboard(); // 🟢 Lưu lại mỗi khi có thay đổi
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new Wrapper { list = leaderboard });
        PlayerPrefs.SetString(LEADERBOARD_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("✅ Leaderboard saved: " + json);
    }

    public void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LEADERBOARD_KEY))
        {
            string json = PlayerPrefs.GetString(LEADERBOARD_KEY);
            leaderboard = JsonUtility.FromJson<Wrapper>(json).list;
            Debug.Log("📥 Leaderboard loaded: " + json);
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<LeaderboardEntry> list;
    }

    // 🔧 Xóa toàn bộ điểm (nếu muốn reset)
    public void ClearLeaderboard()
    {
        leaderboard.Clear();
        PlayerPrefs.DeleteKey(LEADERBOARD_KEY);
        Debug.Log("🧹 Leaderboard cleared!");
    }
}
