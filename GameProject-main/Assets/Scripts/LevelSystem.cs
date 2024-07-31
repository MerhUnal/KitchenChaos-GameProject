using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{

    public static LevelSystem MainLevelSystem;

    public LevelDatas levelDatas;
    private string saveFilePath;

    private void Awake()
    {
        /*
        if (MainLevelSystem == null)
        {
            MainLevelSystem = this;
            DontDestroyOnLoad(this);
        }
        */
            
    }

    void Start()
    {

        if (MainLevelSystem == null)
        {
            MainLevelSystem = this;
            DontDestroyOnLoad(this);

            Debug.Log("Level system  start");
            saveFilePath = Application.persistentDataPath + "/PlayerLevels.json";
            levelDatas = new LevelDatas();

             // Daha �nce kaydedilmi? veriyi y�kle

            // E?er veri yoksa veya hatal?ysa yeni seviye verileri olu?tur
            if (levelDatas.levelDatas.Count == 0)
            {
                InitializeLevels();
            }
            SaveGame();
            LoadGame();

        }

      

    }


    private void InitializeLevels()
    {
        float[] levelTimes = { 60f, 55f, 50f };
        string[] sceneNames = { "GameScene", "Stage2", "Stage3" }; // �rnek sahne adlar?
        int numberOfLevels = 9; // �rnek olarak 9 seviye

        for (int i = 0; i < numberOfLevels; i++)
        {
            LevelData level = new LevelData();
            level.levelIndex = i;
            level.levelTime = levelTimes[i % levelTimes.Length];
            level.sceneName = sceneNames[i / 3]; // Her 3 seviyede bir sahne de?i?ir
            //level.recipeSOs = new List<int>() {0, 3 };

            if (i < 3)
            {
                level.recipeSOs = new List<int>() { 0,1,2 }; // ?lk 3 seviye i�in
            }
            else if (i >= 3 && i < 6)
            {
                level.recipeSOs = new List<int>() { 0,1,2 }; // Sonraki 3 seviye i�in
            }
            else
            {
                level.recipeSOs = new List<int>() { 0,1,2,3 }; // Geri kalan seviyeler i�in 
            }

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
}

