using UnityEngine;
using System.Collections;

namespace TeaSoft
{
    /// <summary>
    /// 计时触发器
    /// designed by G.
    /// </summary>
    public class Timer
    {
        bool isOff = false;
        internal bool IsLoop { get; set; }

        public Timer()
        {
            Ticked += new NonReturnNonParamEventHandle(TurnOffTimer);
        }

        void TurnOffTimer()
        {
            if (!IsLoop)
            {
                isOff = true;
            }
        }

        internal event NonReturnNonParamEventHandle Ticked;

        internal float Interval { get; set; }

        float spanedTime = 0f;

        internal void Update()
        {
            if (isOff)
            {
                return;
            }
            spanedTime += Time.deltaTime;
            if (spanedTime >= Interval)
            {
                spanedTime = 0f;
                if (null != Ticked)
                {
                    Ticked();
                }
            }
        }

        internal void Reset()
        {
            this.spanedTime = 0f;
            isOff = false;
        }
    }

    public delegate void NonReturnNonParamEventHandle();

}