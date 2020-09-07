using UnityEngine;
using System.Collections;

/// <summary>
/// 脚本实现功能
/// 
/// 2014-09-16
/// design by 顾文光
/// </summary>
public class NTopCameraController : MonoBehaviour {

    public Transform root;
	// Use this for initialization
	void Start () {
        MainGame.topLayerCamera = this;
        if (root == null)
        {
            UIAnchor myAnchor = GetComponentInChildren<UIAnchor>();
            if (myAnchor != null)
            {
                root = myAnchor.transform;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void HideTopWindow()
    {
        if (root != null)
        {
            root.gameObject.SetActive(false);
        }
    }

    internal void ShowTopWindow()
    {
        //throw new System.NotImplementedException();
        if (root != null)
        {
            root.gameObject.SetActive(true);
        }
    }
}