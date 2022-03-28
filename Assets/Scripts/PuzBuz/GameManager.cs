using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { INTRO, MAIN_MENU, PUZBUZ, POSHONG };
    //public static int PB_lifeVal = 3;
    public static int StageNumber;
    //public static int SumScore = 0;
    public static bool isRestart = false;
    public static bool isBlackPanel = true;
    public int MoveCount = 0;
    private float moveTime = 0.0f;
    private bool isMainBtnClick = false;
    public static bool startFadeIn = true;
    private AudioSource gameSound;
    Transform blackPanel;
    Image blackPanelImg;
    GameData JsonData;

    public GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                //_instance = new GameManager();
                //DontDestroyOnLoad(_instance);
            }
            return _instance;
        }

    }
    private static GameManager _instance;

    //Sounds           이동    교체      사망     아이템   실패     클리어
    public AudioClip move, change, death, getItem, failSnd, clearSnd;

    void Awake()
    {
        gameSound = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            blackPanel = GameObject.Find("Canvas").transform.GetChild(23).GetComponent<Transform>();
        else
            blackPanel = GameObject.Find("Canvas").transform.GetChild(18).GetComponent<Transform>();
        blackPanelImg = blackPanel.GetComponent<Image>();

        JsonData = DataController.Instance.gameData;
    }

    void Update()
    {
        if (startFadeIn && !isMainBtnClick)
        {
            blackPanel.gameObject.SetActive(true);
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, 1 - moveTime * 2);

            if (moveTime >= 0.5f)
            {
                startFadeIn = false;
                moveTime = 0.0f;
                blackPanel.gameObject.SetActive(false);
            }
        }

        if (isMainBtnClick)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                moveTime = 0.5f;
                SceneManager.LoadScene("Title");
            }
        }
    }

    public void GotoMain()
    {
        //SceneManager.UnloadScene("PuzBuzGame");
        Time.timeScale = 1f;
        MovingScript.isPlayerDie = false;
        blackPanel.gameObject.SetActive(true);
        moveTime = 0.0f;
        isMainBtnClick = true;
        startFadeIn = true;

        if (JsonData.Life <= 0)
        {
            if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                JsonData.HardStage = 1;
                JsonData.StageHardRecord = 1;
                JsonData.Life = 1;
                JsonData.Score = 0;
            }
        }
    }
}
