using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGScript : MonoBehaviour
{
    CanvasGroup ButtonCG;

    void Awake()
    {
        ButtonCG = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (MovingScript.isPlayerDie)
            ButtonCG.blocksRaycasts = false;
    }
}
