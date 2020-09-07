using UnityEngine;
using System.Collections;

/// <summary>
/// 棋盘
/// 2014-05-24
/// design by 顾文光
/// </summary>
public class Chequer : MonoBehaviour {

    public int columnCount = 5;
    public int rowCount = 5;
    public string LevelName = "关卡名称";
    public Color bgColor = Color.white;

    public GridData originalGridSample = null;
    public OperatableGrid originOperatableGrid = null;
    public Clue originalClueSample = null;
    public SpriteRenderer originalBorderSample = null;

    public Transform transOfDataRoot = null;
    public Transform transOfGridRoot = null;
	public Transform transOfClueRoot = null;
    public Transform transOfAnswerText = null;
    public Transform transOfBorders = null;
    

	// Use this for initialization
    void Start()
    {
        #region ------- init some of root transform -------
        if (null == transOfDataRoot)
        {
            transOfDataRoot = this.transform;
        }
        if (null == transOfGridRoot)
        {
            transOfGridRoot = this.transform;
        }
        if (null == transOfClueRoot)
        {
            transOfClueRoot = this.transform;
        }
        if (null == transOfAnswerText)
        {
            transOfAnswerText = this.transform;
        }
        if (null == transOfBorders)
        {
            transOfBorders = this.transform;
        }
        #endregion

        MainGame.maxColumns = this.columnCount;
        MainGame.maxRows = this.rowCount;
        MainGame.currentChequer = this;

        SetPuzzleAnswersToGame();
        //注意：OperatableGrid和Clue都是基于MainGame的Answer而生成的，注意先后顺序
        AutoCreateOperatableGrid();
        AutoCreateClues();


        //建议在这儿加load，并refresh——或者，至少要在这之前load完玩家偏好
        {
            Debug.Log("ido , i do");
            SavingDataManager.CreateInstance().Load();
            this.RefreshOperatableGrids();
        }

        HideDataOnGameStart();
        MainGame.couldControl = true;
    }

	void HideDataOnGameStart()
	{
//		transOfDataRoot.gameObject.SetActive (false);
		foreach(GridData gd in transOfDataRoot.GetComponentsInChildren<GridData>())
		{
			gd.gameObject.SetActive (false);
		}
	}


    private void SetPuzzleAnswersToGame()
    {
        //throw new System.NotImplementedException();
        GridData[] datas = this.transOfDataRoot.GetComponentsInChildren<GridData>();
        //Debug.Log((datas == null) ? -1 : datas.Length);
        MainGame.SetPuzzleAnswer(datas);
    }

    private void AutoCreateOperatableGrid()
    {
        if (null != this.originalGridSample)
        {
            GridData[] datas = this.transOfDataRoot.GetComponentsInChildren<GridData>();
            foreach (GridData gd in datas)
            {
                OperatableGrid clonedGrid = GameObject.Instantiate(originOperatableGrid) as OperatableGrid;
                clonedGrid.ID = gd.ID;
                clonedGrid.transform.parent = transOfGridRoot;
                clonedGrid.transform.localPosition = new Vector2(gd.transform.localPosition.x, gd.transform.localPosition.y);
                clonedGrid.name = string.Format("Grid{0} ({1},{2})", clonedGrid.ID.ToString("00"), (int)clonedGrid.transform.localPosition.x, -(int)clonedGrid.transform.localPosition.y);

                CreateBorderObject(this.transOfBorders, clonedGrid.transform.localPosition);
            }
        }
    }

    void CreateBorderObject(Transform rootOfBorders,Vector3 localPosition)
    {
        if (null != originalBorderSample)
        {
            SpriteRenderer clonedBorder = GameObject.Instantiate(originalBorderSample) as SpriteRenderer;
            clonedBorder.transform.parent = rootOfBorders;
            clonedBorder.transform.localPosition = localPosition;
        }
    }

