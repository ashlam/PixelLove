using UnityEngine;
using System.Collections;

/// <summary>
/// 不会弹起的按钮，一般用来（以按钮）表示状态
/// 
/// 2014-06-09
/// design by 顾文光
/// </summary>
public class NonSpringButton : MonoBehaviour {

    public UISprite spriteForPopup;
    public UISprite spriteForPressed;
    public bool isPopup = true;
	// Use this for initialization
	void Start () {
        //myButton = GetComponent<UIButton>();
        //ShowSpriteByPopupState();
	}

    bool shouldRefresh = false;
    void Update()
    {
        if (isPopup && null != spriteForPopup && !spriteForPopup.enabled)
        {
            //spriteForPopup.enabled = true;
            shouldRefresh = true;
        }
        else if (!isPopup && null != spriteForPressed && !spriteForPressed.enabled)
        {
            //spriteForPressed.enabled = true;
            shouldRefresh = true;
        }
        if (shouldRefresh)
        {
            shouldRefresh = false;
            ShowSpriteByPopupState();
        }
    }

    void OnClick()
    {
        //isPopup = !isPopup;// 
        if (isPopup)
        {
            isPopup = false;
        }
        //ShowSpriteByPopupState();
    }

    void ShowSpriteByPopupState()
    {
            if (null != spriteForPopup)
            {
                spriteForPopup.enabled = isPopup;
            }
            if (null != spriteForPressed)
            {
                spriteForPressed.enabled = !isPopup;
            }
    }
}