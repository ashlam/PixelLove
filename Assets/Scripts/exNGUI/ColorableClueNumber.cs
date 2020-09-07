using UnityEngine;
using System.Collections;

/// <summary>
/// 可改变颜色的Clue数字
/// 这玩意得定位一个所属行/列数据，然后还得有一个标注它在行/列中的index，这样才能顺利地定位是否变灰
/// 
/// 2014-06-11
/// design by 顾文光
/// </summary>
public class ColorableClueNumber : MonoBehaviour {
    
    public bool isGray = false;
    public int number = -1;
    public int colIndex = -1;
    public int rowIndex = -1;
	public int arrayIndex= -1;
	
	public int belongedId=-1;
    public bool isHorizontal = false;

    TextMesh myTextMesh = null;
	Color grayColor = new Color(0.4f,0.4f,0.4f);
	Color defaultColor = Color.white;

	void Awake()
	{
		myTextMesh = GetComponent<TextMesh>();
	}

	// Use this for initialization
	void Start () {
//		SetGrayState(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void SetGrayState(bool isGray)
    {
        this.isGray = isGray;
		if(null != myTextMesh )
		{
			myTextMesh.color = isGray?  grayColor: defaultColor;
		}
    }

	internal void SetText(string text)
	{
		if(null != myTextMesh)
			myTextMesh.text = text;
		if(!string.IsNullOrEmpty (text.Trim ()))
		{
			this.number = System.Convert.ToInt16(text);
		}
	}
}