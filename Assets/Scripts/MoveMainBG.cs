using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMainBG : MonoBehaviour
{
    public float speed = 1.0f;
    Transform BG;
    void Awake()
    {
        BG = GetComponent<Transform>();
    }

    void Update()
    {
        // -0.33, 1.27
        //CoolTime += Time.deltaTime;
        MoveFunc();
        if (BG.position.x == -2.12f && BG.position.y == 1.03f)
        {
            BG.position = new Vector3(0, 0, 100);
            MoveFunc();
        }
    }

    void MoveFunc()
    {
        BG.position = Vector3.MoveTowards(BG.position, new Vector3(-2.12f, 1.03f, 100f), Time.deltaTime * speed);
    }
}
