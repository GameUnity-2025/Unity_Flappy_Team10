using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdControl : MonoBehaviour
{

<<<<<<< HEAD
    public int rotateRate = 3;
=======
    public int rotateRate = 10;
>>>>>>> 264eee5 (Create Panel GameOver)
    public float upSpeed = 10;
    public GameObject scoreMgr;
    public GameObject gameOverPanel;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;
    public GameObject gameoverPic;
    public GameObject pipeSpawner;
    public bool inGame = false;

    private bool dead = false;
    private bool landed = false;

    private Sequence birdSequence;



    // Use this for initialization
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (!inGame)
        {
            return;
        }
        birdSequence.Kill();

        if (!dead)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                JumpUp();
            }
        }

        if (!landed)
        {
            float v = transform.GetComponent<Rigidbody2D>().velocity.y;

<<<<<<< HEAD
            float rotate = Mathf.Clamp(v * 2f, -45f, 10f);


=======
            float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
>>>>>>> 264eee5 (Create Panel GameOver)

            transform.rotation = Quaternion.Euler(0f, 0f, rotate);
        }
        else
        {
            transform.GetComponent<Rigidbody2D>().rotation = -90;
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
                // Gửi GameOver tới các vật thể di chuyển (ống, đất,...)
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver", SendMessageOptions.DontRequireReceiver);
                }

                // Gọi GameOver cho chính con chim
                GameOver();

                // Animation và âm thanh
                GetComponent<Animator>().SetTrigger("die");
                AudioSource.PlayClipAtPoint(hit, Vector3.zero);

                // Hiện panel Game Over sau 1 giây
                StartCoroutine(ShowGameOverDelay());
            }

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
                landed = true;
            }
        }

        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }
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
    }

    public void JumpUp()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }

    public void GameOver()
    {
        dead = true;
<<<<<<< HEAD
    }
}
=======

        // Hiện ảnh Game Over
        gameoverPic.GetComponent<SpriteRenderer>().enabled = true;

        // Ẩn điểm số và dừng ống bay
        pipeSpawner.GetComponent<PipeSpawner>().GameOver();
    }

}

>>>>>>> 264eee5 (Create Panel GameOver)
