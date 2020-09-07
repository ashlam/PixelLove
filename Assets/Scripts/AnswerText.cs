using UnityEngine;
using System.Collections;

/// <summary>
/// 答案文字
/// 
/// 2014-06-06
/// design by 顾文光
/// </summary>
public class AnswerText : MonoBehaviour {

    public string text;
    public bool shouldDisplay = false;
    public bool isFadeIn = false;
    UILabel myLabel = null;
    
	// Use this for initialization
	void Start () {
        RevertAlphaToZero();
        myLabel = GetComponent<UILabel>();
        RevertAlphaToZero();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (isFadeIn && null != myLabel)
        {
            myLabel.color = new Color(myLabel.color.r, myLabel.color.g, myLabel.color.b, currentAlpha);
        }
	}

    void RevertAlphaToZero()
    {
        if (null != myLabel)
        {
            myLabel.color = new Color(myLabel.color.r, myLabel.color.g, myLabel.color.b, 0f);
        }
    }

    internal void StartDisplayAnswerText(string text)
    {
        RevertAlphaToZero();
        shouldDisplay = true;
        isFadeIn = true;
        if (myLabel != null)
        {
            myLabel.text = text; ;
        }
        StartCoroutine(FadeInAlpha());
    }

    float currentAlpha = 0f;
    IEnumerator FadeInAlpha()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (currentAlpha < 1f)
            {
                currentAlpha += 0.1f;
            }
            else
            {
                currentAlpha = 1f;
                yield break;
            }

        }
    }
}