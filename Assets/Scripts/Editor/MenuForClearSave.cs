using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// 增加一个菜单项：清除存档
/// 
/// 2014-09-17
/// design by 顾文光
/// </summary>
public class MenuForClearSave : Editor {

    [MenuItem("DEBUG/Clear PlayerPrefs")]
    static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("**** All save date are cleared ****");
    }
	
}