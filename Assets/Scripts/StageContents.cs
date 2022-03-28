using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageContents : MonoBehaviour
{
    int ContentCount;
    Transform ContentChild;
    Sprite[] StageButton;
    GameData JsonData;

    void Awake()
    {
        ContentCount = transform.childCount;
        StageButton = Resources.LoadAll<Sprite>("StageBtn");
        JsonData = DataController.Instance.gameData;

        for (int i = 1; i < ContentCount; i++)
        {
            ContentChild = transform.GetChild(i);
            if (i + 1 > JsonData.StageRecord) // Locked
            {
                ContentChild.GetComponent<Image>().sprite = StageButton[1];
                ContentChild.transform.Find("Text").gameObject.SetActive(false);
            }
            else // UnLocked
            {
                ContentChild.GetComponent<Image>().sprite = StageButton[0];
                ContentChild.transform.Find("Text").gameObject.SetActive(true);
            }
        }
    }

    public void GoToStage(int StageNumber)
    {
        JsonData = DataController.Instance.gameData;

        if (StageNumber <= JsonData.StageRecord || StageNumber == 1)
        {
            MapSetting.isResume = false;
            JsonData.Stage = StageNumber;
            SceneManager.LoadScene("PuzBuzGame");
        }
    }
}
