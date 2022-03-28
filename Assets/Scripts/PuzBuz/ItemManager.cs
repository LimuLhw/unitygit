using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    float PosX, PosY;
    public static bool isItemGet = false;
    public static int potionMove = 0;
    UIManager UIMgr;
    GameObject ParticleClone;
    ObjectManager ObjectMgr;
    GameData JsonData;

    private GameManager gm;
    private AudioSource snd;



    void Awake()
    {
        UIMgr = GameObject.Find("Canvas").GetComponent<UIManager>();
        ObjectMgr = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        snd = GameObject.Find("GameManager").GetComponent<AudioSource>();

        JsonData = DataController.Instance.gameData;
    }

    void Start()
    {
        PosX = transform.position.x + 8.5f;
        PosY = 4.5f - transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.tag == "Player")
        {
            if (JsonData.isSound)
                snd.PlayOneShot(gm.getItem, 1.0f);

            if (gameObject.tag == "Bomb")
            {
                for (int i = (int)PosY - 1; i <= PosY + 1; i++)
                {
                    if (i < 0 || i >= MapSetting.mapSet.GetLength(0)) continue;
                    for (int j = (int)PosX - 1; j <= PosX + 1; j++)
                    {
                        if (j < 0 || j >= MapSetting.mapSet.GetLength(1)) continue;
                        if (MapSetting.mapSet[i, j] >= 3 && MapSetting.mapSet[i, j] <= 6) 
                            MapSetting.mapSet[i, j] = 0;
                    }
                }
                FindEmeries(transform.position.y, transform.position.x);
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            if (gameObject.tag == "Time")
            {
                UIMgr.delT += 20;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            if (gameObject.tag == "Potion")
            {
                potionMove = 5;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
                // 파티클 색: Blue
            }

            if (gameObject.tag == "Gold")
            {
                //GameManager.SumScore += 50;
                //UIMgr.scoreInt += 50;
                if(SceneManager.GetActiveScene().name == "PuzBuzHardMode")
                    JsonData.Score += 50;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("DiaE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
                // 파티클 색: Gold
            }

            if (gameObject.tag == "Life")
            {
                //++GameManager.PB_lifeVal;
                JsonData.Life += 1;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
                // 파티클 색: Red
            }

            if (gameObject.tag == "Key")
            {
                --GoalScript.keyCount;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            if (gameObject.tag == "Poison")
            {
                UIManager.GetPoison = true;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            if (gameObject.tag == "ShoesN")
            {
                MovingScript.ShoesMove = 1;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            if (gameObject.tag == "ShoesS")
            {
                MovingScript.ShoesMove = 2;
                if (JsonData.isEffect)
                {
                    ParticleClone = ObjectMgr.MakeObj("CircleE");
                    ParticleClone.SetActive(true);
                    ParticleClone.transform.position = gameObject.transform.position;
                }
            }

            isItemGet = true;
            gameObject.SetActive(false);
        }
    }

    void FindEmeries(float y, float x)
    {
        //Transform[] Enemies = GameObject.Find("Emeries").GetComponentsInChildren<Transform>();
        Transform Enemies = GameObject.Find("Emeries").GetComponent<Transform>();
        int EnemyCount = Enemies.transform.childCount;

        for (int k = 0; k < EnemyCount; k++)
        {
            Transform EnemyChild = Enemies.GetChild(k);
            //Debug.Log("x: " + Enemies[k].position.x + ", y: " + Enemies[k].position.y);
            if (EnemyChild.position.y >= y - 1 && EnemyChild.position.y <= y + 1 &&
                EnemyChild.position.x >= x - 1 && EnemyChild.position.x <= x + 1)
            {
                EnemyChild.gameObject.SetActive(false);
            }
        }
    }
}
