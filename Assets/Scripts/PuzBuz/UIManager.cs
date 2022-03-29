using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /*
     점수:
    금괴 아이템 먹으면 +50 점수
    스테이지 클리어 하면 남은 시간 초만큼 점수 획득
    Ex) 60초에 클리어 -> 60점 획득.
     */

    GameData JsonData;
    int stage = 1;
    public int minute = 0;
    public int second = 0;
    public float delT;
    public int scoreInt = 0;
    public bool isClear = false;
    public static bool GetPoison = false;
    bool isPause = false;
    Text TimeUI;
    Text ScoreUI;
    Text StageText;
    Text MoveText;
    Image RetryImg;
    CanvasGroup CtrlCG;
    RectTransform BottomTr;
    public Text LifeText;

    public bool isSpaceKey = false;
    public bool isRightKey = false;
    public bool isLeftKey = false;
    public bool isDownKey = false;
    public bool isUpKey = false;

    void Awake()
    {
        //scoreInt += PlayerPrefs.GetInt("Score", 0);
        if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
        {
            ScoreUI = GameObject.Find("ScoreText").GetComponent<Text>();
        }
        TimeUI = GameObject.Find("TimeText").GetComponent<Text>();
        CtrlCG = GameObject.Find("CtrlButton").GetComponent<CanvasGroup>();
        BottomTr = GameObject.Find("BottomUI").GetComponent<RectTransform>();
        StageText = GameObject.Find("StageText").GetComponent<Text>();
        MoveText = GameObject.Find("MoveText").GetComponent<Text>();
        RetryImg = GameObject.Find("Retry").GetComponent<Image>();
        SetTime();

        GetPoison = false;

        JsonData = DataController.Instance.gameData;
    }

    void Start()
    {
        //Vector3 v = Camera.main.ScreenToWorldPoint(BottomTr.position);
        //Vector3 v = new Vector3(0, 390 - 140 * (10 - MapSetting.mapSet.GetLength(0)), 0);
        isClear = false;
        BottomTr.anchoredPosition = new Vector3(0, 165 + 140 * (10 - MapSetting.mapSet.GetLength(0)), 0);
        //StageText.text = "STAGE " + GameManager.StageNumber;
        if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            StageText.text = "STAGE " + JsonData.Stage.ToString();
        else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            StageText.text = "STAGE " + JsonData.HardStage.ToString();
    }

    void SetTime()
    {
        if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            delT = 180f;
        else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            delT = 90f;
    }

    void Update()
    {
        MoveText.text = (GameManager.Instance.MoveCount).ToString();
        if (!MovingScript.isPlayerDie && !isClear) // 플레이어가 Die상태가 아니거나 클리어 상태가 아니어야 함
            Timer();
        Score();

        if (JsonData.Life <= 1 && SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            RetryImg.color = new Color32(138, 138, 138, 255);
        else
            RetryImg.color = Color.white;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseBtn();
        }
    }

    void Score()
    {
        //ScoreUI.text = GameManager.SumScore.ToString("00000");
        if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            ScoreUI.text = string.Format("{0:#,###0}", JsonData.Score);
    }

    void Timer()
    {
        delT -= Time.deltaTime;
        second = (int)(delT % 60);
        minute = (int)(delT / 60);

        TimeUI.text = minute.ToString() + ":" + second.ToString("00");
        if (delT <= 0)
            delT = 0;
    }

    public void LButton()
    {
        if (MovingScript.DelayTime >= 0.2f)
        {
            if (JsonData.isVibration)
                Vibration.Vibrate((long)100);

            if (GetPoison)
                isRightKey = true;
            else
                isLeftKey = true;
        }
    }

    public void RButton()
    {
        if (MovingScript.DelayTime >= 0.2f)
        {
            if (JsonData.isVibration)
                Vibration.Vibrate((long)100);

            if (GetPoison)
                isLeftKey = true;
            else
                isRightKey = true;
        }
    }

    public void UButton()
    {
        if (MovingScript.DelayTime >= 0.2f)
        {
            if (JsonData.isVibration)
                Vibration.Vibrate((long)100);

            if (GetPoison)
                isDownKey = true;
            else
                isUpKey = true;
        }
    }

    public void DButton()
    {
        if (MovingScript.DelayTime >= 0.2f)
        {
            if (JsonData.isVibration)
                Vibration.Vibrate((long)100);

            if (GetPoison)
                isUpKey = true;
            else
                isDownKey = true;
        }
    }

    public void SButton()
    {
        if (JsonData.isVibration)
            Vibration.Vibrate((long)100);

        if (MovingScript.DelayTime >= 0.2f)
            isSpaceKey = true;
    }

    public void RetryBtn()
    {
        transform.Find("DeathPanel").gameObject.SetActive(false);
        if ((JsonData.Life >= 2 || SceneManager.GetActiveScene().name == "PuzBuzGame") && !MovingScript.isPlayerDie) { //인게임 에서
            if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            {
                SceneManager.LoadScene("PuzBuzGame");
            }
            else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                JsonData.Life -= 1;
                SceneManager.LoadScene("PuzBuzHardMode");
            }
            GameManager.isRestart = true;
           
        }
        //Debug.Log(GameManager.PB_lifeVal);
        else if(JsonData.Life >= 1 && MovingScript.isPlayerDie)    //스테이지 실패 UI에서
        {
            MovingScript.isPlayerDie = false;
            //GameManager.Instance.PB_lifeVal--;
            if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            {
                SceneManager.LoadScene("PuzBuzGame");
            }
            else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                SceneManager.LoadScene("PuzBuzHardMode");
            }
        }
    }

    public void PauseBtn()
    {
        if (MovingScript.isPlayerDie) return;
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            CtrlCG.blocksRaycasts = false;
            transform.Find("PausePanel").gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            CtrlCG.blocksRaycasts = true;
            transform.Find("PausePanel").gameObject.SetActive(false);
        }
    }

    public void NextStageBtn()
    {
        GoalScript.isGoal = true;
    }
}
