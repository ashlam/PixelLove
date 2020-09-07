using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeaSoft
{
    /// <summary>
    /// GUID辅助类，里面包含一些随机GUID的相关函数
    /// 2014-03-27
    /// design by 顾文光
    /// </summary>
    public class GUIDHelper
    {
        List<int> existGUIDRecords = null;

        #region ------ Singleton ------
        static GUIDHelper instance = null;
        internal static GUIDHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new GUIDHelper();
            }
            return instance;
        }
        #endregion

        private GUIDHelper()
        {
            existGUIDRecords = new List<int>();
        }

        /// <summary>
        /// 生成一个完全随机的GUID
        /// </summary>
        /// <returns>返回一个1～max(int)的值，若返回-1则表示沒能随出來</returns>
        internal int CreateNewRandomGUID()
        {
            int result = -1;
            result = TryToCreateRandomGUID(0, new FullyRandomValueCreator(1, int.MaxValue));
            return result;
        }

        /// <summary>
        /// 尝试得到一个guid记录中不存在的数（递归）
        /// 为了避免无限循环，限定了递归次数，最多尝试1000次，超过之后返回-1
        /// 每尝试一定次数后会改变随机种子（seed）
        /// </summary>
        /// <param name="triedTimes"></param>
        /// <returns></returns>
        int TryToCreateRandomGUID(int triedTimes,IRandomValueCreator creator)
        {
            int result = -1;
            if (creator == null)
            {
                return result;
            }
            result = creator.GetRandomValue();
            if (existGUIDRecords.Contains(result))
            {
                triedTimes++;
                if (triedTimes % 60 == 1)
                {
                    Random.seed = System.DateTime.Now.Millisecond;
                }
                if (triedTimes > 1000)
                    return -1;
                result = TryToCreateRandomGUID(triedTimes, creator);
            }
            else
            {
                existGUIDRecords.Add(result);
            }
            return result;
        }

        /// <summary>
        /// 从记录中移除某个GUID
        /// </summary>
        /// <param name="guid"></param>
        internal void RemoveExistGUID(int guid)
        {
            if (existGUIDRecords.Contains(guid))
            {
                existGUIDRecords.Remove(guid);
            }
        }

        /// <summary>
        /// 清除所有GUID记录
        /// </summary>
        internal void ClearAllGUID()
        {
            existGUIDRecords.Clear();
        }

        /// <summary>
        /// 生成一个GUID（重载）
        /// 指定位数
        /// </summary>
        /// <param name="maxDigit"></param>
        /// <returns></returns>
        internal int CreateNewRandomGUID(byte maxDigit)
        {
            int result = -1;
            result = TryToCreateRandomGUID(0, new DigitRandomValueCreator(maxDigit));
            return result;
        }


        #region ---------- inter class for create a random number -----------
        interface IRandomValueCreator
        {
            int GetRandomValue();
        }

        class DigitRandomValueCreator : IRandomValueCreator
        {
            byte maxDigit = 0;
            internal DigitRandomValueCreator(byte maxDigit) { this.maxDigit = maxDigit; }
            public int GetRandomValue()
            {
                //throw new System.NotImplementedException();
                int result = -1;
                if (maxDigit < 1)
                {
                    return -3;
                }
                bool isFirstDigit = true;

                for (int i = 0; i < maxDigit; i++)
                {
                    if (isFirstDigit)
                    {
                        result = Random.Range(1, 10);
                        isFirstDigit = false;
                    }
                    else
                    {
                        result = result * 10 + Random.Range(0, 10);
                    }
                }
                return result;
            }
        }

        class FullyRandomValueCreator : IRandomValueCreator
        {
            int min, max;
            internal FullyRandomValueCreator(int min, int max) { this.min = min; this.max = max; }
            public int GetRandomValue()
            {
                //throw new System.NotImplementedException();
                return Random.Range(min, max);
            }
        }
        #endregion
    }
}