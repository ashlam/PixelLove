using UnityEngine;
using System.Collections;

/// <summary>
/// 格数据，用于编辑图形
/// 每个格有自己的编号，从0开始
/// 编辑前由chequer自动创建并指派一个id，并根据这个id来分配guid的（相对）位置
/// 每个GridData挂在一个包含Sprite的对象上，利用Sprite的Color描绘图形
/// 
/// 2014-05-24
/// design by 顾文光
/// </summary>
public class GridData : MonoBehaviour {
    public int ID;
    public bool IsColored = true;
    //// Use this for initialization
    //void Start()
    //{
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}
}