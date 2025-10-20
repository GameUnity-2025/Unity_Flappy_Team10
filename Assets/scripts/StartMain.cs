using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartMain : MonoBehaviour
{
    [Header("Scene Refs")]
    public GameObject bird;
    public GameObject land;
    public GameObject back_ground;

    [Header("Backgrounds")]
    public Sprite[] back_list;

    private GameObject nowPressBtn = null;

    void Start()
    {
        // Random background an toàn (có kiểm tra null/length)
        if (back_ground != null && back_list != null && back_list.Length > 0)
        {
            int index = Random.Range(0, back_list.Length);
            var sr = back_ground.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = back_list[index];
        }

        // Cập nhật leaderboard nếu có UI trong scene
        var lbUI = FindObjectOfType<LeaderboardUI>();
        if (lbUI != null)
        {
            // Nếu script có hàm UpdateLeaderboardUI() / ForceUpdate() thì gọi cho chắc:
            try
            {
                lbUI.ForceUpdate();
            }
            catch
            {
                try { lbUI.UpdateLeaderboardUI(); } catch { /* bỏ qua nếu không có */ }
            }
        }
    }

    void Update()
    {
        // Xử lý touch thật
        foreach (Touch touch in Input.touches)
            HandleTouch(touch.fingerId, touch.position, touch.phase);

        // Giả lập touch bằng chuột khi chạy trên PC
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0)) HandleTouch(10, Input.mousePosition, TouchPhase.Began);
            if (Input.GetMouseButton(0))     HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
            if (Input.GetMouseButtonUp(0))   HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
        }
    }

    private void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase)
    {
        var cam = Camera.main;
        if (cam == null) return;

        Vector3 wp = cam.ScreenToWorldPoint(touchPosition);
        Vector2 worldPos = new Vector2(wp.x, wp.y);

        switch (touchPhase)
        {
            case TouchPhase.Began:
                foreach (Collider2D c in Physics2D.OverlapPointAll(worldPos))
                {
                    string n = c.gameObject.name;
                    if (n == "start_btn" || n == "rank_btn" || n == "rate_btn")
                    {
                        // nhấn: hạ nút xuống 1 tí
                        c.transform.DOMoveY(c.transform.position.y - 0.03f, 0f);
                        nowPressBtn = c.gameObject;
                    }
                }
                break;

            case TouchPhase.Ended:
                if (nowPressBtn)
                {
                    // thả: trả nút về vị trí
                    nowPressBtn.transform.DOMoveY(nowPressBtn.transform.position.y + 0.03f, 0f);

                    foreach (Collider2D c in Physics2D.OverlapPointAll(worldPos))
                    {
                        if (c.gameObject.name == nowPressBtn.name)
                        {
                            if (nowPressBtn.name == "start_btn")
                                OnPressStart();
                            // nếu cần: else if (nowPressBtn.name == "rank_btn") ...; "rate_btn" ...
                        }
                    }
                    nowPressBtn = null;
                }
                break;
        }
    }

    private void OnPressStart()
    {
        // Chuyển sang GameScene
        SceneManager.LoadScene("GameScene");
    }
}
