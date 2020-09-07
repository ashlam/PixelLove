using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主游戏
///（以及一些与游戏逻辑有关的函数）
/// 2014-05-24
/// design by 顾文光
/// </summary>
public class MainGame
{
    internal static GameState currentGameState = GameState.NotGame;
    internal static int maxRows, maxColumns;
    internal static Chequer currentChequer = null;

    internal static bool couldControl = false;
    internal static LevelInfo currentLevelData = null;

    /// <summary>
    /// 将行列转换成id，行列均从1开始
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <param name="maxColumns"></param>
    /// <returns></returns>
    internal static int ConvertToGridID(int col, int row, int maxColumns)
    {
        int result = -1;
        result = (row - 1) * maxColumns + col;
        return result;
    }

    internal static Vector2 ConvertToLocation(int gridID, int maxColumns)
    {
        Vector2 result = new Vector2(-1, -1);
        int col = (gridID - 1) % maxColumns + 1;
        int row = (gridID - 1) / maxColumns + 1;
        result.x = col;
        result.y = row;
        return result;
    }

    static Dictionary<int, AnswerData> myPuzzleAnswers = null;

    internal static void SetPuzzleAnswer(GridData[] datas)
    {

        //throw new System.NotImplementedException();
        List<GridData> tempArray = new List<GridData>(datas);
        tempArray.Sort(SortGridDatasByIdAnsc);

        myPuzzleAnswers = new Dictionary<int, AnswerData>();

        for (int i = 0; i < tempArray.Count; i++)
        {
            GridData data = tempArray[i];
            AnswerData tempAnswer = new AnswerData()
            {
                ID = data.ID,
                standardAnswer = data.IsColored,
            };
            myPuzzleAnswers.Add(data.ID, tempAnswer);
        }

        //DisplayAnswerOnConsole();
    }

    internal static AnswerData GetPuzzleAnswerByGridID(int gridId)
    {
        AnswerData result = null;
        if (myPuzzleAnswers.ContainsKey(gridId))
        {
            result = myPuzzleAnswers[gridId];
        }
        return result;
    }


    static int SortGridDatasByIdAnsc(GridData data1, GridData data2)
    {
        return data1.ID.CompareTo(data2.ID);
    }

    static void DisplayAnswerOnConsole()
    {
        bool a = false;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (AnswerData re in myPuzzleAnswers.Values)
        {
            if (!a)
            {
                a = true;
                sb.Append(re.standardAnswer.ToString());
            }
            else
            {
                sb.Append(", ");
                sb.Append(re.standardAnswer.ToString());
            }
        }
        Debug.Log(sb.ToString());
    }



    static GridFlag operatingGridFlag = GridFlag.None;


    internal static bool CheckIsContinousOperation()
    {
        //throw new System.NotImplementedException();
        bool result = false;
        result = operatingGridFlag != GridFlag.None;
        return result;
    }

    internal static void StartContinousOpeartion(GridFlag gridOpeationState)
    {
        //throw new System.NotImplementedException();
        operatingGridFlag = gridOpeationState;
    }

    internal static void SetOperationState(GridFlag operationFlag)
    {
        operatingGridFlag = operationFlag;
    }


    internal static void StopContinuousOperation()
    {
        //throw new System.NotImplementedException();
        operatingGridFlag = GridFlag.None;
    }

    internal static GridFlag GetContinousGridFlag()
    {
        //throw new System.NotImplementedException();
        return operatingGridFlag;
    }


    //internal static bool isNegationOperation = false;

    internal static bool CheckCouldModifyGridFlag(GridFlag opState)
    {
        bool result = true;

        if (CheckIsNegationState()) //如果当前是画x状态，那么x不会影响black
        {
            result = (opState != GridFlag.Black);
        }
        else //如果当前不是画x状态，那么black不会影响x
        {
            result = (opState != GridFlag.Negation);
        }

        return result;
    }

    internal static GridFlag GetNextGridFlag(GridFlag opState)
    {
        GridFlag result = GridFlag.None;
        switch (opState)
        {
            case GridFlag.Black:
                result = CheckIsNegationState() ? GridFlag.Black : GridFlag.White;
                break;
            case GridFlag.Negation:
                result = CheckIsNegationState() ? GridFlag.White : GridFlag.Negation;
                break;
            case GridFlag.White:
                result = CheckIsNegationState() ? GridFlag.Negation : GridFlag.Black;
                break;
            default:
                result = opState;
                break;
        }
        return result;
    }



