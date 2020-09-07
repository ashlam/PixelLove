using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// PreviewCreator的编辑器面板
/// 
/// 2014-09-15
/// design by 顾文光
/// </summary>
/// 
[CustomEditor(typeof(PreviewCreator))]
public class InspetorForPreviewCreator : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Save to preview"))
            {
                PreviewCreator pc = target as PreviewCreator;
                if (target != null)
                {
                    pc.SaveToPriview();
                }
            }
        }
    }
    
}