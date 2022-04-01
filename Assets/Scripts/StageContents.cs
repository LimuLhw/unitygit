using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageContents : MonoBehaviour
{
    int ContentCount;
    int StageNum;
    float moveTime = 0.0f;
    bool isStart = false;
    Transform ContentChild;
    Transform blackPanel;
    Image blackPanelImg;
    Sprite[] StageButton;
    GameData JsonData;

    void Awake()
    {
        blackPanel = GameObject.Find("MainTitleCanvas").transform.GetChild(8).GetComponent<Transform>();
        blackPanelImg = blackPanel.GetComponent<Image>();

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

    void Update()
    {
        if (isStart)
        {
            blackPanel.gameObject.SetActive(true);
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                MapSetting.isResume = false;
                JsonData.Stage = StageNum;
                SceneManager.LoadScene("PuzBuzGame");
            }
        }
    }

    public void GoToStage(int StageNumber)
    {
        if (StageNumber <= JsonData.StageRecord || StageNumber == 1)
        {
            isStart = true;
            StageNum = StageNumber;
        }
    }
}
