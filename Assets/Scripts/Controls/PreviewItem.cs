using UnityEngine;
using System.Collections;

/// <summary>
/// 关卡预览
/// 
/// 脚本创建日期
/// design by 顾文光
/// </summary>
public class PreviewItem : MonoBehaviour {

    public LevelInfo info;
    public UITexture tex2dLogo;
    public UILabel lbLevelName;
    public UILabel lbChequerSize;
    public UILabel lbLevelId;
    public int indexForClassic = 1;
	// Use this for initialization
	void Start () {
        AutoSetInfoByLevelInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AutoSetInfoByLevelInfo()
    {
        if (info != null)
        {
            if (info.mainChequerObject != null)
            {
                Chequer cq = info.mainChequerObject.transform.GetChild(0).GetComponent<Chequer>();
                if (null != cq)
                {
                    lbChequerSize.text = string.Format("{0}x{1}", cq.columnCount, cq.rowCount);
                }
            }
            if (SavingDataManager.CreateInstance().CheckHasPassedLevel(info.levelID))//已通过这个关卡
            {
                tex2dLogo.mainTexture = info.previewTexture;
                lbLevelName.text = info.levelName;
            }
            else
            {
                lbLevelName.text = "???";
            }
            lbLevelId.text = string.Format("Level{0} - ", indexForClassic.ToString("00"));
        }
    }
}