    //    internal static void ChangeMyAnswer(int id, bool isColored)
    internal static void ChangeMyAnswer(int id, GridFlag operationState)
    {
        //throw new System.NotImplementedException();
        AnswerData tempAnswer = myPuzzleAnswers[id];
        if (null != tempAnswer)
        {
            currentGameState = GameState.IsPuzzled;//设定此时状态为‘正在游戏’
            if (operatingGridFlag == GridFlag.Negation)
            {
                tempAnswer.isBlocked = true;
            }
            else
            {
                
                if (CheckIsNegationState() && operationState == GridFlag.White)
                {
                    tempAnswer.isBlocked = false;
                }
                else if (!CheckIsNegationState() && !tempAnswer.isBlocked)
                {
                    bool isColored = operationState == GridFlag.White ? false : true;
                    tempAnswer.currentAnswer = isColored;

                    if (tempAnswer.isBlocked)
                    {
                        Debug.LogError(string.Format("something wrong with ad {0}", id));
                    }
                }
                if (tempAnswer.currentAnswer == tempAnswer.standardAnswer)
                {
                    TryToInvokeGameWinEvent();
                }
            }
        }
    }

    static void TryToInvokeGameWinEvent()
    {
        bool allMatched = true;
        foreach (AnswerData answer in myPuzzleAnswers.Values)
        {
            if (answer.standardAnswer != answer.currentAnswer)
            {
                allMatched = false;
                break;
            }
        }
        if (allMatched)
        {
            //Debug.Log("you win , congratulations !");
            if (null != currentChequer)
            {
                currentChequer.ShowAnswerData();
            }
            currentGameState =  GameState.Victory;
            if (null != currentLevelData)
            {
                SavingDataManager.CreateInstance().SaveToVictory();
            }
        }
    }

    static OperationState currentOperationState = OperationState.Pen;
    internal static void SetOperationState(OperationState state)
    {
        //throw new System.NotImplementedException();
        currentOperationState = state;
    }

    internal static OperationState GetCurrentOperationState()
    {
        return currentOperationState;
    }

    static bool CheckIsNegationState()
    {
        return currentOperationState == OperationState.Negation;
    }



    internal static void TryToSetClueTextColor(int operatedGridId)
    {
        //根据id换算出col和row
        //根据row遍历两次（正反）
        //再根据col遍历两次（正反）

        Vector2 localPos = MainGame.ConvertToLocation(operatedGridId, MainGame.maxColumns);
        int currentColIndex = (int)localPos.x;
        int currentRowIndex = (int)localPos.y;
        List<ColorableClueNumber> myRowClues = GetClueTextDatas(true, currentRowIndex); //某行的clueTextData
        ApplyGrayNumbers(currentRowIndex, true, myRowClues);

        List<ColorableClueNumber> myColClues = GetClueTextDatas(false, currentColIndex); //某列的clueTextData
        ApplyGrayNumbers(currentColIndex, false, myColClues);
    }

    #region ----------- 各种计算ColoredClueNumber的方法，超烦，我快晕死了... --------------------
    static void ApplyGrayNumbers(int fixedRowOrColIndex, bool isFixedRow, List<ColorableClueNumber> scaningClues)
    {
        List<int> grayIndex = new List<int>();
        //grayIndex = GetGrayClueIndexsByCurrentAnswer(fixedRowOrColIndex, isFixedRow, scaningClues);
        if (grayIndex.Count != scaningClues.Count)
        {
            InsertElementIntoArray(ref grayIndex, GetGrayClueIndexes(fixedRowOrColIndex, isFixedRow, scaningClues, true));
            InsertElementIntoArray(ref grayIndex, GetGrayClueIndexes(fixedRowOrColIndex, isFixedRow, scaningClues, false));
        }
        for (int i = 0; i < scaningClues.Count; i++)
        {
            ColorableClueNumber c = scaningClues[i];
            if (grayIndex.Contains(i))
            {
                c.SetGrayState(true);
            }
            else
            {
                c.SetGrayState(false);
            }
        }
        foreach (int i in grayIndex)
        {
            scaningClues[i].SetGrayState(true);
        }        
    }

    static void InsertElementIntoArray(ref List<int> finalResult, List<int> array)
    {
        foreach(int n in array)
        {
            if (!finalResult.Contains(n))
                finalResult.Add(n);
        }
    }

