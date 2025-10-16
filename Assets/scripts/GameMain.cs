using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameMain : MonoBehaviour
{

    public GameObject bird;
    public GameObject readyPic;
    public GameObject tipPic;
    public GameObject scoreMgr;
    public GameObject pipeSpawner;
    //   public GameObject gameoverPic;

    private bool gameStarted = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Không xử lý input ở đây nữa - để BirdControl xử lý
    }


    public void StartGame()
    {
        BirdControl control = bird.GetComponent<BirdControl>();
        control.inGame = true;
        control.JumpUp();

        readyPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
        tipPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);

        pipeSpawner.GetComponent<PipeSpawner>().StartSpawning();
    }
    
    public void HideUI()
    {
        if (readyPic != null)
            readyPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
        if (tipPic != null)
            tipPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
    }
    public void GameOver()
    {
        // Hiện ảnh Game Over
        //  gameoverPic.GetComponent<SpriteRenderer>().enabled = true;

        // Ẩn điểm số và dừng ống bay (nếu muốn)
        pipeSpawner.GetComponent<PipeSpawner>().GameOver();
    }
    
    public void RestartGame()
    {
        gameStarted = false;
        
        // Hiện lại readyPic và tipPic
        if (readyPic != null)
        {
            readyPic.GetComponent<SpriteRenderer>().material.DOFade(1f, 0.2f);
        }
        if (tipPic != null)
        {
            tipPic.GetComponent<SpriteRenderer>().material.DOFade(1f, 0.2f);
        }
    }

}
