using UnityEngine;
using System.Collections;

namespace TeaSoft
{
    /// <summary>
    /// 快速操作数组的一个常用方法组 
    /// designed by G.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ArrayQuickOperator<T>
    {
        /// <summary>
        /// insert a new element into array
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="myArray"></param>
        /// <returns></returns>
        internal static T[] InsertElementToCollection(int index, T value, T[] myArray)
        {
            T[] result = new T[myArray.Length + 1];
            for (int i = 0; i < myArray.Length + 1; i++)
            {
                if (i < index)
                {
                    result[i] = myArray[i];
                }
                else if (i == index)
                {
                    result[i] = value;
                }
                else
                {
                    result[i] = myArray[i - 1];
                }
            }
            return result;
        }

        /// <summary>
        /// delete an element from array
        /// </summary>
        /// <param name="index"></param>
        /// <param name="myArray"></param>
        /// <returns></returns>
        internal static T[] DeleteElementFromCollection(int index, T[] myArray)
        {
            T[] result = new T[myArray.Length - 1];
            for (int i = 0; i < myArray.Length; i++)
            {
                if (i < index)
                {
                    result[i] = myArray[i];
                }
                else if (i == index)
                {
                    continue;
                }
                else
                {
                    result[i - 1] = myArray[i];
                }
            }
            return result;
        }

        /// <summary>
        /// swap two elements in specific array;
        /// </summary>
        /// <param name="indexSwap1"></param>
        /// <param name="indexSwap2"></param>
        /// <param name="myArray"></param>
        /// <returns></returns>
        internal static T[] SwapElementFromCollection(int indexSwap1, int indexSwap2, T[] myArray)
        {
            T[] result = myArray;
            T tempData = result[indexSwap1];
            result[indexSwap1] = result[indexSwap2];
            result[indexSwap2] = tempData;
            return result;
        }
    }


    #region -------------------- operations --------------------


    public abstract class ArrayEditOperation<T>
    {
        protected T[] myArray;
        public abstract T[] GetResult();
    }

    public class ArrayMoveUpOperation<T> : ArrayEditOperation<T>
    {
        int index = -1;
        public ArrayMoveUpOperation(int selectedIndex, T[] array)
        {
            this.index = selectedIndex;
            this.myArray = array;
        }

        public override T[] GetResult()
        {
            //throw new System.NotImplementedException();
            if (index >= 1 && myArray != null && myArray.Length > 0)
            {
                myArray = TeaSoft.ArrayQuickOperator<T>.SwapElementFromCollection(index, index - 1, myArray);
            }
            return myArray;
        }
    }
    public class ArrayMoveDownOperation<T> : ArrayEditOperation<T>
    {
        int index = -1;
        public ArrayMoveDownOperation(int selectedIndex, T[] array)
        {
            this.index = selectedIndex;
            this.myArray = array;
        }


        public override T[] GetResult()
        {
            //throw new System.NotImplementedException();
            if (index < myArray.Length - 1 && myArray != null && myArray.Length > 0)
            {
                myArray = TeaSoft.ArrayQuickOperator<T>.SwapElementFromCollection(index, index + 1, myArray);
            }
            return myArray;
        }
    }
    public class ArrayInsertOperation<T> : ArrayEditOperation<T>
    {
        int index;
        T newElement;
        public ArrayInsertOperation(int insertIndex, T element, T[] array)
        {
            this.index = insertIndex;
            this.newElement = element;
            this.myArray = array;
        }

        public override T[] GetResult()
        {
            //throw new System.NotImplementedException();
            if (myArray != null)
            {
                if (myArray.Length == 0 && index == 0)
                {
                    myArray = new T[] { newElement };
                }
                else if (index <= myArray.Length - 1)
                {
                    myArray = TeaSoft.ArrayQuickOperator<T>.InsertElementToCollection(index, newElement, myArray);
                }
            }
            return myArray;
        }
    }
    public class ArrayDeleteOperation<T> : ArrayEditOperation<T>
    {
        int index;
        public ArrayDeleteOperation(int selectedIndex, T[] array)
        {
            this.index = selectedIndex;
            this.myArray = array;
        }

        public override T[] GetResult()
        {
            //throw new System.NotImplementedException();
            if (myArray != null && myArray.Length > 0 && index < myArray.Length)
            {
                myArray = TeaSoft.ArrayQuickOperator<T>.DeleteElementFromCollection(index, myArray);
            }
            return myArray;
        }
    }

    public class ArraySortOperation<T> : ArrayEditOperation<T>
    {
        System.Comparison<T> myComparer = null;
        //System.Collections.Generic.IComparer<T> myComparer = null;
        public ArraySortOperation(T[] array, System.Comparison<T> comparer)
        {
            this.myArray = array;
            this.myComparer = comparer;
        }
        public override T[] GetResult()
        {
            //throw new System.NotImplementedException();
            //System.Array.Sort<T>(myArray,
            if (null != myComparer && null != myArray && myArray.Length > 0)
            {
                System.Array.Sort<T>(myArray, myComparer);
            }
            return myArray;
        }
    }

    #endregion

}