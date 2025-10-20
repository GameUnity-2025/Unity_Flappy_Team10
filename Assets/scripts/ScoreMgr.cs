using UnityEngine;
using System.Collections;

public class ScoreMgr : MonoBehaviour
{
    [Header("Digit prefabs 0-9 (order 0..9)")]
    public GameObject[] scorePrefabs;
    [Header("Khoảng cách giữa các chữ số")]
    public float digitOffset = 0.5f;

    // tối đa 5 chữ số (00000..99999). Tăng nếu cần.
    private GameObject[] nowShowScores = new GameObject[5];
    private int nowScore = 0;

    void Start()
    {
        nowScore = 0;
        SetScore(nowScore);
    }

    /// <summary>+1 điểm</summary>
    public void AddScore()
    {
        nowScore++;
        SetScore(nowScore);
    }

    /// <summary>+amount điểm</summary>
    public void AddScore(int amount)
    {
        nowScore += amount;
        if (nowScore < 0) nowScore = 0;
        SetScore(nowScore);
    }

    /// <summary>Lấy điểm hiện tại</summary>
    public int GetCurrentScore()
    {
        return nowScore;
    }

    /// <summary>Alias (giữ tương thích code cũ)</summary>
    public int GetScore()
    {
        return nowScore;
    }

    /// <summary>Set điểm & hiển thị lại các digit prefab</summary>
    public void SetScore(int score)
    {
        nowScore = Mathf.Max(0, score);

        // phá cũ để tránh ghost digits khi thay đổi số digit
        for (int i = 0; i < nowShowScores.Length; i++)
        {
            if (nowShowScores[i] != null)
            {
                Destroy(nowShowScores[i]);
                nowShowScores[i] = null;
            }
        }

        // Tách chữ số
        int tmpScore = nowScore;
        int[] digits = new int[nowShowScores.Length];
        int index = 0;

        if (tmpScore == 0)
        {
            digits[0] = 0;
            index = 1;
        }
        else
        {
            while (tmpScore != 0 && index < digits.Length)
            {
                digits[index] = tmpScore % 10;
                tmpScore /= 10;
                index++;
            }
        }

        int scoreSize = Mathf.Max(1, index);

        // canh giữa theo digitOffset
        float nowOffset = (scoreSize - 1) * digitOffset / 2f;

        for (int i = 0; i < scoreSize; i++)
        {
            float nowX = transform.position.x + nowOffset;
            Vector2 pos = new Vector2(nowX, transform.position.y);

            int digit = digits[i];
            if (scorePrefabs != null && digit >= 0 && digit < scorePrefabs.Length && scorePrefabs[digit] != null)
            {
                nowShowScores[i] = Instantiate(scorePrefabs[digit], pos, transform.rotation) as GameObject;
            }
            else
            {
                Debug.LogWarning($"ScoreMgr: thiếu prefab cho số {digit} hoặc mảng chưa gán.");
            }

            nowOffset -= digitOffset;
        }
    }
}
