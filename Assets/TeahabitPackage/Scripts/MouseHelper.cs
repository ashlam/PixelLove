using UnityEngine;
using System.Collections;

/// <summary>
/// 关于鼠标的辅助类
/// design by 顾文光
/// </summary>

namespace TeaSoft
{
    public class MouseHelper
    {
        /// <summary>
        /// 把鼠标在3d世界中的坐标，转换成某个摄像机的映射平面坐标
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        internal static Vector3 GetMousePositionInViewport(Camera cam)
        {
            Vector3 result = -Vector3.one;
            if (null != cam)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.x = Mathf.Clamp01(mousePos.x / Screen.width);
                mousePos.y = Mathf.Clamp01(mousePos.y / Screen.height);
                mousePos.z = 0;
                result = cam.ViewportToWorldPoint(mousePos);
            }
            return result;
        }

        /// <summary>
        /// 把鼠标在3d世界中的坐标，转换成某个摄像机的映射平面坐标，并将结果转成二维向量
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        internal static Vector2 GetMousePositionInViewport2D(Camera cam)
        {
            Vector2 result = -Vector2.one;
            result = (Vector2)GetMousePositionInViewport(cam);
            return result;
        }
    }
}