    static List<int> GetGrayClueIndexes(int fixedRowOrColIndex, bool isFixedRow, List<ColorableClueNumber> scaningClues, bool isAsc)
    {
        List<int> result = new List<int>();
        int lastValue = -1;
        int chainedCount = 0;
        int selectClueIndex = isAsc ? 0 : scaningClues.Count - 1;
        int maxRowOrColIndex = (isFixedRow ? (MainGame.maxColumns + 1) : (MainGame.maxRows + 1));
        for (
            int tempColOrRowIndex = isAsc ? 1 : maxRowOrColIndex - 1;
            isAsc ? (tempColOrRowIndex < maxRowOrColIndex) : (tempColOrRowIndex >= 1);
            tempColOrRowIndex = isAsc ? tempColOrRowIndex + 1 : tempColOrRowIndex - 1)
        {
            int gridId = -1;
            if (isFixedRow)
            {
                gridId = MainGame.ConvertToGridID(tempColOrRowIndex, fixedRowOrColIndex, MainGame.maxColumns);
            }
            else
            {
                gridId = MainGame.ConvertToGridID(fixedRowOrColIndex, tempColOrRowIndex, MainGame.maxColumns);
            }

            if (myPuzzleAnswers.ContainsKey(gridId))
            {
                if (isAsc ? (selectClueIndex > scaningClues.Count - 1) : (selectClueIndex < 0))
                {
                    break;
                }

                AnswerData ad = myPuzzleAnswers[gridId];
                //Debug.Log(string.Format("AD id={0},isAnswered={1},isBlocked={2}", gridId, ad.currentAnswer, ad.isBlocked));
                if (ad.currentAnswer)
                {
                    chainedCount++;
                    if ((isAsc && tempColOrRowIndex == (maxRowOrColIndex - 1)) || (!isAsc && tempColOrRowIndex ==1))
                    {
                        if (chainedCount == scaningClues[selectClueIndex].number)
                        {
                            result.Add(selectClueIndex);
                        }
                        else
                        {
                        }
                    }
                }
                else if (ad.isBlocked)
                {
                    if (lastValue == 1)
                    {
                        if (chainedCount == scaningClues[selectClueIndex].number)
                        {
                            result.Add(selectClueIndex);
                        }

                        chainedCount = 0;
                        selectClueIndex = isAsc ? selectClueIndex + 1 : selectClueIndex - 1;
                    }
                }
                else if (!ad.currentAnswer)
                {
                    if (lastValue == 1 && (chainedCount == scaningClues[selectClueIndex].number))
                    {
                        result.Add(selectClueIndex);
                        selectClueIndex = isAsc ? selectClueIndex + 1 : selectClueIndex - 1;
                        chainedCount = 0;
                    }
                    break;
                }
                lastValue = (ad.isBlocked) ? 2 : ad.currentAnswer ? 1 : 0;

            }
        }

        return result;
    }

    static List<int> GetGrayClueIndexsByCurrentAnswer(int fixedRowOrColIndex, bool isFixedRow, List<ColorableClueNumber> scaningClues)
    {
        //同样的，得到id->得到currentAnswer->到达断点时，if数量匹配，then add，else break;
        //所以你要先有个id
        int chainedCount = 0;
        int selectClueIndex = 0; 
        int maxRowOrColIndex = (isFixedRow ? (MainGame.maxColumns + 1) : (MainGame.maxRows + 1));
        int lastValue = -1;
        List<int> result = new List<int>();
        for (int tempColOrRowIndex = 1; tempColOrRowIndex < maxRowOrColIndex; tempColOrRowIndex++)
        {
            int gridId = -1;
            if (isFixedRow)
            {
                gridId = MainGame.ConvertToGridID(tempColOrRowIndex, fixedRowOrColIndex, MainGame.maxColumns);
            }
            else
            {
                gridId  = MainGame.ConvertToGridID(fixedRowOrColIndex, tempColOrRowIndex, MainGame.maxColumns);
            }
            if (myPuzzleAnswers.ContainsKey(gridId))
            {
                AnswerData ad = myPuzzleAnswers[gridId];

                if (selectClueIndex > scaningClues.Count)
                {
                    result.Clear();
                    break;
                }

                //bool isBreaking = false;
                if (ad.currentAnswer)
                {
                    chainedCount++;
                    if (selectClueIndex >= scaningClues.Count)
                    {
                        result.Clear();
                        break;
                    }
                    else if (tempColOrRowIndex == maxRowOrColIndex - 1 && chainedCount == scaningClues[selectClueIndex].number)
                    {
                         result.Add(selectClueIndex);
                    }
                }
                else
                {
                    if (lastValue == 1)
                    {
                        if (selectClueIndex < scaningClues.Count && scaningClues[selectClueIndex].number == chainedCount)
                        {
                            //Debug.Log("add selecteIndex : " + selectClueIndex + ", current chainCount = " + chainedCount);
                            result.Add(selectClueIndex);
                            selectClueIndex++;
                            chainedCount = 0;
                        }
                        else
                        {
                            result.Clear();
                            break;
                        }
                    }
                   
                }
                lastValue = ad.currentAnswer ? 1 : 0;
            }
        }
        if (result.Count != scaningClues.Count)
            result.Clear();
        return result;
    }