    private void AutoCreateClues()
    {
        //throw new System.NotImplementedException();
        if (null != originalClueSample)
        {
            int maxVerticalTextCount = 0;
            int maxHorizontalTextCount = 0;
            //MainGame.ResetClueTextDatas ();	//clean exists clueTexts;
            Debug.Log("should clear colorTexts from mainGame");
            MainGame.ClearAllColorClueNumbers();

            #region ------------- vertical clues -------------
            //创建每一列的Clue
            for (int col = 1; col < columnCount + 1; col++)
            {
                Clue clonedClue = GameObject.Instantiate(originalClueSample) as Clue;//创建对象
                //赋根节点
                clonedClue.transform.parent = transOfClueRoot;
                //赋位置
                clonedClue.col = col;
                clonedClue.isHorizontal = false;
                clonedClue.transform.localPosition = new Vector2(col, 0);
                clonedClue.name = "Clue(col)" + col.ToString("00");
                //赋值（文字）

                for (int tempRowIndex = 1; tempRowIndex < rowCount + 1; tempRowIndex++)
                {
                    int tempGridId = MainGame.ConvertToGridID(col, tempRowIndex, columnCount);
                    clonedClue.AddManagedGrid(tempGridId);

                }
                clonedClue.CreateClueText();
                maxVerticalTextCount = Mathf.Max(maxVerticalTextCount, clonedClue.numberList.Count);
            }


            #endregion

            #region ----------- horizontal clues -------------
            //创建每一行的Clue
            for (int row = 1; row < rowCount + 1; row++)
            {
                Clue clonedClue = GameObject.Instantiate(originalClueSample) as Clue;//创建对象
                //赋根节点
				clonedClue.transform.parent = transOfClueRoot;
                //赋位置
                clonedClue.row = row;
                clonedClue.isHorizontal = true;
                clonedClue.transform.localPosition = new Vector2(0, -row);
                clonedClue.name = "Clue(row)" + row.ToString("00");
                //赋值（文字）
                for (int tempColumnIndex = 1; tempColumnIndex < columnCount + 1; tempColumnIndex++)
                {
                    int tempGridId = MainGame.ConvertToGridID(tempColumnIndex, row, columnCount);
                    clonedClue.AddManagedGrid(tempGridId);

                }
                clonedClue.CreateClueText();
                maxHorizontalTextCount = Mathf.Max(maxHorizontalTextCount, clonedClue.numberList.Count);
            }

            #endregion

            foreach(Clue tempClue in transOfClueRoot.GetComponentsInChildren<Clue>())
            {
                int textCount = tempClue.isHorizontal ? maxHorizontalTextCount : maxVerticalTextCount;
                tempClue.CreateBackground(textCount);
            }
            
        }
	}

    internal void ShowAnswerData()
    {
        //throw new System.NotImplementedException ();
        Debug.Log("you win , congratulations !");
        MainGame.couldControl = false;
        spanedTime = 0f;

        GridData[] tempGridDatas = transOfDataRoot.GetComponentsInChildren<GridData>(true);
        interval = tempGridDatas.Length > 0 ? totalTime / tempGridDatas.Length : interval;
        foreach (GridData go in tempGridDatas)
        {
            StartCoroutine(DelayShowAnswerData(go));
        }

        StartCoroutine(DelayShowAnswerText(tempGridDatas.Length * interval));

        transOfClueRoot.gameObject.SetActive(false);
    }

	float interval = 0.05f;
	float spanedTime = 0f;
    float totalTime = 2f;

	IEnumerator DelayShowAnswerData(GridData go)
	{
		spanedTime +=interval;
		yield return new WaitForSeconds(spanedTime);
		go.gameObject.SetActive(true);

	}

    IEnumerator DelayShowAnswerText(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AnswerText answerText = transOfAnswerText.GetComponentInChildren<AnswerText>();
        if (null != answerText)
        {
            //answerText.StartDisplayAnswerText(this.LevelName);
            string myLevelName = MainGame.currentLevelData == null ? this.LevelName : MainGame.currentLevelData.levelName;
            answerText.StartDisplayAnswerText(myLevelName);
        }
    }



    internal void RefreshOperatableGrids()
    {
        foreach (OperatableGrid ogd in this.GetComponentsInChildren<OperatableGrid>())
        {
            ogd.RefreshByAnswer();
        }
        Debug.Log("refeshed");
    }
}