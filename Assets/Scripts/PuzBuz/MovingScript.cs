using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovingScript : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;
    private AudioSource snd;
    public static int PlayerGender = 1; // 1: 남자, -1: 여자
    public static int ShoesMove = 1; // 1: Normal, 2: Spring
    float PosX, PosY;
    public static int intPosX, intPosY;
    Camera camera;
    [SerializeField]
    private float speed = 1f;
    private Rigidbody2D rigid;
    private float vel = 1f;
    public char keyCode = 'D';
    GameObject Block;
    MapSetting mapSetting;
    UIManager UIMgr;
    List<GameObject> TargetEnemyList;
    public static bool isPlayerDie = false;
    private bool isDeathSceneOpen = false;
    private float MoveCoolTime = 0.0f;
    public static float DelayTime = 0.0f;
    public Sprite[] playerTexture;
    private SpriteRenderer SRenderer;
    private Animator PlayerAni;
    Text lifeText;
    bool isChangeUILifeText = false;
    CanvasGroup TopBtn;
    Transform childObj;
    SpriteRenderer childSR;
    GameData JsonData;

    void Awake() {
        PlayerGender = 1;
        ShoesMove = 1;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        snd = GameObject.Find("GameManager").GetComponent<AudioSource>();
        Block = GameObject.Find("block(Clone)");
        rigid = GetComponent<Rigidbody2D>();
        mapSetting = GameObject.Find("GameManager").GetComponent<MapSetting>();
        UIMgr = GameObject.Find("Canvas").GetComponent<UIManager>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        SRenderer = GetComponent<SpriteRenderer>();
        PlayerAni = GetComponent<Animator>();
        TopBtn = GameObject.Find("TopButton").GetComponent<CanvasGroup>();
        childObj = transform.GetChild(0);
        childSR = childObj.GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
        {
            lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        }

        TargetEnemyList = new List<GameObject>();
        PosX = transform.position.x + 8.5f;
        PosY = 4.5f - transform.position.y;
        intPosX = (int)PosX;
        intPosY = (int)PosY;
        MoveCoolTime = Time.time;
        TopBtn.blocksRaycasts = true;
        //camera.transform.position = new Vector3(-1, 1, -10);
        CameraMove();

        JsonData = DataController.Instance.gameData;
        isPlayerDie = false;
    }

    void Start()
    {
        if (GameManager.isRestart)
        {
            GameManager.isRestart = false;
            if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                childObj.gameObject.SetActive(true);
                StartCoroutine(DeleteBrokenHeartImg());
            }
        }
    }

    IEnumerator DeleteBrokenHeartImg()
    {
        yield return new WaitForSeconds(0.5f);
        childObj.gameObject.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            lifeText.text = JsonData.Life.ToString();
        
        PlayerMove();

        if (!isPlayerDie) // 플레이어가 생존할 때 타이머 조정
            TimeOut();
    }
    void PlayerMove()
    {
        if (UIMgr.isSpaceKey)
        {
            ++GameManager.Instance.MoveCount;
            --ItemManager.potionMove;
            PlayerGender *= -1;
            if (PlayerGender == 1) // Debug.Log("남자");
                PlayerAni.SetInteger("GenderType", 1);
            //SRenderer.sprite = playerTexture[0];
            else if (PlayerGender == -1) // Debug.Log("여자");
                PlayerAni.SetInteger("GenderType", -1);
            //SRenderer.sprite = playerTexture[1];
            if(JsonData.isSound)
                snd.PlayOneShot(gm.change, 1.0f);
            EnemyMoveSetActive();
            if (isPlayerDie) return;

            if (DelayTime >= 0.2f)
                MoveCoolTime = Time.time;
            UIMgr.isSpaceKey = false;
        }

        if (UIMgr.isRightKey) // 오른쪽
        {
            ++GameManager.Instance.MoveCount;
            --ItemManager.potionMove;
            if(JsonData.isSound)
                snd.PlayOneShot(gm.move, 1.0f);
            keyCode = 'D';
            if (intPosX + ShoesMove < MapSetting.mapSet.GetLength(1) &&
                MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX + ShoesMove] != 1) // 벽으로 막혀있지 않을 때
            {
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX + ShoesMove] >= 3 &&
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX + ShoesMove] <= 6 &&
                    ItemManager.potionMove > 0) return; // 포션 효력이 발동된 상태에서는 적과 겹칠 수 없음

                transform.Translate(Vector2.right * ShoesMove);
                intPosX += ShoesMove;
                if (isDie()) return;
                EnemyMoveSetActive();
                if(MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] = 0;
                if(MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX + ShoesMove] != 12)
                   MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX + ShoesMove] = 2;
                mapSetting.PosX += ShoesMove;
            }
            else
            {
                EnemyMoveSetActive();
                if (isDie()) return;
            }

            if (DelayTime >= 0.2f)
                MoveCoolTime = Time.time;
            UIMgr.isRightKey = false;
        }
        if (UIMgr.isLeftKey) // 왼쪽
        {
            ++GameManager.Instance.MoveCount;
            --ItemManager.potionMove;
            if (JsonData.isSound)
                snd.PlayOneShot(gm.move, 1.0f);
            keyCode = 'A';
            if (intPosX - ShoesMove >= 0 &&
                MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX - ShoesMove] != 1) // 벽으로 막혀있지 않을 때
            {
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX - ShoesMove] >= 3 &&
                MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX - ShoesMove] <= 6 &&
                ItemManager.potionMove > 0) return; // 포션 효력이 발동된 상태에서는 적과 겹칠 수 없음

                transform.Translate(Vector2.left * ShoesMove);
                intPosX -= ShoesMove;
                if (isDie()) return;
                EnemyMoveSetActive();
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] = 0;
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX - ShoesMove] != 12)
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX - ShoesMove] = 2;
                mapSetting.PosX -= ShoesMove;
            }
            else
            {
                EnemyMoveSetActive();
                if (isDie()) return;
            }

            if (DelayTime >= 0.2f)
                MoveCoolTime = Time.time;
            UIMgr.isLeftKey = false;
        }

        if (UIMgr.isDownKey) // 아래
        {
            ++GameManager.Instance.MoveCount;
            --ItemManager.potionMove;
            if (JsonData.isSound)
                snd.PlayOneShot(gm.move, 1.0f);
            keyCode = 'S';
            if (intPosY + ShoesMove < MapSetting.mapSet.GetLength(0) &&
                MapSetting.mapSet[mapSetting.PosY + ShoesMove, mapSetting.PosX] != 1) // 벽으로 막혀있지 않을 때
            {
                if (MapSetting.mapSet[mapSetting.PosY + ShoesMove, mapSetting.PosX] >= 3 &&
                MapSetting.mapSet[mapSetting.PosY + ShoesMove, mapSetting.PosX] <= 6 &&
                ItemManager.potionMove > 0) return; // 포션 효력이 발동된 상태에서는 적과 겹칠 수 없음

                transform.Translate(Vector2.down * ShoesMove);
                intPosY += ShoesMove;
                if (isDie()) return;
                EnemyMoveSetActive();
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] = 0;
                if (MapSetting.mapSet[mapSetting.PosY + ShoesMove, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY + ShoesMove, mapSetting.PosX] = 2;
                mapSetting.PosY += ShoesMove;
            }
            else
            {
                EnemyMoveSetActive();
                if (isDie()) return;
            }

            if (DelayTime >= 0.2f)
                MoveCoolTime = Time.time;
            UIMgr.isDownKey = false;
        }

        if (UIMgr.isUpKey) // 위
        {
            ++GameManager.Instance.MoveCount;
            --ItemManager.potionMove;
            if (JsonData.isSound)
                snd.PlayOneShot(gm.move, 1.0f);
            keyCode = 'W';
            if (intPosY - ShoesMove >= 0 &&
                MapSetting.mapSet[mapSetting.PosY - ShoesMove, mapSetting.PosX] != 1) // 벽으로 막혀있지 않을 때
            {
                if (MapSetting.mapSet[mapSetting.PosY - ShoesMove, mapSetting.PosX] >= 3 &&
                    MapSetting.mapSet[mapSetting.PosY - ShoesMove, mapSetting.PosX] <= 6 &&
                    ItemManager.potionMove > 0) return; // 포션 효력이 발동된 상태에서는 적과 겹칠 수 없음

                transform.Translate(Vector2.up * ShoesMove);
                intPosY -= ShoesMove;
                if (isDie()) return;
                EnemyMoveSetActive();
                if (MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY, mapSetting.PosX] = 0;
                if (MapSetting.mapSet[mapSetting.PosY - ShoesMove, mapSetting.PosX] != 12)
                    MapSetting.mapSet[mapSetting.PosY - ShoesMove, mapSetting.PosX] = 2;
                mapSetting.PosY -= ShoesMove;
            }
            else
            {
                EnemyMoveSetActive();
                if (isDie()) return;
            }

            if (DelayTime >= 0.2f)
                MoveCoolTime = Time.time;
            UIMgr.isUpKey = false;
        }

        //Debug.Log("(" + mapSetting.PosX + ", " + mapSetting.PosY + ")");
        //플레이어 사망시 구문 중단
        if (isPlayerDie) {
          StartCoroutine(DeathScene());
        }

        if (GoalScript.isGoal) // 골인지점에 도착하면 이동하지 않도록 방지
        {
            Debug.Log("Game Clear!");
            if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            {
                JsonData.Stage += 1;
                SceneManager.LoadScene("PuzBuzGame");
            }
            else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                JsonData.HardStage += 1;
                SceneManager.LoadScene("PuzBuzHardMode");
            }
            GoalScript.isGoal = false;
            return;
        }

        DelayTime = Time.time - MoveCoolTime;
        if (DelayTime < 0.2f) return; // 너무 빠르게 움직일 때 발생하는 버그 방지

        CameraMove();
    }
    IEnumerator DeathScene()
    {
        if (isDeathSceneOpen)
        {
            TopBtn.blocksRaycasts = false;
            PlayerAni.SetTrigger("Die");
            if(JsonData.isVibration)
                Vibration.Vibrate((long)1000);
            Transform deathPanel = GameObject.Find("Canvas").transform.Find("DeathPanel");
            isDeathSceneOpen = false;

            //https://drehzr.tistory.com/751 여기를 참고함. UnityManifest.xml에서 권한을 넣어야함.
            //진동기능은 반드시 모바일 기기에서 테스트 해야함.
            if (Application.platform == RuntimePlatform.Android)
                if (JsonData.isVibration)
                    Vibration.Vibrate((long)1000); //1초동안 진동

            yield return new WaitForSeconds(1.5f);

            if (JsonData.isSound)
                snd.PlayOneShot(gm.failSnd, 0.1f);
            deathPanel.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "PuzBuzGame")
            {
                deathPanel.transform.Find("ScoreImage").gameObject.SetActive(false); // 점수는 하드코어 모드에서만 표기
                deathPanel.transform.GetChild(0).transform.Find("UI").gameObject.SetActive(false);
                deathPanel.transform.GetChild(0).transform.Find("LifeImage").gameObject.SetActive(false);
            }

            else if (SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            {
                Text ScoreDataText = GameObject.Find("CurrentScoreDataText").GetComponent<Text>();
                ScoreDataText.text = string.Format("{0:#,###0}", JsonData.Score);

                JsonData.Life -= 1;
                Text lifeText = GameObject.Find("LifeImageText").GetComponent<Text>();
                lifeText.text = "× " + JsonData.Life.ToString();
            }
            yield return false;

            GameObject ContinueButton = GameObject.Find("ContinueBtn");
            GameObject FreezeButton = GameObject.Find("FreezeBtn");

            if (JsonData.Life <= 0)
            {
                ContinueButton.SetActive(false);
                FreezeButton.SetActive(true);
                // 게임 오버 UI 생성
            }
            else
            {
                ContinueButton.SetActive(true);
                FreezeButton.SetActive(false);
                // 해당 스테이지 위치로 재배치 (LoadScene.this)
            }
            if (isChangeUILifeText)
            {
                //--GameManager.Instance.PB_lifeVal;
                isChangeUILifeText = false;
            }
            
        }

    }
    private void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.tag == "Enemy")
        {
            if (JsonData.isSound)
                snd.PlayOneShot(gm.death, 1.0f);
            Debug.Log("Game Over!");
            isChangeUILifeText = true;
            isPlayerDie = true;
            isDeathSceneOpen = true;
        }
    }

    void EnemyMoveSetActive()
    {
        GameObject Enemy = GameObject.Find("Emeries");
        int ECount = Enemy.transform.childCount;
        for (int i = 0; i < ECount; i++)
        {
            EScript escript = Enemy.transform.GetChild(i).GetComponent<EScript>();
            //Debug.Log("Num: " + (i + 1));
            if (isTargetEnemy(escript.intPosY, escript.intPosX))
                escript.isEnemyMove = true;
        }
    }

    bool isTargetEnemy(int targetY, int targetX)
    {
        for (int i = 0; i < 4; i++) // Player
        {
            int x = intPosX;
            int y = intPosY;
            while (true)
            {
                //Debug.Log("PosX: " + x + ", PosY: " + y + ", EnemyX: " + targetX + ", EnemyY: " + targetY);
                //if (MapSetting.mapSet[y, x] == 1) break;
                if (y == targetY && x == targetX)
                {
                    return true;
                }
                else if (MapSetting.mapSet[y, x] >= 3 && MapSetting.mapSet[y, x] <= 6)
                    break;

                switch (i)
                {
                    case 0: // up
                        y--;
                        break;
                    case 1: // down
                        y++;
                        break;
                    case 2: // left
                        x--;
                        break;
                    case 3: // right
                        x++;
                        break;
                }
                if (x < 0 || x >= MapSetting.mapSet.GetLength(1)) break;
                else if (y < 0 || y >= MapSetting.mapSet.GetLength(0)) break;
            }
        }
        return false;
    }

    void CameraMove()
    {
        //Debug.Log(transform.position.x);
        if (transform.position.x > -5 && transform.position.x < MapSetting.mapSet.GetLength(1) - 13)
            camera.transform.position = new Vector3(transform.position.x, camera.transform.position.y, -10);
        else if (transform.position.x <= -5)
            camera.transform.position = new Vector3(-5, camera.transform.position.y, -10);
        else if (MapSetting.mapSet.GetLength(1) >= 9)
            camera.transform.position = new Vector3(MapSetting.mapSet.GetLength(1) - 13, camera.transform.position.y, -10);
    }

    bool isDie()
    {
        if (MapSetting.mapSet[intPosY, intPosX] >= 3
                && MapSetting.mapSet[intPosY, intPosX] <= 6 && !ItemManager.isItemGet && ItemManager.potionMove > 0)
        {
            if (JsonData.isSound)
                snd.PlayOneShot(gm.death, 1.0f);
            Debug.Log("Game Over");
            isPlayerDie = true;
            
            return true;
        }
        if (ItemManager.isItemGet)
            ItemManager.isItemGet = false;
        return false;
    }

    void TimeOut()
    {
        if (UIMgr.delT <= 0)
        {
            Debug.Log("Game Over");
            isChangeUILifeText = true;
            isPlayerDie = true;
            isDeathSceneOpen = true;
        }
    }
}
