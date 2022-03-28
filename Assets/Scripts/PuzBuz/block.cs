using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    private GameObject player;
    private MovingScript move;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        move = player.GetComponent<MovingScript>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("!");
        switch (move.keyCode)
        {
            case 'D':
                player.transform.position += Vector3.left;
                break;
            case 'A':
                player.transform.position += Vector3.right;
                break;
            case 'S':
                player.transform.position += Vector3.up;
                break;
            case 'W':
                player.transform.position += Vector3.down;
                break;

        }
    }
}
