using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageManager : MonoBehaviour
{
    Transform page1, page2, page4, page5, page6, page7;
    Transform LButton, RButton, UButton, DButton;
    Transform GenButton;
    Transform Female6_1, Female6_2;
    Transform blackPanel;
    RectTransform P1;
    RectTransform P4, M4_F, M4_M;
    RectTransform P5, M5;
    RectTransform P6, M6_1, M6_2;
    RectTransform P7;
    RectTransform xSign;
    Vector3 P1_OriginPos;
    Vector3 P4_OriginPos, M4_FemalePos, M4_MalePos;
    Vector3 P5_OriginPos, M5_Pos;
    Vector3 P6_OriginPos, M6_1Pos, M6_2Pos;
    Vector3 P7_OriginPos;

    Image LImage, RImage, UImage, DImage;
    Image PlayerImage, GenImage, P4Image, P5Image, P7Image;
    Image PrevImg, NextImg;
    Image xSign_Img;
    Image blackPanelImg;
    Text PageText;

    Sprite[] PlayerSprites = new Sprite[2];

    public int pageNumber = 1;
    public int pageChild;
    int GenType = 0;
    float timer = 0.0f;
    float moveTime = 0.0f;
    bool startFadeIn = true;
    bool isBackToMainClick = false;

    void Awake()
    {
        page1 = transform.GetChild(0);
        LButton = page1.transform.GetChild(5);
        RButton = page1.transform.GetChild(6);
        UButton = page1.transform.GetChild(7);
        DButton = page1.transform.GetChild(8);
        LImage = LButton.GetComponent<Image>();
        RImage = RButton.GetComponent<Image>();
        UImage = UButton.GetComponent<Image>();
        DImage = DButton.GetComponent<Image>();

        P1 = page1.transform.GetChild(9).GetComponent<RectTransform>();
        P1_OriginPos = P1.anchoredPosition;

        page2 = transform.GetChild(1);
        PlayerImage = page2.transform.GetChild(2).GetComponent<Image>();
        GenButton = page2.transform.GetChild(3);
        GenImage = GenButton.GetComponent<Image>();

        page4 = transform.GetChild(3);
        P4 = page4.transform.GetChild(19).GetComponent<RectTransform>();
        M4_F = page4.transform.GetChild(20).GetComponent<RectTransform>();
        M4_M = page4.transform.GetChild(21).GetComponent<RectTransform>();
        P4_OriginPos = P4.anchoredPosition;
        M4_FemalePos = M4_F.anchoredPosition;
        M4_MalePos = M4_M.anchoredPosition;
        P4Image = page4.transform.GetChild(19).GetComponent<Image>();

        page5 = transform.GetChild(4);
        P5 = page5.transform.GetChild(19).GetComponent<RectTransform>();
        M5 = page5.transform.GetChild(21).GetComponent<RectTransform>();
        P5Image = page5.transform.GetChild(19).GetComponent<Image>();
        P5_OriginPos = P5.anchoredPosition;
        M5_Pos = M5.anchoredPosition;

        page6 = transform.GetChild(5);
        P6 = page6.transform.GetChild(7).GetComponent<RectTransform>();
        Female6_1 = page6.transform.GetChild(8);
        Female6_2 = page6.transform.GetChild(9);
        M6_1 = Female6_1.GetComponent<RectTransform>();
        M6_2 = Female6_2.GetComponent<RectTransform>();
        P6_OriginPos = P6.anchoredPosition;
        M6_1Pos = M6_1.anchoredPosition;
        M6_2Pos = M6_2.anchoredPosition;
        xSign = page6.transform.GetChild(10).GetComponent<RectTransform>();
        xSign_Img = page6.transform.GetChild(10).GetComponent<Image>();

        page7 = transform.GetChild(6);
        P7 = page7.transform.GetChild(14).GetComponent<RectTransform>();
        P7Image = page7.transform.GetChild(14).GetComponent<Image>();
        P7_OriginPos = P7.anchoredPosition;

        pageChild = transform.childCount;
        PrevImg = GameObject.Find("ArrowPrev").GetComponent<Image>();
        NextImg = GameObject.Find("ArrowNext").GetComponent<Image>();

        PageText = GameObject.Find("PageNum").GetComponent<Text>();

        PlayerSprites[0] = Resources.Load<Sprite>("Prefabs/PlayerM");
        PlayerSprites[1] = Resources.Load<Sprite>("Prefabs/PlayerW");

        blackPanel = GameObject.Find("Canvas").transform.GetChild(5).GetComponent<Transform>();
        blackPanelImg = blackPanel.GetComponent<Image>();
    }

    void Start()
    {
        PageMove(1);
        ArrowIcon(1);
    }

    void Update()
    {
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

        PageText.text = pageNumber.ToString() + "/" + pageChild.ToString();
        timer += Time.deltaTime;
        PageMove(pageNumber);
        ArrowIcon(pageNumber);

        switch (pageNumber)
        {
            case 1:
                if (timer < 0.5f)
                {
                    LButton.localScale = Vector3.one * 1.4f;
                    RButton.localScale = Vector3.one * 1.4f;
                    UButton.localScale = Vector3.one * 1.4f;
                    DButton.localScale = Vector3.one * 1.4f;
                    LImage.color = new Vector4(1, 1, 1, 0.8f);
                    RImage.color = new Vector4(1, 1, 1, 0.8f);
                    UImage.color = new Vector4(1, 1, 1, 0.8f);
                    DImage.color = new Vector4(1, 1, 1, 0.8f);
                    P1.anchoredPosition = P1_OriginPos;
                }
                else if (timer >= 0.5f && timer < 1.0f)
                {
                    LButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 0.5f));
                    LImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 0.5f));
                    P1.anchoredPosition = P1_OriginPos + Vector3.left * 128;
                }
                else if (timer >= 1.0f && timer < 1.5f)
                {
                    LButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 1.0f));
                    LImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 1.0f));
                }
                else if (timer >= 1.5f && timer < 2.0f)
                {
                    RButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 1.5f));
                    RImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 1.5f));
                    P1.anchoredPosition = P1_OriginPos;
                }
                else if (timer >= 2.0f && timer < 2.5f)
                {
                    RButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 2.0f));
                    RImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 2.0f));
                }
                else if (timer >= 2.5f && timer < 3.0f)
                {
                    RButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 2.5f));
                    RImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 2.5f));
                    P1.anchoredPosition = P1_OriginPos + Vector3.right * 128;

                }
                else if (timer >= 3.0f && timer < 3.5f)
                {
                    RButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 3.0f));
                    RImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 3.0f));
                }
                else if (timer >= 3.5f && timer < 4.0f)
                {
                    LButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 3.5f));
                    LImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 3.5f));
                    P1.anchoredPosition = P1_OriginPos;
                }
                else if (timer >= 4.0f && timer < 4.5f)
                {
                    LButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 4.0f));
                    LImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 4.0f));
                }
                else if (timer >= 4.5f && timer < 5.0f)
                {
                    UButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 4.5f));
                    UImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 4.5f));
                    P1.anchoredPosition = P1_OriginPos + Vector3.up * 128;
                }
                else if (timer >= 5.0f && timer < 5.5f)
                {
                    UButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 5.0f));
                    UImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 5.0f));
                }
                else if (timer >= 5.5f && timer < 6.0f)
                {
                    DButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 5.5f));
                    DImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 5.5f));
                    P1.anchoredPosition = P1_OriginPos;
                }
                else if (timer >= 6.0f && timer < 6.5f)
                {
                    DButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 6.0f));
                    DImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 6.0f));
                }
                else if (timer >= 6.5f)
                {
                    timer = 0.0f;
                }
                break;

            case 2:
                if (timer < 0.5f)
                {
                    GenButton.localScale = Vector3.one * 1.4f;
                    GenImage.color = new Vector4(1, 1, 1, 0.8f);
                    PlayerImage.sprite = PlayerSprites[0];
                }
                else if (timer >= 0.5f && timer < 1.0f)
                {
                    GenButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 0.5f));
                    GenImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 0.5f));
                    PlayerImage.sprite = PlayerSprites[1];
                }
                else if (timer >= 1.0f && timer < 1.5f)
                {
                    GenButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 1.0f));
                    GenImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 1.0f));
                }
                else if (timer >= 2.5f && timer < 3.0f)
                {
                    GenButton.localScale = Vector3.one * (1.4f + 0.2f * (timer - 2.5f));
                    GenImage.color = new Color(1, 1, 1, 0.8f - 0.8f * (timer - 2.5f));
                    PlayerImage.sprite = PlayerSprites[0];
                }
                else if (timer >= 3.0f && timer < 3.5f)
                {
                    GenButton.localScale = Vector3.one * (1.5f - 0.2f * (timer - 3.0f));
                    GenImage.color = new Color(1, 1, 1, 0.4f + 0.8f * (timer - 3.0f));
                }
                else if (timer >= 4.0f)
                {
                    timer = 0.0f;
                }
                break;

            case 4:
                if (timer < 0.5f)
                {
                    P4Image.sprite = PlayerSprites[0];
                    P4.anchoredPosition = P4_OriginPos;
                    M4_F.anchoredPosition = M4_FemalePos;
                    M4_M.anchoredPosition = M4_MalePos;
                }
                else if (timer >= 0.5f && timer < 1.5f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos + Vector3.left * 133;
                }
                else if (timer >= 1.5f && timer < 2.5f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 266;
                    M4_M.anchoredPosition = M4_MalePos + Vector3.right * 133;
                }
                else if (timer >= 2.5f && timer < 3.5f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos + Vector3.left * 266;
                }
                else if (timer >= 3.5f && timer < 4.5f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 266;
                    M4_M.anchoredPosition = M4_MalePos + Vector3.right * 266;
                }
                else if (timer >= 4.5f && timer < 5.5f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos + Vector3.left * 399;
                }
                else if (timer >= 5.5f && timer < 7.0f)
                {
                    P4.anchoredPosition = P4_OriginPos;
                }
                else if (timer >= 7.0f && timer < 8.0f)
                {
                    P4Image.sprite = PlayerSprites[1];
                }
                else if (timer >= 8.0f && timer < 9.0f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos + Vector3.left * 266;
                }
                else if (timer >= 9.0f && timer < 10.0f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 266;
                    M4_M.anchoredPosition = M4_MalePos + Vector3.right * 133;
                }
                else if (timer >= 10.0f && timer < 11.0f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos + Vector3.left * 133;
                }
                else if (timer >= 11.0f && timer < 12.0f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 266;
                    M4_M.anchoredPosition = M4_MalePos;
                }
                else if (timer >= 12.0f && timer < 13.0f)
                {
                    P4.anchoredPosition = P4_OriginPos + Vector3.down * 133;
                    M4_F.anchoredPosition = M4_FemalePos;
                }
                else if (timer >= 13.0f && timer < 14.5f)
                {
                    P4.anchoredPosition = P4_OriginPos;
                }
                else if (timer >= 14.5f && timer < 15.0f)
                {
                    P4Image.sprite = PlayerSprites[0];
                }
                else if (timer >= 15.0f)
                {
                    timer = 0.0f;
                }
                break;
            case 5:
                if (timer < 0.5f)
                {
                    P5Image.sprite = PlayerSprites[0];
                    P5.anchoredPosition = P5_OriginPos;
                    M5.anchoredPosition = M5_Pos;
                }
                else if (timer >= 0.5f && timer < 1.5f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 1.5f && timer < 2.5f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 266;
                    M5.anchoredPosition = M5_Pos + Vector3.left * 266;
                }
                else if (timer >= 2.5f && timer < 3.5f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 3.5f && timer < 4.5f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 266;
                    M5.anchoredPosition = M5_Pos + Vector3.left * 532;
                }
                else if (timer >= 4.5f && timer < 5.5f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 5.5f && timer < 7.0f)
                {
                    P5.anchoredPosition = P5_OriginPos;
                }
                else if (timer >= 7.0f && timer < 8.0f)
                {
                    P5Image.sprite = PlayerSprites[1];
                }
                else if (timer >= 8.0f && timer < 9.0f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 9.0f && timer < 10.0f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 266;
                    M5.anchoredPosition = M5_Pos + Vector3.left * 266;
                }
                else if (timer >= 10.0f && timer < 11.0f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 11.0f && timer < 12.0f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 266;
                    M5.anchoredPosition = M5_Pos;
                }
                else if (timer >= 12.0f && timer < 13.0f)
                {
                    P5.anchoredPosition = P5_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 13.0f && timer < 14.5f)
                {
                    P5.anchoredPosition = P5_OriginPos;
                }
                else if (timer >= 14.5f && timer < 15.0f)
                {
                    P5Image.sprite = PlayerSprites[0];
                }
                else if (timer >= 15.0f)
                {
                    timer = 0.0f;
                }
                break;
            case 6:
                if (timer < 0.5f)
                {
                    P6.anchoredPosition = P6_OriginPos;
                    M6_1.anchoredPosition = M6_1Pos;
                    M6_1.gameObject.SetActive(true);
                    M6_2.gameObject.SetActive(false);
                    xSign_Img.color = Vector4.zero;
                }
                else if (timer >= 0.5f && timer < 0.7f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 133;
                }
                else if (timer >= 0.7f && timer < 1.5f)
                {
                    M6_1.anchoredPosition = M6_1Pos + Vector3.left * 133;
                }
                else if (timer >= 1.5f && timer < 1.7f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 266;
                }
                else if (timer >= 1.7f && timer < 2.5f)
                {
                    M6_1.anchoredPosition = M6_1Pos + Vector3.left * 266;
                }
                else if (timer >= 2.5f && timer < 2.7f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 399;
                }
                else if (timer >= 2.7f && timer < 3.0f)
                {
                    xSign.localScale = Vector3.one * (8 - 10 * (timer - 2.7f));
                    xSign_Img.color = Vector4.one;
                }
                else if (timer >= 3.5f && timer < 4.0f)
                {
                    xSign_Img.color = new Color(1, 1, 1, 1 - 2 * (timer - 3.5f));
                }
                if (timer >= 5.0f && timer < 5.5f)
                {
                    P6.anchoredPosition = P6_OriginPos;
                    M6_2.anchoredPosition = M6_2Pos;
                    M6_1.gameObject.SetActive(false);
                    M6_2.gameObject.SetActive(true);
                }
                else if (timer >= 5.5f && timer < 5.7f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 133;
                }
                else if (timer >= 5.7f && timer < 6.5f)
                {
                    M6_2.anchoredPosition = M6_2Pos + Vector3.left * 266;
                }
                else if (timer >= 6.5f && timer < 6.7f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 266;
                }
                else if (timer >= 8.0f && timer < 8.2f)
                {
                    P6.anchoredPosition = P6_OriginPos + Vector3.right * 133;
                }
                else if (timer >= 8.2f && timer < 8.4f)
                {
                    M6_2.anchoredPosition = M6_2Pos + Vector3.left * 532;
                }
                else if (timer >= 8.4f && timer < 8.7f)
                {
                    xSign.localScale = Vector3.one * (8 - 10 * (timer - 8.4f));
                    xSign_Img.color = Vector4.one;
                }
                else if (timer >= 9.2f && timer < 9.7f)
                {
                    xSign_Img.color = new Color(1, 1, 1, 1 - 2 * (timer - 9.2f));
                }
                else if (timer >= 10.7f)
                {
                    timer = 0.0f;
                }
                break;
            case 7:
                if (timer < 0.5f)
                {
                    P7Image.color = Vector4.one;
                    P7.anchoredPosition = P7_OriginPos;
                }
                else if (timer >= 0.5f && timer < 1.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.down * 133;
                }
                else if (timer >= 1.5f && timer < 2.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.down * 133 + Vector3.right * 133;
                }
                else if (timer >= 2.5f && timer < 3.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.down * 133 + Vector3.right * 266;
                }
                else if (timer >= 3.5f && timer < 4.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.right * 266;
                }
                else if (timer >= 4.5f && timer < 5.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.right * 399;
                }
                else if (timer >= 5.5f && timer < 6.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.right * 532;
                }
                else if (timer >= 6.5f && timer < 7.5f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.down * 133 + Vector3.right * 532;
                }
                else if (timer >= 7.5f && timer < 7.7f)
                {
                    P7.anchoredPosition = P7_OriginPos + Vector3.down * 133 + Vector3.right * 665;
                }
                else if (timer >= 7.7f && timer < 8.0f)
                {
                    P7Image.color = new Color(1, 1, 1, 1 - (1 / 0.3f) * (timer - 7.7f));
                }
                else if (timer >= 9.0f)
                {
                    timer = 0.0f;
                }
                break;
        }

        if (isBackToMainClick)
        {
            moveTime += Time.deltaTime;
            blackPanelImg.color = new Color(0, 0, 0, moveTime * 2);

            if (moveTime >= 0.5f)
            {
                moveTime = 0.5f;
                SceneManager.LoadScene("Title");
            }
        }
    }

    void PageMove(int pageNum)
    {
        for (int i = 0; i < pageChild; i++)
        {
            if (i == pageNum - 1)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void ArrowIcon(int pageNum)
    {
        if (pageNum == 1)
        {
            PrevImg.color = new Color32(217, 86, 52, 128);
            NextImg.color = new Color32(217, 86, 52, 255);
        }
        else if (pageNum == pageChild)
        {
            PrevImg.color = new Color32(217, 86, 52, 255);
            NextImg.color = new Color32(217, 86, 52, 128);
        }
        else
        {
            PrevImg.color = new Color32(217, 86, 52, 255);
            NextImg.color = new Color32(217, 86, 52, 255);
        }
    }

    // Button //
    public void NextPageBtn()
    {
        if (pageNumber < pageChild)
        {
            timer = 0.0f;
            pageNumber += 1;
        }
    }

    public void PrevPageBtn()
    {
        if (pageNumber >= 2)
        {
            timer = 0.0f;
            pageNumber -= 1;
        }
    }

    public void BackToMain()
    {
        blackPanel.gameObject.SetActive(true);
        moveTime = 0.0f;
        isBackToMainClick = true;
    }
}
