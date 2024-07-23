using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{

    public static LevelSystem MainLevelSystem;

    public LevelDatas levelDatas;
    private string saveFilePath;

    void Start()
    {

        if (MainLevelSystem == null)
        {
            MainLevelSystem = this;

            Debug.Log("Level system  start");
            saveFilePath = Application.persistentDataPath + "/PlayerLevels.json";
            levelDatas = new LevelDatas();

            //LoadGame(); // Daha önce kaydedilmi? veriyi yükle

            // E?er veri yoksa veya hatal?ysa yeni seviye verileri olu?tur
            if (levelDatas.levelDatas.Count == 0)
            {
                InitializeLevels();
            }

            SaveGame();

            DontDestroyOnLoad(this);
        }
    }

    private void InitializeLevels()
    {
        float[] levelTimes = { 20f, 25f, 25f };
        string[] sceneNames = { "GameScene", "Stage2", "Stage3" }; // Örnek sahne adlar?
        int numberOfLevels = 9; // Örnek olarak 9 seviye

        for (int i = 0; i < numberOfLevels; i++)
        {
            LevelData level = new LevelData();
            level.levelIndex = i;
            level.levelTime = levelTimes[i % levelTimes.Length];
            level.sceneName = sceneNames[i / 3]; // Her 3 seviyede bir sahne de?i?ir
            level.recipeSOs = new List<int>() { 0, 3 };
            levelDatas.levelDatas.Add(level);
        }
    }

    public void SaveGame()
    {
        string saveLevelData = JsonUtility.ToJson(levelDatas);
        File.WriteAllText(saveFilePath, saveLevelData);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            levelDatas = JsonUtility.FromJson<LevelDatas>(loadPlayerData);
            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("There is no save file to load!");
        }
    }

    public int GetCurrentLevelIndex()
    {
        return levelDatas.currentLevelIndex;
    }

    public void SetNextLevel()
    {
        levelDatas.currentLevelIndex++;
        //SaveGame();
    }

    public float GetLevelTime(int levelIndex)
    {
        return levelDatas.levelDatas[levelIndex].levelTime;
    }

    public string GetCurrentLevelScene()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        return levelDatas.levelDatas[currentLevelIndex].sceneName;
    }

}

[System.Serializable]
public class LevelDatas
{
    public List<LevelData> levelDatas = new List<LevelData>();
    public int currentLevelIndex = 0;
}

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public float levelTime;
    public string sceneName;
    public List<int> recipeSOs;
    public bool canFireStart;
}

