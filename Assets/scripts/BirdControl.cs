using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdControl : MonoBehaviour
{
<<<<<<< HEAD

<<<<<<< HEAD
    public int rotateRate = 3;
=======
    public int rotateRate = 10;
>>>>>>> 264eee5 (Create Panel GameOver)
    public float upSpeed = 10;
=======
    public int rotateRate = 6;
    public float upSpeed = 2.5f;
    public float gravity = 0.45f; // 🔹 Giảm trọng lực xuống
>>>>>>> 25e8c24 (khi chết bấm nút play trên panel quay lại trang chơi ban đầu , ghi nhận được điểm)
    public GameObject scoreMgr;
    public GameObject gameOverPanel;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;
    //public GameObject gameoverPic;
    public GameObject pipeSpawner;
    public GameObject gameOverPanel;

    public bool inGame = false;

    private bool dead = false;
    private bool landed = false;
    private Sequence birdSequence;
    private Vector3 startPos;


    void Start()
    {
        startPos = transform.position;

        // DOTween nhảy nhẹ
        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();
        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
            .SetLoops(-1);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // gravity tắt lúc chờ
        rb.velocity = Vector2.zero;
        rb.simulated = true;

        // Set các giá trị chuẩn
        upSpeed = 2.4f;
        gravity = 0.45f;
        rotateRate = 6;
    }
    public void RestartBird()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Reset vị trí và xoay chim
        transform.position = startPos;
        transform.rotation = Quaternion.identity;

        // Reset Rigidbody
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f; // gravity tắt lúc chờ chơi

        // Reset trạng thái
        dead = false;
        landed = false;
        inGame = false;

        // Bật lại animation nhảy nhẹ
        if (birdSequence != null)
            birdSequence.Restart();
    }


    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (!inGame)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                inGame = true;
                rb.gravityScale = gravity; // bật gravity
                birdSequence.Kill(); // dừng animation nhảy nhẹ
                JumpUp();
                
                // Kích hoạt pipe spawner
                if (pipeSpawner != null)
                {
                    pipeSpawner.GetComponent<PipeSpawner>().StartSpawning();
                }
                
                // Ẩn readyPic và tipPic
                GameObject gameMain = GameObject.Find("GameMain");
                if (gameMain != null)
                {
                    GameMain gameMainScript = gameMain.GetComponent<GameMain>();
                    if (gameMainScript != null)
                    {
                        // Gọi hàm HideUI để ẩn UI
                        gameMainScript.HideUI();
                    }
                }
            }
            return;
        }

        if (!dead && Input.GetButtonDown("Fire1"))
        {
            JumpUp();
        }

        // Xoay chim
        if (!landed)
        {
<<<<<<< HEAD
            float v = transform.GetComponent<Rigidbody2D>().velocity.y;

<<<<<<< HEAD
            float rotate = Mathf.Clamp(v * 2f, -45f, 10f);


=======
            float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
>>>>>>> 264eee5 (Create Panel GameOver)

=======
            float v = rb.velocity.y;
            float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
>>>>>>> 25e8c24 (khi chết bấm nút play trên panel quay lại trang chơi ban đầu , ghi nhận được điểm)
            transform.rotation = Quaternion.Euler(0f, 0f, rotate);
        }
        else
        {
            rb.rotation = -90;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
<<<<<<< HEAD
        // 🚫 Bỏ qua va chạm khi game chưa bắt đầu
        if (!inGame) return;

=======
>>>>>>> 264eee5 (Create Panel GameOver)
        if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
        {
            if (!dead)
            {
                // Gửi GameOver tới các vật thể di chuyển
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver", SendMessageOptions.DontRequireReceiver);
                }

                // Gọi GameOver
                GameOver();

                // Animation và âm thanh
                GetComponent<Animator>().SetTrigger("die");
                AudioSource.PlayClipAtPoint(hit, Vector3.zero);

                // Hiện panel Game Over sau 1 giây
                StartCoroutine(ShowGameOverDelay());
            }

<<<<<<< HEAD
<<<<<<< HEAD
=======



>>>>>>> 264eee5 (Create Panel GameOver)
            if (other.name == "land")
            {
                transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
<<<<<<< HEAD
=======

>>>>>>> 264eee5 (Create Panel GameOver)
=======
            if (other.name == "land")
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
>>>>>>> 25e8c24 (khi chết bấm nút play trên panel quay lại trang chơi ban đầu , ghi nhận được điểm)
                landed = true;
            }
        }

        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }
<<<<<<< HEAD
    }

    IEnumerator ShowGameOverDelay()
    {
        yield return new WaitForSeconds(1f);

<<<<<<< HEAD
        if (gameOverPanel != null)
        {
            int currentScore = scoreMgr.GetComponent<ScoreMgr>().GetCurrentScore();
            gameOverPanel.GetComponent<GameOverPanel>().ShowGameOver(currentScore);
        }
=======
>>>>>>> 264eee5 (Create Panel GameOver)
=======
>>>>>>> 25e8c24 (khi chết bấm nút play trên panel quay lại trang chơi ban đầu , ghi nhận được điểm)
    }

    public void JumpUp()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, upSpeed); // dùng velocity trực tiếp
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }


    public void GameOver()
    {
        dead = true;
<<<<<<< HEAD
    }
}
=======

        // Dừng ống bay
        if (pipeSpawner != null)
            pipeSpawner.GetComponent<PipeSpawner>().GameOver();

        // Hiển thị Game Over Panel với điểm số sau 1 giây
        StartCoroutine(ShowGameOverPanelDelayed());
    }

    IEnumerator ShowGameOverPanelDelayed()
    {
        yield return new WaitForSecondsRealtime(1f);

<<<<<<< HEAD
>>>>>>> 264eee5 (Create Panel GameOver)
=======
        if (gameOverPanel != null)
        {
            int currentScore = scoreMgr.GetComponent<ScoreMgr>().GetCurrentScore();
            gameOverPanel.GetComponent<GameOverPanel>().ShowGameOver(currentScore);
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverPanel chưa được gán trong BirdControl!");
        }
    }
}
>>>>>>> 25e8c24 (khi chết bấm nút play trên panel quay lại trang chơi ban đầu , ghi nhận được điểm)
