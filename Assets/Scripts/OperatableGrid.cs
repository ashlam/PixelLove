using UnityEngine;
using System.Collections;

/// <summary>
/// 可操作的格子
/// 具有以下属性：
/// 1.ID（编号），和GridData对应
/// 2.当前操作值，用来表示黑白状态（false＝白；true＝黑）
/// 
/// 然后，每个格子具有以下行为：
/// 1.接受和响应操作
/// 2.更改自己的显示信息/视觉表现
/// 3.将操作结果提交给MainGame，以检索是否完成游戏，或更改Clue的状态
/// 
/// 2014-05-26
/// design by 顾文光
/// </summary>
public class OperatableGrid : MonoBehaviour
{

    public int ID;
    GridFlag operationState = GridFlag.White;
    bool shouldChangeColor = false;
    internal bool isControlable = true;

    SpriteRenderer mySpriteRender = null;
    public Sprite sampleWhiteSprite;
    public Sprite sampleNegationSprite;

    void Awake()
    {
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        //mySpriteRender = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {

        if (MainGame.couldControl)
        {
            #region --------------- old --------------
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckIsInGridRange())
                {
                    if (!MainGame.CheckIsContinousOperation() && MainGame.CheckCouldModifyGridFlag(operationState)) //没有连续操作/没有进行任何操作
                    {
                        operationState = MainGame.GetNextGridFlag(operationState); //得到当前格的反操作（黑变白，白变黑，x变白，白变x）
                        MainGame.StartContinousOpeartion(operationState);//画
                        shouldChangeColor = true;
                        //Debug.Log(string.Format("{0} - {1}", ID, operationState.ToString()));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (MainGame.CheckIsContinousOperation())
                {
                    MainGame.StopContinuousOperation();
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (CheckIsInGridRange())
                {
                    if (MainGame.CheckIsContinousOperation())
                    {
                        GridFlag gameOperationState = MainGame.GetContinousGridFlag();
                        if (operationState != gameOperationState)
                        {
                            operationState = MainGame.GetNextGridFlag(operationState);
                            shouldChangeColor = true;
                        }
                    }
                }
            }

            #endregion

        }
        SetAndDisplayNewColor();

    }

    private void SetAndDisplayNewColor()
    {
        if (shouldChangeColor)
        {
            MainGame.ChangeMyAnswer(this.ID, this.operationState);

            DisplayNewColor();

            if (MainGame.currentGameState == GameState.IsPuzzled)
            {
                SavingDataManager.CreateInstance().Save();
            }
            else
            {
                //SavingDataManager.CreateInstance().SaveToVictory();
            }
        }
    }

    bool CheckIsInGridRange()
    {
        bool result = false;
        Vector2 mousePos2d = TeaSoft.MouseHelper.GetMousePositionInViewport2D(Camera.main);
        if (Vector2.Distance(this.transform.position, mousePos2d) <= 0.5)
        {
            result = true;
        }
        return result;
    }

    internal void RefreshByAnswer()
    {
        GridFlag gf = GridFlag.None;
        AnswerData ad = MainGame.GetPuzzleAnswerByGridID(this.ID);
   
        gf = ad.isBlocked ? GridFlag.Negation : (ad.currentAnswer ? GridFlag.Black : GridFlag.White);
        operationState = gf;
        //Debug.Log(string.Format("id={0},gridFlag={1}", ID, operationState.ToString()));
        DisplayNewColor();
    }

    void DisplayNewColor()
    {
        if (null != mySpriteRender)
        {
            if (operationState == GridFlag.Negation)
            {
                mySpriteRender.sprite = sampleNegationSprite;
                mySpriteRender.color = Color.white;
            }
            else
            {
                mySpriteRender.sprite = sampleWhiteSprite;
                mySpriteRender.color = (operationState == GridFlag.Black) ? Color.black : Color.white;
            }
            shouldChangeColor = false;
            //Debug.Log("should insert checking clueColor at here");
            MainGame.TryToSetClueTextColor(this.ID);
        }
    }
}