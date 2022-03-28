using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    GameData JsonData;
    private GameManager gm;
    private AudioSource snd;
    UIManager UIMgr;
    ObjectManager ObjectMgr;
    Transform ClearUI;
    GameObject ParticleClone;
    SpriteRenderer SRenderer;
    Transform GenderBtn, LeftBtn, RightBtn, UpBtn, DownBtn;
    CanvasGroup GenderCG, LeftCG, RightCG, UpCG, DownCG;
    public static bool isGoal = false;
    public static int keyCount = 0;
    public Sprite[] doorSprite;
    float PosX, PosY;

    void Awake()
    {
        GenderBtn = GameObject.Find("GenderButton").GetComponent<Transform>();
        LeftBtn = GameObject.Find("LeftButton").GetComponent<Transform>();
        RightBtn = GameObject.Find("RightButton").GetComponent<Transform>();
        UpBtn = GameObject.Find("UpButton").GetComponent<Transform>();
        DownBtn = GameObject.Find("DownButton").GetComponent<Transform>();

        GenderCG = GenderBtn.GetComponent<CanvasGroup>();
        LeftCG = LeftBtn.GetComponent<CanvasGroup>();
        RightCG = RightBtn.GetComponent<CanvasGroup>();
        UpCG = UpBtn.GetComponent<CanvasGroup>();
        DownCG = DownBtn.GetComponent<CanvasGroup>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        snd = GameObject.Find("GameManager").GetComponent<AudioSource>();

        UIMgr = GameObject.Find("Canvas").GetComponent<UIManager>();
        ObjectMgr = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        ClearUI = GameObject.Find("Canvas").transform.Find("ClearPanel");

        SRenderer = GetComponent<SpriteRenderer>();
        keyCount = 0;

        PosX = transform.position.x + 8.5f;
        PosY = 4.5f - transform.position.y;

        JsonData = DataController.Instance.gameData;
    }

    void Update()
    {
        //Debug.Log(keyCount);
        if (keyCount >= 1)
            SRenderer.sprite = doorSprite[0]; // Locked
        else
            SRenderer.sprite = doorSprite[1]; // UnLocked
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.tag == "Player")
        {
            if (keyCount >= 1) return;
            UIMgr.isClear = true;
            if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            {
                if (JsonData.Stage + 1 > JsonData.StageRecord)
                    JsonData.StageRecord = JsonData.Stage + 1;
            }
            else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                if (JsonData.HardStage + 1 > JsonData.StageHardRecord)
                    JsonData.StageHardRecord = JsonData.HardStage + 1;
                JsonData.Score += (UIMgr.minute * 60 + UIMgr.second);
                HighScore();
            }
            RemoveEnemy();
            DontMove();
            //PlayerPrefs.SetInt("Score", GameManager.SumScore);
            StartCoroutine(ShowClearPanel());
        }
    }

    void RemoveEnemy()
    {
        int enemyCount = GameObject.Find("Emeries").transform.childCount;

        for (int i = 0; i < enemyCount; i++)
        {
            Transform enemyObj = GameObject.Find("Emeries").transform.GetChild(i);
            if (JsonData.isEffect)
            {
                ParticleClone = ObjectMgr.MakeObj("CircleE");
                ParticleClone.SetActive(true);
                ParticleClone.transform.position = enemyObj.transform.position;
            }
            enemyObj.gameObject.SetActive(false);
        }
    }

    void DontMove()
    {
        GenderCG.blocksRaycasts = false;
        LeftCG.blocksRaycasts = false;
        RightCG.blocksRaycasts = false;
        UpCG.blocksRaycasts = false;
        DownCG.blocksRaycasts = false;
    }

    IEnumerator ShowClearPanel()
    {
        yield return new WaitForSeconds(1.0f);
        if (JsonData.isSound)
            snd.PlayOneShot(gm.clearSnd, 1.0f);
        ClearUI.gameObject.SetActive(true);
        if (SceneManager.GetActiveScene().name == "PuzBuzGame")
        {
            ClearUI.transform.Find("ScoreImage").gameObject.SetActive(false); // 점수는 하드코어 모드에서만 표기
            ClearUI.transform.GetChild(0).transform.Find("UI").gameObject.SetActive(false);
            ClearUI.transform.GetChild(0).transform.Find("LifeImage").gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
        {
            Text ScoreDataText = GameObject.Find("CurrentScoreDataText").GetComponent<Text>();
            ScoreDataText.text = string.Format("{0:#,###0}", JsonData.HighScore);
            Text lifeText = GameObject.Find("LifeImageText").GetComponent<Text>();
            lifeText.text = "× " + JsonData.Life;
        }
    }

    void HighScore()
    {
        if (JsonData.Score > JsonData.HighScore)
            JsonData.HighScore = JsonData.Score;
    }
}
