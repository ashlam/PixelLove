using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SavingDataManager
{
    #region -------- defined field name ---------
    internal const string NameOfLevelID = "LevelID";
    internal const string NameOfMaxColumns = "MaxColumns";
    internal const string NameOfMaxRows = "MaxRows";
    internal const string NameOfAnswerText = "AnswerText";
    internal const string NameOfPassedLevels = "PassedLevel";
    #endregion

    #region -------- sington --------
    static SavingDataManager instance = new SavingDataManager();
    internal static SavingDataManager CreateInstance()
    {
        return instance;
    }
    #endregion

    GameSavingData_Chequer chequerSavingData = null;
    GameSavingData_Victory victorySavingData = null;
    private SavingDataManager()
    {

        //CleanSaveData(); //调试备用
        ShowPlayerPrefsContent();//调试备用
        Initialize();
        //CleanSaveData(); //调试备用
    }

    void Initialize()
    {
        //read playerprab and init to data;
        chequerSavingData = new GameSavingData_Chequer();
        {
            chequerSavingData.InitializeFromPlayerPrefs();
        }

        victorySavingData = new GameSavingData_Victory();
        {
            Debug.Log("what ?");
            victorySavingData.InitializeFromPlayerPrefs();
        }
    }



    internal GameSavingData_Chequer GetChequerSaveData()
    {
        return chequerSavingData;
    }



    internal void Save()
    {
        chequerSavingData.WriteToPlayerPrefs();
    }

    internal void SaveToVictory()
    {
        victorySavingData.AddVictoryRecord(MainGame.currentLevelData.levelID);
        victorySavingData.WriteToPlayerPrefs();
        Debug.Log(" save victory " + MainGame.currentLevelData.levelID);
        //MainGame.currentLevelData = null;//放这儿？

        //chequerSavingData.currentLevelID = -1;
        //chequerSavingData.maxColumns = 0;
        //chequerSavingData.maxRows = 0;
        //chequerSavingData.answerText = "";
        chequerSavingData.WriteToPlayerPrefs();

    }


    internal void Load()
    {
        if (chequerSavingData == null)
        {
            this.Initialize();
        }
        Debug.Log("mi mi mi " + chequerSavingData.ToString() + " _---_ " + MainGame.currentLevelData);
        if (chequerSavingData != null && MainGame.currentLevelData != null)
        {
            Debug.Log("tora tora tora " + chequerSavingData.currentLevelID + " -___- " + MainGame.currentLevelData.levelID);
            if (chequerSavingData.currentLevelID >= 0 && chequerSavingData.currentLevelID == MainGame.currentLevelData.levelID)
            {
                Debug.Log(" monster kill !");
                chequerSavingData.ConvertTextToAnswer();
            }
        }
    }

    internal void CleanSaveData()
    {
        PlayerPrefs.DeleteAll();
        //Debug.Log("all save be deleted");
    }

    internal void ShowPlayerPrefsContent()
    {
        Debug.Log("-----------Player prefs ------------");
        Debug.Log(string.Format("{0}:{1}", NameOfAnswerText, PlayerPrefs.GetString(NameOfAnswerText)));
        Debug.Log(string.Format("{0}:{1}", NameOfLevelID, PlayerPrefs.GetInt(NameOfLevelID)));
        Debug.Log(string.Format("{0}:{1}", NameOfMaxColumns, PlayerPrefs.GetInt(NameOfMaxColumns)));
        Debug.Log(string.Format("{0}:{1}", NameOfMaxRows, PlayerPrefs.GetInt(NameOfMaxRows)));
        Debug.Log(string.Format("{0}:{1}", NameOfPassedLevels, PlayerPrefs.GetString(NameOfPassedLevels)));
        Debug.Log("-----------");
    }

    internal void ClearCurrentLevel()
    {
        chequerSavingData.answerText = "";
        Save();
    }

    internal bool CheckHasPassedLevel(int levelId)
    {
        //throw new System.NotImplementedException();
        bool result = false;
        result = victorySavingData.CheckExistRecord(levelId);
        return result;
    }
}