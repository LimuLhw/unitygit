/* DataController.cs */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public string GameDataFileName = ".json"; // 주의: 해당 파일 이름을 절대로 변경하지 말 것!
    public string AndroidSaveFileName = "/data.json";

    public GameData _gameDataAndroid;
    public GameData _gameData;

    public GameData gameData
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (_gameDataAndroid == null)
                {
                    LoadGameDataAndroid();
                    SaveGameDataAndroid();
                }
                return _gameDataAndroid;
            }
            else
            {
                if (_gameData == null)
                {
                    LoadGameData();
                    SaveGameData();
                }
                return _gameData;
            }
        }
    }

    /* PC(Editor) */
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }

    /* Android */
    public void LoadGameDataAndroid()
    {
        string filePath = Application.persistentDataPath + AndroidSaveFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _gameDataAndroid = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            _gameDataAndroid = new GameData();
        }
    }

    public void SaveGameDataAndroid()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + AndroidSaveFileName;
        File.WriteAllText(filePath, ToJsonData);
    }

    private void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
            SaveGameDataAndroid();
        else
            SaveGameData();
    }

    public static bool bPaused = false;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (Application.platform == RuntimePlatform.Android)
                SaveGameDataAndroid();
            bPaused = true;
        }
        else
        {
            if (bPaused)
            {
                bPaused = false;
            }
        }
    }
}