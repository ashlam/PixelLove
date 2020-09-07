using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

interface ISavingData
{
    void InitializeFromPlayerPrefs();
    void WriteToPlayerPrefs();
}

public class GameSavingData_Chequer:ISavingData
{
    string nameOfLevelID = SavingDataManager.NameOfLevelID;
    string nameOfMaxColumns = SavingDataManager.NameOfMaxColumns;
    string nameOfMaxRows = SavingDataManager.NameOfMaxRows;
    string nameOfAnswerText = SavingDataManager.NameOfAnswerText;

    /// <summary>
    /// 当前是第几关
    /// 若为-1则表示不再读取answer数据（没有正在进行中的关卡）
    /// </summary>
    public int currentLevelID = -1;
    //本关有多少行列
    public int maxColumns, maxRows;
    //string[] answersText = null;
    public string answerText;

    internal void ConvertTextToAnswer()
    {
        Debug.Log("load text is: " + answerText);
        string[] texts = answerText.Split(',');
        Dictionary<int, int> tempAnswers = new Dictionary<int, int>();
        int tempId = 1;
        foreach (string s in texts)
        {
            if (string.IsNullOrEmpty(s.Trim()))
            {
                continue;
            }
            try
            {
                int value = System.Convert.ToInt32(s);
                tempAnswers.Add(tempId, value);
                tempId++;
            }
            catch
            {
                Debug.LogError("FormatException: " + s + " in " + tempId);
            }
        }

        foreach (int id in tempAnswers.Keys)
        {
            GridFlag gf = GridFlag.None;
            switch (tempAnswers[id])
            {
                case 1:
                    gf = GridFlag.Black;
                    break;
                case 2:
                    gf = GridFlag.Negation;
                    break;
                default:
                    gf = GridFlag.White;
                    break;
            }
            MainGame.SetOperationState(gf);
            MainGame.ChangeMyAnswer(id, gf);
            MainGame.SetOperationState(GridFlag.None);
        }

    }

    internal void ConvertAnswerToText()
    {
        maxColumns = MainGame.maxColumns;
        maxRows = MainGame.maxRows;
        List<string> xxx = new List<string>();
        StringBuilder sb = new StringBuilder();
        bool isFirst = true;
        for (int row = 1; row < maxRows + 1; row++)
        {
            for (int col = 1; col < maxColumns + 1; col++)
            {
                int tempId = MainGame.ConvertToGridID(col, row, maxColumns);
                AnswerData ad = MainGame.GetPuzzleAnswerByGridID(tempId);
                if (null == ad)
                {
                    Debug.LogError("save error!");
                    return;
                }
                int v = -1;
                if (ad.isBlocked)
                {
                    v = 2;
                }
                else
                {
                    v = ad.currentAnswer ? 1 : 0;
                }
                if (isFirst)
                {
                    isFirst = false;
                    sb.Append(v.ToString());
                }
                else
                {
                    sb.Append("," + v.ToString());
                }

            }
        }
        answerText = sb.ToString();

    }



    public void InitializeFromPlayerPrefs()
    {
        //throw new System.NotImplementedException();
        if (PlayerPrefs.HasKey(nameOfLevelID))
        {
            currentLevelID = PlayerPrefs.GetInt(nameOfLevelID);
            if (currentLevelID >= 0)
            {
                maxColumns = PlayerPrefs.GetInt(nameOfMaxColumns);
                maxRows = PlayerPrefs.GetInt(nameOfMaxRows);
                answerText = PlayerPrefs.GetString(nameOfAnswerText);
            }
            else
            {
                Debug.Log("currentLevelId<0");
            }
        }
        else
        {
            Debug.LogError("Load PlayerPrefs Failed!");
        }
    }


    public void WriteToPlayerPrefs()
    {
        if (MainGame.currentGameState == GameState.IsPuzzled)
        {
            RecordOnAnswerChanged();
        }
        else
        {
            RecordOnVictory();
        }
    }

    void RecordOnAnswerChanged()
    {
        //throw new System.NotImplementedException();
        if (MainGame.currentLevelData != null)
        {
            currentLevelID = MainGame.currentLevelData.levelID;
        }
        else
        {
            currentLevelID = -1;
        }

        ConvertAnswerToText();

        #region -------- write to playerPref ---------

        PlayerPrefs.SetInt(nameOfLevelID, currentLevelID);
        PlayerPrefs.SetInt(nameOfMaxColumns, maxColumns);
        PlayerPrefs.SetInt(nameOfMaxRows, maxRows);
        PlayerPrefs.SetString(nameOfAnswerText, answerText);
        #endregion

        //Debug.Log("save! " + (currentLevelID >= 0).ToString());

        //Debug.Log("str is " + answerText);
    }

    void RecordOnVictory()
    {
        Debug.Log("hmm...");
        answerText = "";
        maxColumns = 0; 
        maxRows = 0; 
        currentLevelID = -1;
        PlayerPrefs.SetInt(nameOfLevelID, -1);
        PlayerPrefs.SetInt(nameOfMaxColumns, 0);
        PlayerPrefs.SetInt(nameOfMaxRows, 0);
        PlayerPrefs.SetString(nameOfAnswerText, "");
    }
}

public class GameSavingData_Victory :ISavingData
{
    string nameOfPassedLevels = SavingDataManager.NameOfPassedLevels;

    public string passedLevelsText = "";

    List<int> myRecords = new List<int>();

    internal void ConvertTextToNumbers()
    {
        if (!string.IsNullOrEmpty(passedLevelsText))
        {
            List<int> tempNumbers = new List<int>();

            foreach (string s in passedLevelsText.Split(','))
            {
                if (string.IsNullOrEmpty(s.Trim()))
                {
                    continue;
                }
                try
                {
                    int n = System.Convert.ToInt32(s.Trim());
                    tempNumbers.Add(n);
                }
                catch
                {
                    tempNumbers.Clear();
                    Debug.LogError("Error: invaild passedLevel data");
                }
            }
            myRecords = tempNumbers;
        }
        
    }

    internal void ConvertNumberToText()
    {
        if (myRecords.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int n in myRecords)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(n.ToString());
            }
            this.passedLevelsText = sb.ToString();
        }
    }

    internal void AddVictoryRecord(int levelId)
    {
        if (!myRecords.Contains(levelId))
        {
            myRecords.Add(levelId);
        }
    }

    internal bool CheckExistRecord(int levelId)
    {
        return myRecords.Contains(levelId);
    }

    public void InitializeFromPlayerPrefs()
    {
        //throw new System.NotImplementedException();
        if (PlayerPrefs.HasKey(nameOfPassedLevels))
        {
            passedLevelsText = PlayerPrefs.GetString(nameOfPassedLevels);
            ConvertTextToNumbers();
        }
    }

    public void WriteToPlayerPrefs()
    {
        //throw new System.NotImplementedException();
        ConvertNumberToText();
        PlayerPrefs.SetString(nameOfPassedLevels, passedLevelsText);
    }
}

