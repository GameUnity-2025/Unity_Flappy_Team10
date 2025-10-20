using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdControl : MonoBehaviour
{
    [Header("Movement")]
    public int rotateRate = 6;
    public float upSpeed = 2.5f;
    public float gravity = 0.45f; // giảm trọng lực

    [Header("Refs")]
    public GameObject scoreMgr;
    public GameObject gameOverPanel;   // GameObject có component GameOverPanel
    public GameObject pipeSpawner;

    [Header("SFX")]
    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;

    [Header("State")]
    public bool inGame = false;

    private bool dead = false;
    private bool landed = false;
    private Sequence birdSequence;
    private Vector3 startPos;
    private Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;

        // DOTween nhảy nhẹ ở màn chờ
        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();
        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
                    .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
                    .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
                    .SetLoops(-1);

        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.gravityScale = 0f;      // chưa vào game thì tắt gravity
        rb.velocity = Vector2.zero;

        // Giá trị chuẩn
        upSpeed = 2.4f;
        gravity = 0.45f;
        rotateRate = 6;
    }

    public void RestartBird()
    {
        // Reset vị trí và xoay chim
        transform.position = startPos;
        transform.rotation = Quaternion.identity;

        // Reset Rigidbody
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f; // chờ chơi

        // Reset trạng thái
        dead = false;
        landed = false;
        inGame = false;

        // Bật lại animation nhảy nhẹ
        if (birdSequence != null)
        {
            if (!birdSequence.IsActive()) // nếu sequence bị Kill
            {
                float birdOffset = 0.05f;
                float birdTime = 0.3f;
                float birdStartY = transform.position.y;

                birdSequence = DOTween.Sequence();
                birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
                            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
                            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
                            .SetLoops(-1);
            }
            else
            {
                birdSequence.Restart();
            }
        }
    }

    void Update()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Chưa vào game: click để bắt đầu
        if (!inGame)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                inGame = true;
                rb.gravityScale = gravity;   // bật gravity
                if (birdSequence != null) birdSequence.Kill(); // dừng animation nhảy nhẹ
                JumpUp();

                // Kích hoạt pipe spawner
                if (pipeSpawner != null)
                {
                    var spawner = pipeSpawner.GetComponent<PipeSpawner>();
                    if (spawner != null) spawner.StartSpawning();
                }

                // Ẩn ready/tips nếu có
                GameObject gameMain = GameObject.Find("GameMain");
                if (gameMain != null)
                {
                    GameMain gameMainScript = gameMain.GetComponent<GameMain>();
                    if (gameMainScript != null)
                    {
                        gameMainScript.HideUI();
                    }
                }
            }
            return;
        }

        // Trong game: click để nhảy
        if (!dead && (Input.GetButtonDown("Fire1")))
        {
            JumpUp();
        }

        // Xoay chim theo vận tốc
        if (!landed)
        {
            float v = rb.velocity.y;
            float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
            transform.rotation = Quaternion.Euler(0f, 0f, rotate);
        }
        else
        {
            rb.rotation = -90;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Va chạm vật cản/đất -> die
        if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
        {
            if (!dead)
            {
                // Báo tất cả vật thể có tag "movable" dừng lại
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver", SendMessageOptions.DontRequireReceiver);
                }

                // Animation + âm thanh
                var anim = GetComponent<Animator>();
                if (anim) anim.SetTrigger("die");
                if (hit) AudioSource.PlayClipAtPoint(hit, Vector3.zero);

                // Gọi GameOver (một lần)
                GameOver();
            }

            if (other.name == "land")
            {
                // dừng ngay khi chạm đất
                if (rb == null) rb = GetComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

                landed = true;

                // Hiển thị Game Over Panel sau một khoảng delay nhỏ (real-time)
                StartCoroutine(ShowGameOverDelay());
            }

            return;
        }

        // Qua điểm
        if (other.name == "pass_trigger" || other.CompareTag("Score"))
        {
            if (scoreMgr != null)
            {
                var sm = scoreMgr.GetComponent<ScoreMgr>();
                if (sm != null) sm.AddScore();
            }
            if (score) AudioSource.PlayClipAtPoint(score, Vector3.zero);
            return;
        }
    }

    IEnumerator ShowGameOverDelay()
    {
        // dùng realtime để không phụ thuộc timeScale
        yield return new WaitForSecondsRealtime(1f);

        if (gameOverPanel != null)
        {
            int currentScore = 0;
            if (scoreMgr != null)
            {
                var sm = scoreMgr.GetComponent<ScoreMgr>();
                if (sm != null) currentScore = sm.GetCurrentScore();
            }

            var panel = gameOverPanel.GetComponent<GameOverPanel>();
            if (panel != null)
            {
                panel.ShowGameOver(currentScore);
            }
            else
            {
                Debug.LogWarning("GameOverPanel component not found on gameOverPanel object.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ gameOverPanel chưa được gán trong BirdControl!");
        }
    }

    public void JumpUp()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, upSpeed); // set velocity trực tiếp
        if (jumpUp) AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }

    public void GameOver()
    {
        if (dead) return; // chống gọi lặp
        dead = true;

        // Dừng ống bay
        if (pipeSpawner != null)
        {
            var spawner = pipeSpawner.GetComponent<PipeSpawner>();
            if (spawner != null) spawner.GameOver();
        }

        // Ghi điểm vào Leaderboard nếu có
        try
        {
            int finalScore = 0;
            if (scoreMgr != null)
            {
                var sm = scoreMgr.GetComponent<ScoreMgr>();
                if (sm != null) finalScore = sm.GetCurrentScore();
            }

            var lb = FindObjectOfType<LeaderboardMgr>();
            if (lb != null)
            {
                lb.AddScore("Player", finalScore);

                var ui = FindObjectOfType<LeaderboardUI>();
                if (ui != null) ui.ForceUpdate();
            }
        }
        catch { /* bỏ qua nếu không có hệ leaderboard */ }

        // Hiện panel sau 1s (real-time)
        StartCoroutine(ShowGameOverPanelDelayed());
    }

    IEnumerator ShowGameOverPanelDelayed()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (gameOverPanel != null)
        {
            int currentScore = 0;
            if (scoreMgr != null)
            {
                var sm = scoreMgr.GetComponent<ScoreMgr>();
                if (sm != null) currentScore = sm.GetCurrentScore();
            }

            var panel = gameOverPanel.GetComponent<GameOverPanel>();
            if (panel != null)
            {
                panel.ShowGameOver(currentScore);
            }
            else
            {
                Debug.LogWarning("GameOverPanel component not found on gameOverPanel object.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverPanel chưa được gán trong BirdControl!");
        }
    }
}