    static List<ColorableClueNumber> colorClueNumbers = new List<ColorableClueNumber>();
    internal static void SetClueTextRecord(ColorableClueNumber coloredNumber)
    {
        //throw new System.NotImplementedException();
        if (coloredNumber != null)
        {
            colorClueNumbers.Add(coloredNumber);
        }
    }

    internal static List<ColorableClueNumber> GetClueTextDatas(bool isHorizontal, int index)
    {
        List<ColorableClueNumber> result = new List<ColorableClueNumber>();

        foreach (ColorableClueNumber ctd in colorClueNumbers)
        {
            if (ctd.isHorizontal == isHorizontal)
            {
                if (isHorizontal && ctd.rowIndex == index)
                {
                    result.Add(ctd);
                }
                else if (!isHorizontal && ctd.colIndex == index)
                {
                    result.Add(ctd);
                }
            }
        }
        result.Sort(ComparedByArrayIndex);
        return result;
    }

    static int ComparedByArrayIndex(ColorableClueNumber n1, ColorableClueNumber n2)
    {
        return n1.arrayIndex.CompareTo(n2.arrayIndex);
    }
    #endregion





    internal static void ClearAllColorClueNumbers()
    {
        //throw new System.NotImplementedException();
        colorClueNumbers.Clear();
    }

    internal static NTopCameraController topLayerCamera = null;
    internal static void ResumeGame()
    {
        //throw new System.NotImplementedException();
        //isPaused = false;
        couldControl = true;
        if (null != topLayerCamera)
        {
            topLayerCamera.HideTopWindow();
        }
    }
    //static bool isPaused = false;
    internal static void PauseGame()
    {
        //isPaused = true;
        couldControl = false;
        if (null != topLayerCamera)
        {
            topLayerCamera.ShowTopWindow();
        }
    }

    internal static void RestartGame()
    {
        //throw new System.NotImplementedException();
        couldControl = true;
        
        foreach (int id in myPuzzleAnswers.Keys)
        {
            myPuzzleAnswers[id].isBlocked = false;
            myPuzzleAnswers[id].currentAnswer = false;
        }

        SavingDataManager.CreateInstance().ClearCurrentLevel();
        if (currentChequer != null)
        {
            currentChequer.RefreshOperatableGrids();
        }
        if (null != topLayerCamera)
        {
            topLayerCamera.HideTopWindow();
        }
        operatingGridFlag = GridFlag.White;
        currentOperationState = OperationState.Pen;
        currentGameState = GameState.IsPuzzled;

        Application.LoadLevel(GameConstants.MainSceneName);

    }

    internal static void GoNextGame()
    {
        //throw new System.NotImplementedException();
        if (currentLevelData != null)
        {
            Debug.Log("xxxa");
            if (currentLevelData.nextLevel != null)
            {
                GoToLevel(currentLevelData.nextLevel);
            }
            else
            {
                BackMainMenu(); 
            }
        }
    }

    internal static void GoToLevel(LevelInfo levelInfo)
    {
        //throw new System.NotImplementedException();
        currentLevelData = levelInfo;
        Application.LoadLevel(GameConstants.MainSceneName);
        currentGameState = GameState.IsPuzzled;
    }


    internal static void BackMainMenu()
    {
        currentGameState = GameState.NotGame;
        Application.LoadLevel(GameConstants.LevelSelectionName);
    }
}

public class AnswerData
{
    internal int ID;
    internal bool standardAnswer;
    internal bool currentAnswer;
    internal bool isBlocked = false;
}