using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Chequer的编辑器面板
///
/// 2014-05-24
/// design by 顾文光
/// </summary>

[CustomEditor(typeof(Chequer))]
public class InspetorForChequer : Editor {


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        #region --------- create enemy grid datas -----------
        if (!Application.isPlaying && GUILayout.Button( "Create Emety Grids"))        
        {
            Chequer myChequer = target as Chequer;
            if (myChequer != null && null != myChequer.originalGridSample)
            {
                Transform rootTrans = myChequer.transOfDataRoot == null ? myChequer.transform : myChequer.transOfDataRoot;

                #region -------------- clear all exist datas --------------
                foreach (GridData existedData in rootTrans.GetComponentsInChildren<GridData>())
                {
                    GameObject.DestroyImmediate(existedData.gameObject);
                }
                #endregion

                for (int row = 1; row < myChequer.rowCount + 1; row++) 
                {
                    for (int col = 1; col < myChequer.columnCount + 1; col++)
                    {
                        GridData clonedObject = GameObject.Instantiate(myChequer.originalGridSample) as GridData;
                        /*
                         * (1,1) 1,2 ( 1,3)
                         */

                        clonedObject.transform.parent = rootTrans;
                        clonedObject.ID = (row - 1) * (myChequer.columnCount) + col;
                        clonedObject.transform.localPosition = new Vector2(col, -row);
                        clonedObject.name = string.Format("GridData {2} ({0},{1})", col.ToString("00"), row.ToString("00"), clonedObject.ID.ToString("000"));
                        clonedObject.GetComponent<SpriteRenderer>().color = myChequer.bgColor;
                        //clonedObject.gameObject.layer = 1 << LayerMask.NameToLayer(GameConstants.BGLayerName);
                    }
                }
            }
        }
        #endregion

        #region -------- create operatable grids -----------
        // no no ，不走这儿了
        #endregion
    }
}