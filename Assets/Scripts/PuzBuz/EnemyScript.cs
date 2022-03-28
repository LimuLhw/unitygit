using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    enum moveType { UP = 0, DOWN, LEFT, RIGHT}
    float PosX, PosY;
    int intPosX, intPosY;
    int minCount = -1;
    int[] BFS_Count = new int[4];
    int GoToEnemyVecType = 0;
    public int dist = 0;
    public bool isEnemyChoice = false;
    bool isFirstCount = false;
    public static bool isEnemyMove = false;
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
        PosX = transform.position.x + 5.5f;
        PosY = 3.5f - transform.position.y;
        intPosX = (int)PosX;
        intPosY = (int)PosY;
        DefineBFSArray();
    }
    void DefineBFSArray()
    {
        for (int i = 0; i < MapSetting.mapSet.GetLength(0); i++)
        {
            for (int j = 0; j < MapSetting.mapSet.GetLength(1); j++)
            {
                if (MapSetting.mapSet[i, j] == 1)
                    BFS_Array[i, j] = -1;
                else
                    BFS_Array[i, j] = 0;
            }
        }
    }
    void ClearBFSCountArray()
    {
        for (int i = 0; i < 4; i++)
            BFS_Count[i] = 0;
    }
    void CompareCount()
    {
        minCount = 0;
        for (int i = 0; i < BFS_Count.Length; i++)
        {
            if (BFS_Count[i] <= 0)
                continue;
            if (BFS_Count[i] <= minCount || !isFirstCount)
            {
                minCount = BFS_Count[i];
                isFirstCount = true;
            }
        }
    }
    void GetListVec()
    {
        for (int i = 0; i < BFS_Count.Length; i++)
        {
            if (BFS_Count[i] == minCount)
            {
                listVec.Add(i);
            }
        }
        GoToEnemyVecType = listVec[Random.Range(0, listVec.Count)];

    }
    void Update()
    {
        ClearBFSCountArray();
        //dist = BFS(intPosY, intPosX, 1);
        if (!MovingScript.isPlayerDie) // 플레이어가 게임오버된 상태가 아니면
        {
            //ChoiceBFS();
        }
        CompareCount();
        if (isEnemyMove)
        {
            //GetListVec();
            Debug.Log(listVec.Count);
            switch (GoToEnemyVecType)
            {
                case (int)moveType.UP:
                    MapSetting.mapSet[intPosY, intPosX] = 0;
                    MapSetting.mapSet[intPosY - 1, intPosX] = 3;
                    --intPosY;
                    transform.position += Vector3.up;
                    break;
                case (int)moveType.DOWN:
                    MapSetting.mapSet[intPosY, intPosX] = 0;
                    MapSetting.mapSet[intPosY + 1, intPosX] = 3;
                    ++intPosY;
                    transform.position += Vector3.down;
                    break;
                case (int)moveType.LEFT:
                    MapSetting.mapSet[intPosY, intPosX] = 0;
                    MapSetting.mapSet[intPosY, intPosX - 1] = 3;
                    --intPosX;
                    transform.position += Vector3.left;
                    break;
                case (int)moveType.RIGHT:
                    MapSetting.mapSet[intPosY, intPosX] = 0;
                    MapSetting.mapSet[intPosY, intPosX + 1] = 3;
                    ++intPosX;
                    transform.position += Vector3.right;
                    break;
            }
            isEnemyMove = false;
            listVec.RemoveAll(x => true);
        }
        isFirstCount = false;
    }
    //int BFS(int y, int x, int count)
    //{
    //    queY.Enqueue(y);
    //    queX.Enqueue(x);
    //    queCnt.Enqueue(count);

    //    while (true)
    //    {
    //        y = queY.Peek();
    //        x = queX.Peek();
    //        count = queCnt.Peek();

    //        if (MapSetting.mapSet[y, x] == 2) // 해당 위치가 플레이어의 위치와 동일하면
    //        {
    //            while (queX.Count > 0)
    //            {
    //                queX.Dequeue();
    //                queY.Dequeue();
    //                queCnt.Dequeue();
    //                DefineBFSArray();
    //            }
    //            return count;
    //        }
    //        if (BFS_Array[y - 1, x] == 0)
    //        {
    //            queY.Enqueue(y - 1);
    //            queX.Enqueue(x);
    //            BFS_Array[y - 1, x] = count;
    //            queCnt.Enqueue(count + 1);
    //        }
    //        if (BFS_Array[y + 1, x] == 0)
    //        {
    //            queY.Enqueue(y + 1);
    //            queX.Enqueue(x);
    //            BFS_Array[y + 1, x] = count;
    //            queCnt.Enqueue(count + 1);
    //        }
    //        if (BFS_Array[y, x - 1] == 0)
    //        {
    //            queY.Enqueue(y);
    //            queX.Enqueue(x - 1);
    //            BFS_Array[y, x - 1] = count;
    //            queCnt.Enqueue(count + 1);
    //        }
    //        if (BFS_Array[y, x + 1] == 0)
    //        {
    //            queY.Enqueue(y);
    //            queX.Enqueue(x + 1);
    //            BFS_Array[y, x + 1] = count;
    //            queCnt.Enqueue(count + 1);
    //        }
    //        queY.Dequeue();
    //        queX.Dequeue();
    //        queCnt.Dequeue();

    //        if (queX.Count == 0)
    //            return -1;
    //    }
    //}

    //void ChoiceBFS()
    //{
    //    if (MapSetting.mapSet[intPosY - 1, intPosX] != 1) // UP
    //        BFS_Count[(int)moveType.UP] = BFS(intPosY - 1, intPosX, 1);
    //    if (MapSetting.mapSet[intPosY + 1, intPosX] != 1) // DOWN
    //        BFS_Count[(int)moveType.DOWN] = BFS(intPosY + 1, intPosX, 1);
    //    if (MapSetting.mapSet[intPosY, intPosX - 1] != 1) // LEFT
    //        BFS_Count[(int)moveType.LEFT] = BFS(intPosY, intPosX - 1, 1);
    //    if (MapSetting.mapSet[intPosY, intPosX + 1] != 1) // RIGHT
    //        BFS_Count[(int)moveType.RIGHT] = BFS(intPosY, intPosX + 1, 1);
    //}
}
