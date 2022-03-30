using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSetting : MonoBehaviour
{
    enum mapType {Wall = 1, Player, EnemyF1, EnemyM1, EnemyF2, EnemyM2, Bomb, Time, Potion, Gold,
        Life, Goal, Key, Poison, ShoesN, ShoesS}

    public GameObject[] bg = new GameObject[2];
    public GameObject[] block = new GameObject[2];
    public GameObject player;
    public GameObject enemyF1;
    public GameObject enemyM1;
    public GameObject enemyF2;
    public GameObject enemyM2;
    public GameObject bomb;
    public GameObject timeItem;
    public GameObject potion;
    public GameObject gold;
    public GameObject life;
    public GameObject goal;
    public GameObject key;
    public GameObject poison;
    public GameObject shoesN;
    public GameObject shoesS;

    public int PosX;
    public int PosY;
    private GameObject MapPosition;

    public static bool isResume = false;
    GameData JsonData;

    // 맵 배치 씬
    // 0: 빈칸, 1: 벽, 2: 플레이어, 3: F몬스터(1), 4: M몬스터(1), 5: F몬스터(2), 6: M몬스터(2),
    // 7: 폭탄, 8: 시간아이템, 9: 포션, 10: 골드, 11: 생명, 12: 골인...
    public static int[,] mapSet;
    /* int[,] mapSet1 ={ //맵 제작 참고용
            {2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,12}
    };*/

    int[,] mapSet1 ={   //By Lhw
            {2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,0 ,0 ,0 ,10 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,4 ,0 ,0},
            {0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,12},
            {4 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,10 ,0 ,0},
            {0 ,4 ,0 ,0 ,0 ,1 ,10 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0}
    };
    int[,] mapSet2 ={ //by Lhw
       {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,10,0 ,4 ,0 ,0 ,0 ,12},
       {0 ,0 ,4 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
       {0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,0},
       {0 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,1 ,0 ,0 ,4 ,0 ,0 ,0},
       {0 ,0 ,0 ,10,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
       {0 ,0 ,3 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,4 ,0 ,0 ,1 ,0},
       {0 ,0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
       {0 ,0 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,10,0 ,0},
       {0 ,0 ,1 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,1 ,0 ,0 ,3 ,0 ,0},
       {2 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0}
    };
    int[,] mapSet3 =
    {
       {12,1,10 ,0, 0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0},
       {0 ,1 ,1 ,0, 1 ,0 ,1 ,0 ,1 ,0 ,3 ,7 ,1 ,1 ,1 ,0},
       {0 ,0 ,1 ,0, 1 ,0 ,1 ,0 ,0 ,1 ,4 ,1 ,0 ,0 ,0 ,0},
       {1 ,0 ,0 ,0, 1 ,0 ,0 ,1 ,0 ,0 ,1 ,0 ,0 ,1 ,1 ,1},
       {0 ,1 ,1 ,1, 1 ,1 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,0 ,0},
       {1 ,0 ,0 ,0, 3 ,0 ,1 ,0 ,0 ,9 ,1 ,0 ,0 ,0 ,1 ,0},
       {0 ,0 ,1 ,1, 1 ,0 ,0 ,1 ,4 ,1 ,1 ,1 ,1 ,1 ,1 ,0},
       {0 ,1 ,0 ,0, 0 ,1 ,0 ,1 ,1 ,1 ,1 ,0 ,0 ,0 ,0 ,0},
       {0 ,0 ,0 ,1, 0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,1},
       {0 ,1 ,1 ,3, 1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,2},
    };
    int[,] mapSet4 ={ //맵 제작 참고용
            {1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,0 ,1 ,0},
            {0 ,0 ,4 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,5 ,0 ,0 ,0 ,0},
            {0 ,0 ,0 ,0 ,4 ,0 ,3 ,0 ,1 ,0 ,1 ,0 ,0 ,0 ,0 ,0},
            {0 ,0 ,4 ,0 ,0 ,1 ,0 ,0 ,3 ,0 ,0 ,0 ,6 ,1 ,0 ,0},
            {0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,5 ,0 ,0 ,4 ,0 ,0},
            {0 ,0 ,3 ,1 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,6 ,0 ,0 ,0 ,5},
            {0 ,0 ,0 ,0 ,10,3 ,0 ,0 ,0 ,0 ,0 ,0,6 ,0 ,0 ,0},
            {0 ,3 ,0 ,1 ,1 ,0 ,0 ,0 ,3 ,5 ,0 ,1 ,4 ,0 ,6 ,0},
            {0 ,0 ,4 ,3 ,1 ,0 ,0 ,1 ,0 ,0 ,0 ,5 ,0 ,5 ,0 ,0},
            {2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,12}
    };
    int[,] mapSet5 = {  //by Lhw
              {2 ,0 ,0  ,0  ,0 ,0 ,0 ,0,0,1,0,0,0,0,0,0},
              {1 ,1 ,1  ,10,1,0,1,0,0,0,0,0,0,0,0,0},
              {0 ,4 ,10,3  ,10,0,0,0,0,0,0,0,0,0,0,0},
              {1 ,0 ,1 ,10 ,1,0,1,0,0,0,0,0,1,0,0,0},
              {1 ,0 ,0 ,0  ,0,0,0,0,1,0,0,4,0,0,1,0},
              {0 ,0 ,1 ,0  ,1,0,1,0,0,0,0,4,0,0,0,0},
              {1 ,0 ,0 ,0  ,0,0,10,1,0,0,0,0,0,0,0,0},
              {0 ,1 ,0 ,0  ,0,0,0,0,0,0,0,0,0,3,10,1},
              {0 ,0 ,1 ,0  ,0,0,3,0,0,1,3,0,4,10,0,3},
              {8 ,0 ,0 ,0  ,0,0,1,0,0,0,0,0,0,1,4,12}
            };
    int[,] mapSet6 = { //by 권오형
            {1 ,1 ,1  ,0  ,1 ,0 ,1 ,0,1,0,1,0,1,1,1,0},
            {1 ,1 ,1  ,10,1,0,1,0,0,0,0,0,1,0,1,0},
            {0 ,4 ,10,3  ,10,0,1,1,0,0,1,0,0,9,0,0},
            {1 ,0 ,1 ,9 ,1,0,1,0,0,0,1,0,1,1,0,0},
            {0 ,2 ,0 ,0  ,0,0,0,0,1,1,0,4,0,1,0,1},
            {0 ,0 ,1 ,0  ,1,0,0,0,0,1,0,4,0,0,0,0},
            {1 ,0 ,0 ,0  ,1,0,10,1,0,0,0,0,0,0,1,12},
            {0 ,1 ,0 ,0  ,1,0,0,0,0,1,1,0,0,0,0,0},
            {1 ,1 ,1 ,0  ,1,0,3,0,0,1,3,1,4,3,0,0},
            {8 ,0 ,0 ,0  ,0,0,1,0,3,0,0,0,0,1,4,0}
        };

    int[,] mapSet7 =
        {
        {4,0,0,0 ,0,1,1,0,0,4,4,1,4,3,0,0},
        {4,0,1,3 ,1,0,0,0,0,0,0,1,1,0,0,0},
        {4,0,3,4 ,3,0,0,0,1,0,0,0,0,0,0,0},
        {0,0,1,1 ,1,1,0,1,1,0,0,0,0,0,0,0},
        {1,4,1,12,1,3,0,0,1,0,0,1,8,0,0,0},
        {1,0,1,3 ,4,1,4,0,1,0,0,8,1,0,0,0},
        {1,0,0,0 ,1,1,1,3,1,0,0,0,0,0,0,0},
        {0,0,0,3 ,4,0,0,0,0,0,0,0,0,0,0,0},
        {3,0,1,1 ,4,1,1,0,0,0,0,1,1,0,0,0},
        {4,3,0,0 ,0,0,0,0,0,3,3,1,3,4,1,2}
        };
    int[,] mapSet8 =
           {{1,0,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,2,1,0,5,0,0,1},
            {1,0,0,0,0,7,0,0,0,0,1},
            {1,0,0,1,0,0,0,0,0,4,1},
            {1,6,0,0,0,0,0,0,4,0,1},
            {1,0,0,0,0,0,1,0,0,0,1},
            {1,0,0,5,3,0,0,6,0,0,1},
            {1,0,0,0,0,0,0,0,0,12,0},
            {1,0,1,1,1,1,1,1,1,1,1}};
    int[,] mapSet9 =    //By Lhw (오우 왜이리 어려워)
    {
      {13,0,0,0,6,1,2,1,5,0,0,0,6,0,13},
      {0 ,1,0,1,0,1,0,1,0,1,0,1,0,1 ,0 },
      {0 ,0,0,0,0,0,0,0,0,0,0,0,0,0 ,0 },
      {0 ,1,0,1,0,1,0,1,0,1,0,1,0,1 ,0 },
      {0 ,0,0,0,0,0,0,0,5,0,0,0,0,0 ,0 },
      {0 ,1,0,1,0,1,0,1,0,1,0,1,0,1 ,0 },
      {5 ,0,0,0,6,0,0,0,0,0,0,0,0,0 ,5 },
      {0 ,1,0,1,0,1,0,1,0,1,0,1,0,1 ,0 },
      {0 ,0,6,0,0,0,5,0,0,0,0,0,6,0 ,13},
      {12,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
    };
    int[,] mapSet10 =   //Based on Original Map (Stage 9)
        {
        {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,5,1,0,1,6,1,0,1,5,1,0,1,6,1 },
        {0,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
        {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1 },
        {0,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
        {0,1,6,1,0,1,5,1,0,1,6,1,0,1,5,1 },
        {0,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
        {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1 },
        {0,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
        {0,0,5,0,0,0,6,0,0,0,5,0,0,0,6,12}
    };
    int[,] mapSet11 =
    {
        {0, 0, 1, 0, 0, 0, 0, 0, 4,10, 3, 1, 5, 1, 1, 1},
        {0, 0, 4, 0, 3, 0, 1, 0, 0, 1,10, 0, 6, 1,12, 1},
        {8, 3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,10, 4, 6, 1},
        {3, 0, 0, 0, 1, 0, 1, 6, 0, 0, 1, 0, 0, 1, 0, 0},
        {0, 0, 2, 0, 0, 3, 4, 4, 3, 1, 0, 0, 0, 0, 1, 1},
        {0, 0, 0, 0, 1, 1,10,10, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 0, 0, 1, 0, 3, 0, 0, 0, 0, 1, 6, 0, 0, 1, 4},
        {0, 1, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 0, 0, 1, 0},
        {0, 0, 4, 0, 0, 0, 0, 0, 1, 5, 3, 0, 1, 0, 0, 0},
        {0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 4, 3, 0, 0}
    };
    int[,] mapSet12 =
    {
        { 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
        { 0, 2, 0, 0, 0, 3, 4, 0, 0, 0, 1, 0},
        { 0, 0, 0, 0, 1, 1, 9, 1, 0,10, 3, 0},
        { 1, 0, 0, 0, 4, 1, 0, 3, 1, 4, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 4, 3, 0, 0, 1},
        {10,10,10, 1, 0, 0, 0, 4, 1,10, 0, 0},
        {10, 3,10, 0, 0, 0, 0, 3, 4, 0, 1, 0},
        {10,10,10, 0, 0, 0, 0, 0, 1, 3, 1,12}
    };
    int[,] mapSet13 =
    {
        {0, 0, 0, 1, 4, 8, 4, 0, 3, 1, 0, 0,10, 3, 0, 1},
        {0, 1, 0, 0, 0, 1, 5, 0, 6, 0, 0, 0, 4, 0, 0, 0},
        {0, 3, 0, 0, 0, 0, 0, 0, 7, 5, 0, 0, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 0, 0, 5, 3, 4,10, 3, 6, 0, 0},
        {1, 0, 2, 1, 0, 0, 0, 6, 1, 6, 0, 1,12, 3, 5, 0},
        {0, 0, 1, 0, 0, 0, 0, 6, 0, 4, 5, 1, 4, 4, 6, 7},
        {0, 4, 0, 0, 0, 1, 0, 0, 3, 5, 0,10, 4, 5, 0, 0},
        {0, 1, 0, 0, 0, 5, 0, 0, 7, 6, 0, 0, 1, 0, 0, 0},
        {0, 0, 0, 3, 0, 0, 0, 0, 6, 4, 5, 0, 3, 0, 0, 0},
        {0 ,0, 3, 8, 3, 0, 0, 0, 0, 1, 0, 0,10, 4, 0, 1}
    };
    int[,] mapSet14 =
    {
        {2,0,0,0,0,14,0,0,0},
        {0,0,0,13,0,0,0,14,0},
        {0,0,1,0,0,0,0,0,0},
        {0,13,0,0,0,0,0,0,0},
        {0,0,0,3,12,0,0,0,0},
        {0,15,0,0,0,0,0,16,0},
        {0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,0},
        {0,0,0,0,0,0,1,0,0}
    };
    int[,] mapSet15 ={   //스테이지 18 베이스
            {2 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1},
            {0 ,1 ,6 ,0 ,0 ,1 ,0,10,0 ,1 ,0 ,0 ,0 ,1,12,1},
            {0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,1 ,0 ,1 ,6 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {10,1,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,1 ,0 ,1 ,0 ,1 ,5 ,1 ,0 ,1 ,6 ,1 ,5 ,1 ,0 ,1},
            {0 ,1 ,8 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,6 ,1 ,0 ,1 ,0 ,1 ,6 ,1},
            {0 ,0 ,0 ,1 ,0 ,8 ,0 ,1 ,0 ,9 ,0 ,1 ,8,10 ,0 ,1}
    };
    int[,] mapSet16 ={   //스테이지 20 베이스
            {2 ,0 ,4 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,0 ,0 ,0 ,10},
            {0 ,1 ,5 ,1 ,6 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,0 ,0 ,0 ,4 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,0 ,1 ,5 ,1 ,6 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,0 ,1 ,0 ,1 ,5 ,1 ,6 ,1 ,0 ,1 ,0 ,1 ,0 ,1},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,3 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,5 ,1 ,6 ,1 ,0 ,1 ,0 ,1},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,0 ,3 ,0 ,0 ,0},
            {10 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,5 ,1 ,6 ,1 ,0 ,12}
    };
    int[,] mapSet17 ={   //스테이지 28 베이스
            {2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1,10},
            {0 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,8},
            {0 ,1 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {0 ,1 ,1 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,0 ,0},
            {0 ,1 ,1 ,6 ,5 ,6 ,5 ,6 ,5 ,6 ,5 ,6 ,5 ,6 ,0 ,0},
            {0 ,1 ,1 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,4 ,3 ,0 ,0},
            {8 ,1 ,1,12,5 ,6 ,5 ,6 ,5 ,6 ,5 ,6 ,5 ,6 ,0 ,0}
    };
    int[,] mapSet18 ={   //스테이지 30 베이스
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0},
            {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,0},
            {0 ,0 ,5 ,0 ,0 ,0 ,6 ,0 ,0 ,0 ,5 ,0 ,0 ,0 ,1 ,0},
            {0 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,1 ,0},
            {0 ,1 ,5 ,0 ,6 ,0 ,6 ,0 ,5 ,0 ,5,12,1 ,8 ,1 ,0},
            {8 ,1 ,8 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,1 ,0},
            {0 ,1 ,0 ,0 ,5 ,0 ,0 ,0 ,6 ,0 ,0 ,0 ,0 ,0 ,1 ,0},
            {0 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,0},
            {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0}
    };
    int[,] mapSet19 =
   {
        {16,1, 0, 1, 0, 1, 0, 1, 0, 1, 0,1, 5, 1, 0, 1},
        {2, 0, 1, 0, 1, 0, 1, 5, 1, 0, 1, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6, 1, 5, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 5, 1, 0, 1, 5, 1, 6, 1, 6, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6, 1, 0},
        {0, 1, 0, 1, 0, 1, 6, 1, 5, 1, 0, 1, 0, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 1, 6, 1, 0, 1, 0, 1, 0, 1, 12, 1},
        {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0}
    };
    int[,] mapSet20 ={   //By Lhw
            {2 ,1 ,0 ,0 ,5 ,0 ,0 ,1 ,0 ,1 ,1,0 ,0 ,0 ,1,12},
            {8 ,1 ,0 ,1 ,1 ,1 ,0 ,1 ,0 ,1 ,0 ,0 ,1 ,0 ,1 ,6},
            {0 ,0 ,0 ,0 ,0 ,1 ,5 ,1 ,0 ,1 ,0 ,1 ,6 ,0 ,0 ,0},
            {1 ,1 ,1 ,1 ,0 ,1 ,8 ,1 ,0 ,1 ,0 ,0 ,7, 1 ,1 ,1},
            {1 ,16,0 ,0,0 ,1 ,13,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15},
            {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1  ,1},
            {0,15,1 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,5 ,1 ,0 ,13,16},
            {0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1, 1},
            {0 ,6 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,0 ,0},
            {1 ,1 ,3 ,0 ,0 ,1 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,1 ,1 ,0},
            {13,0,0 ,0 ,1 ,0 ,0 ,16,1,15,1 ,0 ,0 ,0 ,0 ,0}
    };

    void Awake()
    {
        player = Resources.Load<GameObject>("Prefabs/Player");
        enemyF1 = Resources.Load<GameObject>("Prefabs/EnemyF1");
        enemyM1 = Resources.Load<GameObject>("Prefabs/EnemyM1");
        enemyF2 = Resources.Load<GameObject>("Prefabs/EnemyF2");
        enemyM2 = Resources.Load<GameObject>("Prefabs/EnemyM2");
        bomb = Resources.Load<GameObject>("Prefabs/Bomb");
        timeItem = Resources.Load<GameObject>("Prefabs/Time");
        potion = Resources.Load<GameObject>("Prefabs/Potion");
        gold = Resources.Load<GameObject>("Prefabs/Gold");
        life = Resources.Load<GameObject>("Prefabs/Life");
        goal = Resources.Load<GameObject>("Prefabs/Goal");
        key = Resources.Load<GameObject>("Prefabs/Key");
        poison = Resources.Load<GameObject>("Prefabs/Poison");
        shoesN = Resources.Load<GameObject>("Prefabs/Shoes1");
        shoesS = Resources.Load<GameObject>("Prefabs/Shoes2");

        bg[0] = Resources.Load<GameObject>("Prefabs/BG1");
        bg[1] = Resources.Load<GameObject>("Prefabs/BG2");
        block[0] = Resources.Load<GameObject>("Prefabs/Block 1");
        block[1] = Resources.Load<GameObject>("Prefabs/Block 2");

        JsonData = DataController.Instance.gameData;

        //스테이지 이동 디버그용
        //혹시 세이브 파일(?) 같은거 구현 될련지?

        /* Test */
        //GameManager.StageNumber = 14;
        //SelectStage(GameManager.StageNumber);

        if (JsonData.Stage == 0)
            JsonData.Stage = 1;
        if (JsonData.HardStage == 0)
            JsonData.HardStage = 1;

        if (SceneManager.GetActiveScene().name == "PuzBuzGame")
        {
            SelectStage(JsonData.Stage);
        }
        else if(SceneManager.GetActiveScene().name == "PuzBuzHardMode")
            SelectStage(JsonData.HardStage);
    }

    void SelectStage(int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                mapSet = mapSet1;
                break;
            case 2:
                mapSet = mapSet2;
                break;
            case 3:
                mapSet = mapSet3;
                break;
            case 4:
                mapSet = mapSet4;
                break;
            case 5:
                mapSet = mapSet5;
                break;
            case 6:
                mapSet = mapSet6;
                break;
            case 7:
                mapSet = mapSet7;
                break;
            case 8:
                mapSet = mapSet8;
                break;
            case 9:
                mapSet = mapSet9;
                break;
            case 10:
                mapSet = mapSet10;
                break;
            case 11:
                mapSet = mapSet11;
                break;
            case 12:
                mapSet = mapSet12;
                break;
            case 13:
                mapSet = mapSet13;
                break;
            case 14:
                mapSet = mapSet14;
                break;
            case 15:
                mapSet = mapSet15;
                break;
            case 16:
                mapSet = mapSet16;
                break;
            case 17:
                mapSet = mapSet17;
                break;
            case 18:
                mapSet = mapSet18;
                break;
            case 19:
                mapSet = mapSet19;
                break;
            case 20:
                mapSet = mapSet20;
                break;
        }
    }

    void Start()
    {
        MapSet();
        FindKey();
        CreateBG();
    }

    public void MapSet()
    {
        Debug.Log("xLength: " + mapSet.GetLength(1) + ", yLength: " + mapSet.GetLength(0));
        MapPosition = GameObject.Find("MapPos");
        //GetLength()로 바꿨음... 맵 크기 조정할 때 편하라고...
        for (int i = 0; i < mapSet.GetLength(0); i++)
        {
            for (int j = 0; j < mapSet.GetLength(1); j++)
            {
                switch (mapSet[i, j])
                {
                    case (int)mapType.Wall:
                        if ((i + j) % 2 == 0)
                            Instantiate(block[0], new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                                GameObject.Find("Walls").transform);
                        else
                            Instantiate(block[1], new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                                GameObject.Find("Walls").transform);
                        break;
                    case (int)mapType.Player:
                        Instantiate(player, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity);
                        PosX = j;
                        PosY = i;
                        break;
                    case (int)mapType.EnemyF1:
                        Instantiate(enemyF1, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Emeries").transform);
                        break;
                    case (int)mapType.EnemyM1:
                        Instantiate(enemyM1, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Emeries").transform);
                        break;
                    case (int)mapType.EnemyF2:
                        Instantiate(enemyF2, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Emeries").transform);
                        break;
                    case (int)mapType.EnemyM2:
                        Instantiate(enemyM2, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Emeries").transform);
                        break;
                    case (int)mapType.Bomb:
                        Instantiate(bomb, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Time:
                        Instantiate(timeItem, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Potion:
                        Instantiate(potion, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Gold:
                        Instantiate(gold, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Life:
                        Instantiate(life, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Goal:
                        Instantiate(goal, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Key:
                        Instantiate(key, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.Poison:
                        Instantiate(poison, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.ShoesN:
                        Instantiate(shoesN, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    case (int)mapType.ShoesS:
                        Instantiate(shoesS, new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                            GameObject.Find("Items").transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void FindKey()
    {
        for (int i = 0; i < mapSet.GetLength(0); i++)
        {
            for (int j = 0; j < mapSet.GetLength(1); j++)
            {
                if (mapSet[i, j] == (int)mapType.Key)
                    ++GoalScript.keyCount;
            }
        }
    }

    private void CreateBG()
    {
        for (int i = 0; i < mapSet.GetLength(0); i++)
        {
            for (int j = 0; j < mapSet.GetLength(1); j++)
            {
                if ((i + j) % 2 == 0)
                    Instantiate(bg[0], new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                        GameObject.Find("BGTile").transform);
                else
                    Instantiate(bg[1], new Vector2(j - 8.5f, 4.5f - i), Quaternion.identity,
                        GameObject.Find("BGTile").transform);
            }
        }
    }
}
