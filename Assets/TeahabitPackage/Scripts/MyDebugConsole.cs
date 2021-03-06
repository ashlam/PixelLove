using UnityEngine;
using System.Collections;
namespace TeaSoft
{
    /// <summary>
    /// 我自己用的一个输出控制台... 
    /// designed by G.
    /// </summary>
    class MyDebugConsole
    {
        /// <summary>
        /// 输出一行字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void OutputText(object format, params object[] args)
        {
            string text = string.Format(format.ToString(), args);
            Debug.Log(text);
        }

        /// <summary>
        /// 输出一个带时间戳的字符串
        /// </summary>
        /// <param name="flag"></param>
        internal static void OutputCurrentTime(string flag)
        {
            OutputText("[{1} {2}ms]--> {0}", flag, System.DateTime.Now.ToLongTimeString(), System.DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 输出一个带时间戳的字符串（重载）
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void OutputCurrentTime(object format,params object[] args)
        {
            string text = (null == format)? "NullString " : string.Format(format.ToString(), args);
            OutputCurrentTime(text);
        }

        /// <summary>
        /// 输出某物体的名字
        /// </summary>
        /// <param name="obj"></param>
        internal static void OutputObjectName(Object obj)
        {
            System.DateTime currentDateTime = System.DateTime.Now;
            Debug.Log(string.Format("Run {0} at {1}:{2}", obj.ToString(), currentDateTime.ToShortTimeString(), currentDateTime.Millisecond));

        }

        internal static void Assert(bool result, string message)
        {
            if (!result)
                MyDebugConsole.OutputText("{0}. {1}", result.ToString(), message);
        }

        internal static void Assert(bool result)
        {
            Assert(result, "");
        }

        /// <summary>
        /// 如果满足条件，则输出指定的文字信息
        /// 可用于监视一些简单的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="text"></param>
        internal static void OutputConditionText(bool condition, string text)
        {
            if (condition)
            {
                OutputText(text);
            }
        }

        /// <summary>
        /// 如果满足条件，则输出指定的文字信息（重载）
        /// 可用于监视一些简单的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void OutputConditionText(bool condition, object format, params object[] args)
        {
            OutputConditionText(condition, string.Format(format.ToString(), args));
        }
    }
}
