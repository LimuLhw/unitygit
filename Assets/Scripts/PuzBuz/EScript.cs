using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EScript : MonoBehaviour
{
    enum moveType { UP = 0, DOWN, LEFT, RIGHT, STAY = -1 }
    float PosX, PosY;
    public int intPosX, intPosY;
    public bool isMale = false; // 몬스터 성별 여부
    public bool isDouble = false; // 두 칸 짜리 몬스터 여부
    int EnemyGender = 1;
    [SerializeField]
    int EnemyNum;
    int[] BFS_Count = new int[4];
    [SerializeField]
    int GoToEnemyVecType = 0;
    public int dist = 0;
    public bool isEnemyChoice = false;
    public bool isEnemyMove = false;
    List<int> listVec = new List<int>();
    Queue<int> queX = new Queue<int>();
    Queue<int> queY = new Queue<int>();
    Queue<int> queCnt = new Queue<int>();
    public int[,] BFS_Array = new int[7, 10];
    MapSetting mapSetting;
    void Awake()
    {
        mapSetting = GameObject.Find("GameManager").GetComponent<MapSetting>();
    }
    void Start()
    {
        PosX = transform.position.x + 8.5f;
        PosY = 4.5f - transform.position.y;
        intPosX = (int)PosX;
        intPosY = (int)PosY;

        if (isMale)
        {
            if (!isDouble) // M1
            {
                EnemyNum = 4;
                EnemyGender = -1;
            }
            else // M2
            {
                EnemyNum = 6;
                EnemyGender = -2;
            }
        }
        else
        {
            if (!isDouble) // F1
            {
                EnemyNum = 3;
                EnemyGender = 1;
            }
            else // F2
            {
                EnemyNum = 5;
                EnemyGender = 2;
            }
        }
    }
    void Update()
    {
        //if (!MovingScript.isPlayerDie) // 플레이어가 게임오버된 상태가 아니면
        //{
            //ChoiceBFS();
        //}
        if (isEnemyMove && ItemManager.potionMove <= 0)
        {
            StartCoroutine(MoveEnemy());
            isEnemyMove = false;
        }
    }

    IEnumerator MoveEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        GoToEnemyVecType = SearchPlayer();
        switch (GoToEnemyVecType)
        {
            case (int)moveType.UP:
                if (intPosY - 1 * MovingScript.PlayerGender * EnemyGender < 0 ||
                    intPosY - 1 * MovingScript.PlayerGender * EnemyGender >= MapSetting.mapSet.GetLength(0) ||
                    MapSetting.mapSet[intPosY - 1 * MovingScript.PlayerGender * EnemyGender, intPosX] == 1 ||
                    MapSetting.mapSet[intPosY - 1 * MovingScript.PlayerGender * EnemyGender, intPosX] >= 3) break;

                else if (isDouble &&
                    Mathf.Abs(intPosY - MovingScript.intPosY) <= 1 &&
                    MovingScript.PlayerGender * EnemyGender >= 1) break;

                else if (MapSetting.mapSet[intPosY, intPosX] == 2) break;

                MapSetting.mapSet[intPosY, intPosX] = 0;
                MapSetting.mapSet[intPosY - 1 * MovingScript.PlayerGender * EnemyGender, intPosX] = 3;

                intPosY -= 1 * MovingScript.PlayerGender * EnemyGender;
                transform.position += Vector3.up * MovingScript.PlayerGender * EnemyGender;
                break;
            case (int)moveType.DOWN:
                if (intPosY + 1 * MovingScript.PlayerGender * EnemyGender < 0 ||
                    intPosY + 1 * MovingScript.PlayerGender * EnemyGender >= MapSetting.mapSet.GetLength(0) ||
                    MapSetting.mapSet[intPosY + 1 * MovingScript.PlayerGender * EnemyGender, intPosX] == 1 ||
                    MapSetting.mapSet[intPosY + 1 * MovingScript.PlayerGender * EnemyGender, intPosX] >= 3) break;

                else if (isDouble &&
                    Mathf.Abs(intPosY - MovingScript.intPosY) <= 1 &&
                    MovingScript.PlayerGender * EnemyGender >= -1) break;

                else if (MapSetting.mapSet[intPosY, intPosX] == 2) break;

                MapSetting.mapSet[intPosY, intPosX] = 0;
                MapSetting.mapSet[intPosY + 1 * MovingScript.PlayerGender * EnemyGender, intPosX] = 3;

                intPosY += 1 * MovingScript.PlayerGender * EnemyGender;
                transform.position += Vector3.down * MovingScript.PlayerGender * EnemyGender;
                break;
            case (int)moveType.LEFT:
                if (intPosX - 1 * MovingScript.PlayerGender * EnemyGender < 0 ||
                    intPosX - 1 * MovingScript.PlayerGender * EnemyGender >= MapSetting.mapSet.GetLength(1) ||
                    MapSetting.mapSet[intPosY, intPosX - 1 * MovingScript.PlayerGender * EnemyGender] == 1 ||
                    MapSetting.mapSet[intPosY, intPosX - 1 * MovingScript.PlayerGender * EnemyGender] >= 3) break;

                else if (isDouble &&
                    Mathf.Abs(intPosX - MovingScript.intPosX) <= 1 &&
                    MovingScript.PlayerGender * EnemyGender >= -1) break;

                else if (MapSetting.mapSet[intPosY, intPosX] == 2) break;

                MapSetting.mapSet[intPosY, intPosX] = 0;
                MapSetting.mapSet[intPosY, intPosX - 1 * MovingScript.PlayerGender * EnemyGender] = 3;

                intPosX -= 1 * MovingScript.PlayerGender * EnemyGender;
                transform.position += Vector3.left * MovingScript.PlayerGender * EnemyGender;
                break;
            case (int)moveType.RIGHT:
                if (intPosX + 1 * MovingScript.PlayerGender * EnemyGender < 0 ||
                    intPosX + 1 * MovingScript.PlayerGender * EnemyGender >= MapSetting.mapSet.GetLength(1) ||
                    MapSetting.mapSet[intPosY, intPosX + 1 * MovingScript.PlayerGender * EnemyGender] == 1 ||
                    MapSetting.mapSet[intPosY, intPosX + 1 * MovingScript.PlayerGender * EnemyGender] >= 3) break;

                else if (isDouble &&
                    Mathf.Abs(intPosX - MovingScript.intPosX) <= 1 &&
                    MovingScript.PlayerGender * EnemyGender >= -1) break;

                else if (MapSetting.mapSet[intPosY, intPosX] == 2) break;

                MapSetting.mapSet[intPosY, intPosX] = 0;
                MapSetting.mapSet[intPosY, intPosX + 1 * MovingScript.PlayerGender * EnemyGender] = 3;

                intPosX += 1 * MovingScript.PlayerGender * EnemyGender;
                transform.position += Vector3.right * MovingScript.PlayerGender * EnemyGender;
                break;
            case (int)moveType.STAY:
                break;
        }
    }

    int SearchPlayer()
    {
        for (int i = 0; i < 4; i++) // Enemy
        {
            int x = intPosX;
            int y = intPosY;
            while (true)
            {
                //if (MapSetting.mapSet[y, x] == 1) break;
                if (MapSetting.mapSet[y, x] == 2) return i;

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
        return -1;
    }
}
