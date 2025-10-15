using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartMain : MonoBehaviour
{
<<<<<<< HEAD
=======

>>>>>>> 264eee5 (Create Panel GameOver)
    public GameObject bird;
    public GameObject land;
    public GameObject back_ground;
    public Sprite[] back_list;

    private GameObject nowPressBtn = null;

<<<<<<< HEAD
    void Start()
    {
        // random background
        int index = Random.Range(0, back_list.Length);
        if (back_ground != null && back_list != null && back_list.Length > 0)
=======
    // Use this for initialization
    void Start()
    {

        // random background
        int index = Random.Range(0, back_list.Length);
        back_ground.GetComponent<SpriteRenderer>().sprite = back_list[index];

    }

    // Update is called once per frame
    void Update()
    {
        // Handle native touch events
        foreach (Touch touch in Input.touches)
>>>>>>> 264eee5 (Create Panel GameOver)
        {
            var sr = back_ground.GetComponent<SpriteRenderer>();
            if (sr) sr.sprite = back_list[index];
        }
    }

    void Update()
    {
        // Touch
        foreach (Touch touch in Input.touches)
            HandleTouch(touch.fingerId, touch.position, touch.phase);

        // Mouse giả lập touch
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0)) HandleTouch(10, Input.mousePosition, TouchPhase.Began);
            if (Input.GetMouseButton(0))     HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
            if (Input.GetMouseButtonUp(0))   HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
        }
    }

    private void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(touchPosition);
        Vector2 worldPos = new Vector2(wp.x, wp.y);

        switch (touchPhase)
        {
            case TouchPhase.Began:
<<<<<<< HEAD
=======
                print(touchPosition);
                print(worldPos);

>>>>>>> 264eee5 (Create Panel GameOver)
                foreach (Collider2D c in Physics2D.OverlapPointAll(worldPos))
                {
                    string n = c.gameObject.name;
                    if (n == "start_btn" || n == "rank_btn" || n == "rate_btn")
                    {
                        c.transform.DOMoveY(c.transform.position.y - 0.03f, 0f);
                        nowPressBtn = c.gameObject;
                    }
                }
                break;

            case TouchPhase.Ended:
                if (nowPressBtn)
                {
                    nowPressBtn.transform.DOMoveY(nowPressBtn.transform.position.y + 0.03f, 0f);

                    foreach (Collider2D c in Physics2D.OverlapPointAll(worldPos))
                    {
                        if (c.gameObject.name == nowPressBtn.name)
                        {
                            if (nowPressBtn.name == "start_btn")
                                OnPressStart();
                        }
                    }
                    nowPressBtn = null;
                }
<<<<<<< HEAD
=======


>>>>>>> 264eee5 (Create Panel GameOver)
                break;
        }
    }

    private void OnPressStart()
    {
        
        SceneManager.LoadScene("GameScene");

        
    }
}
