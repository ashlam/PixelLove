using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 线索
/// 线索是包含一系列数字的对象，用于指示当前场上的情况
/// 线索对应棋盘的某一行或一列，需要设计一个id之类的索引，以便检索
/// 
/// 2014-05-29
/// design by 顾文光
/// </summary>
public class Clue : MonoBehaviour
{

    List<int> clueValues = null;

    TextMesh myText = null;

    public int row = -1;
    public int col = -1;
    public bool isHorizontal = false;

    //    public TextMesh sampleOfText;
    public ColorableClueNumber sampleOfText;
    public SpriteRenderer background;
    List<int> managedGrids = new List<int>();

    internal List<string> numberList = new List<string>();

    // Use this for initialization
    void Start()
    {
        //managedGrids = new List<int>();
        myText = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void AddManagedGrid(int gridId)
    {
        //throw new System.NotImplementedException();
        if (!managedGrids.Contains(gridId))
            managedGrids.Add(gridId);
    }


    internal void CreateClueText()
    {
        //throw new System.NotImplementedException();
        managedGrids.Sort(SortManageredGridDesc);
        int chainedCount = 0;
        //List<string> clueText = new List<string>();
        numberList.Clear();
        for (int i = 0; i < managedGrids.Count; i++)
        {
            int gridId = managedGrids[i];
            AnswerData answerData = MainGame.GetPuzzleAnswerByGridID(gridId);
            if (null != answerData)
            {
                if (answerData.standardAnswer) //有值
                {
                    if (chainedCount == 0 && numberList.Count > 0) //不是第一次
                    {
                        //加一个空格
                        numberList.Insert(0, " ");
                    }
                    chainedCount++;
                }
                else // 没值
                {
                    if (chainedCount > 0)
                    {
                        //表示遇到空格了，chainedCount计数归零
                        numberList.Insert(0, chainedCount.ToString());
                        chainedCount = 0;
                    }
                }

                if (chainedCount > 0 && i == managedGrids.Count - 1) //判断最后一格的检测情况
                {
                    numberList.Insert(0, chainedCount.ToString());
                }
            }
        }

        CreateClueTextObject(numberList);
        //ShowNumberListForDebug();
    }

    void ShowNumberListForDebug()
    {
        Debug.Log(">> show clue " + (this.isHorizontal ? "row " : "col ") + (this.isHorizontal ? row : col).ToString());
        foreach (string s in numberList)
        {
            Debug.Log(s);
        }
        Debug.Log("<< -done- >>");
    }

    int maxHorizontalCount = 0;
    int maxVerticalCount = 0;
    private void CreateClueTextObject(List<string> clueTexts)
    {
        //throw new System.NotImplementedException();
        float currentLocalPos = -0.25f;
        if (null != sampleOfText)
        {
            int numberCount = 0;
            int clueTextIndex = clueTexts.Count - 1;
            for (int i = 0; i < clueTexts.Count; i++)
            {
                string s = clueTexts[i];
                if (string.IsNullOrEmpty(s.Trim()))
                    continue;
                ColorableClueNumber clonedText = GameObject.Instantiate(sampleOfText) as ColorableClueNumber;

                clonedText.arrayIndex = (clueTextIndex + 1) / 2;
                clueTextIndex -= 2;

                clonedText.SetText(s);
                clonedText.isHorizontal = isHorizontal;
                if (this.isHorizontal)
                {
                    clonedText.rowIndex = this.row;
                    //MainGame.SetClueTextRecord(isHorizontal, clonedText.rowIndex, System.Convert.ToInt32(s));            
                }
                else
                {
                    clonedText.colIndex = this.col;
                    //MainGame.SetClueTextRecord(isHorizontal, clonedText.colIndex, System.Convert.ToInt32(s));
                }
                

                clonedText.transform.parent = this.transform;
                clonedText.transform.localPosition = isHorizontal ? new Vector2(-currentLocalPos, 0) : new Vector2(0, currentLocalPos);

                MainGame.SetClueTextRecord(clonedText);

                currentLocalPos += 0.5f;
                numberCount++;
                if (isHorizontal)
                {
                    maxHorizontalCount = Mathf.Max(numberCount, maxHorizontalCount);
                }
                else
                {
                    maxVerticalCount = Mathf.Max(numberCount, maxVerticalCount);
                }
            }

            //CreateClueBackground(this.transform, numberCount);
        }
    }


    void CreateClueBackgroundObject(Transform transParent, int numberCount)
    {
        if (null != background)
        {
            float height = 0.25f;
            SpriteRenderer mySprite = GameManager.Instantiate(background) as SpriteRenderer;
            Vector3 tempPos = new Vector3(isHorizontal ? 0.49f : 0f, isHorizontal ? 0f : -0.49f, 0.01f);
            mySprite.transform.parent = transParent;
            mySprite.transform.localPosition = tempPos;
            if (isHorizontal)
            {
                mySprite.transform.Rotate(Vector3.forward, 90);
            }
            if (numberCount > 0)
            {
                mySprite.transform.localScale = new Vector3(0.96f, 1 + (numberCount - 1) * height, 1);
            }
        }
    }


    int SortManageredGridDesc(int id1, int id2)
    {
        return id1.CompareTo(id2);
    }

    internal void CreateBackground(int textCount)
    {
        //throw new System.NotImplementedException();
        CreateClueBackgroundObject(this.transform, textCount);
    }

    /*
     * - 变灰
     * 以下情况会导致clue变灰：
     * 1.涂抹过的格子，数量和顺序与clueValues相匹配
     * 2.第一波涂抹，数量与clueValues的第一条数据匹配？
     * 3.最后一波涂抹，数量与clueValue的最后一条数据匹配
     * 
     * 设计思路：
     * 0.根据格的行列数据找到对应的两个Clue
     * 1.找到Clue的(index of clueValue),命名为t
     * 2.正序检测一遍，依次检测前面是否存在空格，if(true)则进入2，else则监控matched数量是否与t相符，matched匹配则变灰
     * 3.倒序检测一遍，同上
     */
}