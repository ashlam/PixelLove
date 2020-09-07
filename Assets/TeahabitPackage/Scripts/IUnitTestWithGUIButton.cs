using UnityEngine;
using System.Collections;
namespace TeaSoft.UnitTest
{

    /// <summary>
    /// 一个通过按钮来触发某行为的类
    /// 用于单元测试
    ///
    /// 2014-03-24
    /// design by 顾文光
    /// </summary>
    public abstract class IUnitTestWithGUIButton : MonoBehaviour
    {

        public Rect buttonRect;
        public string buttonName;
        //// Use this for initialization
        //void Start () {

        //}

        //// Update is called once per frame
        //void Update () {

        //}

        protected virtual new void OnGUI()
        {
            this.DrawButton();
        }

        protected void DrawButton()
        {
            if (GUI.Button(buttonRect, buttonName))
            {
                DoSomething();
            }
        }

        protected abstract void DoSomething();
    }
}