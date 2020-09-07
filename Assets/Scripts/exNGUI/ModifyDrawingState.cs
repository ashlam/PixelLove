using UnityEngine;
using System.Collections;

/// <summary>
/// 修改当前的绘制状态（Pen/Negation）
/// For NGUI only,
/// For this Game only.
/// 
/// 2014-06-10
/// design by 顾文光
/// </summary>
public class ModifyDrawingState : MonoBehaviour {

    public OperationState state;
    NonSpringButton myButton = null;

    // Use this for initialization
    void Start()
    {
        myButton = GetComponent<NonSpringButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainGame.GetCurrentOperationState() != state)
        {
            if (null != myButton && !myButton.isPopup)
            {
                myButton.isPopup = true;
            }
        }
    }

    void OnClick()
    {
        MainGame.SetOperationState(state);
    }
}