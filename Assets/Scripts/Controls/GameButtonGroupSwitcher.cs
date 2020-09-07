using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏按钮组控制
/// 根据mainGame.isDone切换按钮组的状态
/// 
/// 2014-09-19
/// design by 顾文光
/// </summary>
public class GameButtonGroupSwitcher : MonoBehaviour {

    GameState lastState = GameState.NotGame;
    public Transform transGrounp1;
    public Transform transGrounp2;
	// Use this for initialization
	void Start () {
        ResetButtonGroupState();
	}
	
	// Update is called once per frame
	void Update () {

        if (lastState != MainGame.currentGameState)
        {
            ResetButtonGroupState();
            lastState = MainGame.currentGameState;
        }
	}

    private void ResetButtonGroupState()
    {
        //throw new System.NotImplementedException();
        if (MainGame.currentGameState== GameState.IsPuzzled)
        {
            transGrounp1.gameObject.SetActive(true);
            transGrounp2.gameObject.SetActive(false);
        }
        else
        {
            transGrounp1.gameObject.SetActive(false);
            transGrounp2.gameObject.SetActive(true);
        }
    }
}