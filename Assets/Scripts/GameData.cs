/* GameData.cs */
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    public int Stage; // 일반모드 스테이지
    public int HardStage; // 하드코어모드 스테이지
    public int Life; // 목숨
    public int Score; // 점수
    public int HighScore; // 최고 점수
    public int StageRecord; // 최고 스테이지 기록(일반)
    public int StageHardRecord; // 최고 스테이지 기록(하드코어)
    public bool isSound = true; // 사운드 On/Off
    public bool isVibration = false; // 진동 On/Off
    public bool isEffect = true; // 이펙트 On/Off
}