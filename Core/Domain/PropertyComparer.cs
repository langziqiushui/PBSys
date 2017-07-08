using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace YX.Core
{
    /// <summary>
    /// 自定义对象比较对象。
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    internal class PropertyComparer<T> : IComparer<T>
    {
        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="SortPropertyCore">用于对列表排序的属性说明符</param>
        /// <param name="SortDirectionCore">排序操作的方向</param>
        public PropertyComparer(PropertyDescriptor SortPropertyCore, ListSortDirection SortDirectionCore)
        {
            this._sortPropertyCore = SortPropertyCore;
            this._sortDirectionCore = SortDirectionCore;
        }

        /// <summary>
        /// 排序操作的方向 。
        /// </summary>
        private ListSortDirection _sortDirectionCore;
        /// <summary>
        /// 用于对列表排序的属性说明符。
        /// </summary>
        private PropertyDescriptor _sortPropertyCore;

        /// <summary>
        /// 比较两个对象。
        /// </summary>
        /// <param name="x">泛型对象一</param>
        /// <param name="y">泛型对象二</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            int num2;
            object xValue = x.GetType().GetProperty(this._sortPropertyCore.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(this._sortPropertyCore.Name).GetValue(y, null);
            if (xValue is IComparable)
            {
                num2 = ((IComparable)xValue).CompareTo(yValue);
            }
            else
            {
                num2 = xValue.ToString().CompareTo(yValue.ToString());
            }
            if (this._sortDirectionCore == ListSortDirection.Ascending)
            {
                return num2;
            }
            return -num2;
        }
    }

}
