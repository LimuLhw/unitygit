using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CircleParticle;
    [SerializeField]
    private GameObject StarParticle;
    [SerializeField]
    private GameObject DiaParticle;

    GameObject[] CircleE;
    GameObject[] StarE;
    GameObject[] DiaE;

    GameObject[] targetPool;

    GameObject ObjectPoolingZone;

    void Awake()
    {
        CircleE = new GameObject[50];
        StarE = new GameObject[10];
        DiaE = new GameObject[10];

        ObjectPoolingZone = GameObject.Find("ObjectPoolingZone");

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < CircleE.Length; i++)
        {
            CircleE[i] = Instantiate(CircleParticle, ObjectPoolingZone.transform);
            CircleE[i].SetActive(false);
        }
        for (int i = 0; i < StarE.Length; i++)
        {
            StarE[i] = Instantiate(StarParticle, ObjectPoolingZone.transform);
            StarE[i].SetActive(false);
        }
        for (int i = 0; i < DiaE.Length; i++)
        {
            DiaE[i] = Instantiate(DiaParticle, ObjectPoolingZone.transform);
            DiaE[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "CircleE":
                targetPool = CircleE;
                break;
            case "StarE":
                targetPool = StarE;
                break;
            case "DiaE":
                targetPool = DiaE;
                break;
        }
        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
}
