using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindShortDist : MonoBehaviour
{
    int shortDist = -1;
    int EmeryCount;
    int[] tempArray;
    EnemyScript EScript;
    List<int> tempChoice = new List<int>();

    void Update()
    {
        EmeryCount = transform.childCount;
        tempArray = new int[EmeryCount];
        FindShortDistance();
        ChoiceEnemy();
        RandomChoice();
    }

    void FindShortDistance()
    {
        shortDist = -1;
        for (int i = 0; i < EmeryCount; i++)
        {
            Transform EmeryChild = transform.GetChild(i);
            EScript = EmeryChild.GetComponent<EnemyScript>();
            if (i == 0)
                shortDist = EScript.dist;
            else if (EScript.dist < shortDist)
                shortDist = EScript.dist;
            tempArray[i] = EScript.dist;
        }
    }

    void ChoiceEnemy()
    {
        for (int i = 0; i < EmeryCount; i++)
        {
            if (tempArray[i] == shortDist)
            {
                tempChoice.Add(i);
            }
        }
        Debug.Log(EmeryCount);
    }

    void RandomChoice()
    {
        Transform EmeryChild = transform.GetChild(tempChoice[Random.Range(0, tempChoice.Count)]);
        EScript = EmeryChild.GetComponent<EnemyScript>();
        EScript.isEnemyChoice = true;
        tempChoice.RemoveAll(x => true);
    }
}
