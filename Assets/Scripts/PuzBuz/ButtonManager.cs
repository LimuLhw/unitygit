using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    float moveTime = 0.0f;
    float outputTime = 0.0f;
    float speed = 1.0f;
    RectTransform titleLogo;
    Transform selectUI, stageUI, optionUI, arrowBtn1, arrowBtn2, modeUI, warnUI, blackPanel, dataResetUI;
    Image soundBtn, vibrationBtn, effectBtn;
    CanvasGroup selectCG, stageCG, optionCG, arrowCG;
    Image blackPanelImg;
    Text GeneralStageText, HardcoreStageText, DataResetText, HighScoreText;
    Vector3 LogoPos;
    bool isSelectBtnClick = false;
    bool isBackBtnClick = false;
    bool isResumeClick = false;
    bool isHardModeClick = false;
    bool isHowToPlayClick = false;
    bool isOutputDataResetText = false;
    bool startFadeIn = true;
    Sprite[] btnSprites;
    GameData JsonData;

    void Awake()
    {
        titleLogo = transform.GetChild(0).GetComponent<RectTransform>();
        selectUI = transform.GetChild(1).GetComponent<Transform>();
        stageUI = transform.GetChild(2).GetComponent<Transform>();
        arrowBtn1 = transform.GetChild(3).GetComponent<Transform>();
        arrowBtn2 = transform.GetChild(4).GetComponent<Transform>();
        optionUI = transform.GetChild(5).GetComponent<Transform>();
        modeUI = transform.GetChild(6).GetComponent<Transform>();
        warnUI = transform.GetChild(7).GetComponent<Transform>();
        blackPanel = transform.GetChild(8).GetComponent<Transform>();
        dataResetUI = transform.GetChild(9).GetComponent<Transform>();

        selectCG = selectUI.GetComponent<CanvasGroup>();
        stageCG = stageUI.GetComponent<CanvasGroup>();
        arrowCG = arrowBtn1.GetComponent<CanvasGroup>();
        optionCG = optionUI.GetComponent<CanvasGroup>();

        GeneralStageText = modeUI.transform.GetChild(0).transform.Find("StageRecordText").GetComponent<Text>();
        HardcoreStageText = modeUI.transform.GetChild(1).transform.Find("StageRecordText").GetComponent<Text>();
        HighScoreText = modeUI.transform.GetChild(2).transform.Find("HighScoreText").GetComponent<Text>();
        DataResetText = dataResetUI.GetComponent<Text>();

        soundBtn = optionUI.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        vibrationBtn = optionUI.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        effectBtn = optionUI.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Image>();

        blackPanelImg = blackPanel.GetComponent<Image>();

        LogoPos = titleLogo.anchoredPosition;

        btnSprites = Resources.LoadAll<Sprite>("OnOff");

        JsonData = DataController.Instance.gameData;

        //if (!JsonData.isNormalStart && !JsonData.isHardCoreStart)
        //    stageText.text = "게임 시작";
        //else
        //    stageText.text = "이어서 하기";
    }

    void Update()
    {
        /* 초기 데이터 */
        if (JsonData.Stage <= 0)
            JsonData.Stage = 1;
        if (JsonData.HardStage <= 0)
            JsonData.HardStage = 1;
        if (JsonData.StageRecord <= 0)
            JsonData.StageRecord = 1;
        if (JsonData.StageHardRecord <= 0)
            JsonData.StageHardRecord = 1;
        if (JsonData.Life <= 0)
            JsonData.Life = 1;
        /////////////////////////////

        GeneralStageText.text = "현재 스테이지 : " + JsonData.StageRecord.ToString();
        HardcoreStageText.text = "현재 스테이지 : " + JsonData.StageHardRecord.ToString();
        HighScoreText.text = string.Format("{0:#,###0}", JsonData.HighScore);

        SoundFunc();
        VibrationFunc();
        EffectFunc();

        if (startFadeIn)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, 1 - moveTime * 2);

            if (moveTime >= 0.5f)
            {
                startFadeIn = false;
                moveTime = 0.0f;
                blackPanel.gameObject.SetActive(false);
            }
        }

        if (isSelectBtnClick)
        {
            moveTime += Time.deltaTime;
            titleLogo.anchoredPosition = LogoPos + Vector3.up * (-50 * Mathf.Cos(moveTime * Mathf.PI) + 50);
            selectUI.gameObject.SetActive(false);
            stageUI.gameObject.SetActive(true);
            arrowBtn1.gameObject.SetActive(true);

            arrowCG.blocksRaycasts = false;

            if (moveTime >= 1.0f)
            {
                LogoPos = titleLogo.anchoredPosition;
                moveTime = 0.0f;
                isSelectBtnClick = false;
                arrowCG.blocksRaycasts = true;
            }
        }

        if (isBackBtnClick)
        {
            moveTime += Time.deltaTime;
            titleLogo.anchoredPosition = LogoPos + Vector3.up * (50 * Mathf.Cos(moveTime * Mathf.PI) - 50);
            selectUI.gameObject.SetActive(true);
            stageUI.gameObject.SetActive(false);
            arrowBtn1.gameObject.SetActive(false);

            selectCG.blocksRaycasts = false;

            if (moveTime >= 1.0f)
            {
                LogoPos = titleLogo.anchoredPosition;
                moveTime = 0.0f;
                isBackBtnClick = false;
                selectCG.blocksRaycasts = true;
            }
        }

        if (isResumeClick)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                moveTime = 0.5f;
                MapSetting.isResume = true;
                JsonData.Stage = JsonData.StageRecord;
                SceneManager.LoadScene("PuzBuzGame");
            }
        }

        if (isHardModeClick)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                moveTime = 0.5f;
                MapSetting.isResume = true;
                JsonData.HardStage = JsonData.StageHardRecord;
                SceneManager.LoadScene("PuzBuzHardMode");
            }
        }

        if (isHowToPlayClick)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                moveTime = 0.5f;
                SceneManager.LoadScene("HowToPlay");
            }
        }

        if (isOutputDataResetText)
        {
            outputTime += Time.deltaTime;

            if (outputTime >= 4.5f)
            {
                JsonData.Stage = 1;
                JsonData.HardStage = 1;
                JsonData.Life = 1;
                JsonData.Score = 0;
                JsonData.StageRecord = 1;
                JsonData.StageHardRecord = 1;
                JsonData.HighScore = 0;
                SceneManager.LoadScene("Title");
            }

            switch ((int)(outputTime * 2) % 3)
            {
                case 0:
                    DataResetText.text = "데이터 초기화 중.";
                    break;
                case 1:
                    DataResetText.text = "데이터 초기화 중..";
                    break;
                case 2:
                    DataResetText.text = "데이터 초기화 중...";
                    break;
            }
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (modeUI.gameObject.activeSelf)
                {
                    modeUI.gameObject.SetActive(false);
                    selectUI.gameObject.SetActive(true);
                }
                else if (stageUI.gameObject.activeSelf && !isBackBtnClick)
                {
                    isBackBtnClick = true;
                }
                else if (selectUI.gameObject.activeSelf)
                {
                    Application.Quit();
                }
            }
        }
    }

    private void SoundFunc()
    {
        if (JsonData.isSound)
        {
            soundBtn.sprite = btnSprites[1];
        }
        else
        {
            soundBtn.sprite = btnSprites[0];
        }
    }

    private void VibrationFunc()
    {
        if (JsonData.isVibration)
        {
            vibrationBtn.sprite = btnSprites[1];
        }
        else
        {
            vibrationBtn.sprite = btnSprites[0];
        }
    }

    private void EffectFunc()
    {
        if (JsonData.isEffect)
        {
            effectBtn.sprite = btnSprites[1];
        }
        else
        {
            effectBtn.sprite = btnSprites[0];
        }
    }

    public void SelectStageBtn()
    {
        moveTime = 0.0f;
        isSelectBtnClick = true;
    }

    public void BackSelectBtn()
    {
        moveTime = 0.0f;
        isBackBtnClick = true;
    }

    public void OptionBtn(bool isClick)
    {
        if (isClick)
        {
            selectUI.gameObject.SetActive(false);
            optionUI.gameObject.SetActive(true);
            arrowBtn2.gameObject.SetActive(true);
        }
        else
        {
            selectUI.gameObject.SetActive(true);
            optionUI.gameObject.SetActive(false);
            arrowBtn2.gameObject.SetActive(false);
            modeUI.gameObject.SetActive(false);
        }
    }

    public void ResumeBtn()
    {
        selectUI.gameObject.SetActive(false);
        modeUI.gameObject.SetActive(true);
        arrowBtn2.gameObject.SetActive(true);
    }

    public void GeneralBtn()
    {
        blackPanel.gameObject.SetActive(true);
        moveTime = 0.0f;
        isResumeClick = true;
    }

    public void HardCoreBtn()
    {
        blackPanel.gameObject.SetActive(true);
        moveTime = 0.0f;
        isHardModeClick = true;
    }

    public void HowToPlayBtn()
    {
        blackPanel.gameObject.SetActive(true);
        moveTime = 0.0f;
        isHowToPlayClick = true;
    }

    public void ResetBtn()
    {
        warnUI.gameObject.SetActive(true);
        optionUI.gameObject.SetActive(false);
        arrowBtn2.gameObject.SetActive(false);
    }

    public void isResetOK()
    {
        warnUI.gameObject.SetActive(false);
        dataResetUI.gameObject.SetActive(true);
        isOutputDataResetText = true;
    }

    public void isResetCancel()
    {
        warnUI.gameObject.SetActive(false);
        optionUI.gameObject.SetActive(true);
        arrowBtn2.gameObject.SetActive(true);
    }

    public void SoundOnOff()
    {
        JsonData.isSound = !JsonData.isSound;
    }

    public void VibrationOff()
    {
        JsonData.isVibration = !JsonData.isVibration;
    }

    public void EffectOnOff()
    {
        JsonData.isEffect = !JsonData.isEffect;
    }

    //public void NormalModeBtn()
    //{
    //    JsonData.Stage = 1;
    //    JsonData.Life = 3;
    //    JsonData.Score = 0;
    //    blackPanel.gameObject.SetActive(true);
    //    moveTime = 0.0f;
    //    isResumeClick = true;
    //}

    //public void HardcoreModeBtn()
    //{
    //    JsonData.Stage = 1;
    //    JsonData.Life = 3;
    //    JsonData.Score = 0;
    //    JsonData.isHardCoreStart = true;
    //    blackPanel.gameObject.SetActive(true);
    //    moveTime = 0.0f;
    //    isHardModeClick = true;
    //}
}
