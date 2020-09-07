using UnityEngine;
using System.Collections;

/// <summary>
/// Preview管理器
/// 
/// 2014-09-18
/// design by 顾文光
/// </summary>
public class PreviewManager : MonoBehaviour {

    public PreviewItem originalPreviewItem;
    public LevelInfo[] levelInfos;

	// Use this for initialization
	void Start () {
        AutoCreatePreviewItems();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AutoCreatePreviewItems()
    {
        if (levelInfos != null && originalPreviewItem != null)
        {
            for(int i =0; i < levelInfos.Length ;i++)
            {
                LevelInfo tempInfo = levelInfos[i];
                PreviewItem clonedItem = GameObject.Instantiate(originalPreviewItem) as PreviewItem;
                clonedItem.transform.parent = this.transform;
                clonedItem.info = tempInfo;
                clonedItem.transform.localScale = Vector3.one;
                clonedItem.transform.localPosition = Vector2.zero;
                clonedItem.indexForClassic = i + 1;
            }
        }
        UIGrid uigrid = this.GetComponent<UIGrid>();
        if (null != uigrid)
        {
            uigrid.repositionNow = true;
            uigrid.Reposition();
        }
    }
}