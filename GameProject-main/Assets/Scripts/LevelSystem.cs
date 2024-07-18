using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public LevelDatas levelDatas;
    private string saveFilePath;
    private int currentLevelIndex;


    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/PlayerLevels.json";
        levelDatas = new LevelDatas();

        LoadGame(); // Daha önce kaydedilmis veriyi yükle

        // Eger veri yoksa veya hataliysa yeni seviye verileri olu?tur
        if (levelDatas.levelDatas.Count == 0)
        {
            InitializeLevels();
        }

        SaveGame();
        print(GetLevelTime(currentLevelIndex));
    }

    private void InitializeLevels()
    {
        float[] levelTimes = { 65f, 55f, 50f };
        int numberOfLevels = 6;

        for (int i = 0; i < numberOfLevels; i++)
        {
            LevelData level = new LevelData();
            level.levelIndex = i;
            level.levelTime = levelTimes[i % levelTimes.Length];
            level.recipeSOs = new List<int>() { 0, 3 };
            levelDatas.levelDatas.Add(level);
        }

        currentLevelIndex = 0; // ?lk seviye olarak 0 ba?lat
    }


    public float GetLevelTime(int levelIndex)
    {
        return levelDatas.levelDatas[levelIndex].levelTime;
    }
    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
    public void SetNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levelDatas.levelDatas.Count)
        {
            currentLevelIndex = 0; // E?er tüm seviyeler tamamland?ysa tekrar ba?a dön
        }
        SaveGame(); // Yeni seviye bilgilerini kaydet
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


}

[System.Serializable]
public class LevelDatas
{
    public List<LevelData> levelDatas = new List<LevelData>();
}

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public float levelTime;
    public List<int> recipeSOs;
}
