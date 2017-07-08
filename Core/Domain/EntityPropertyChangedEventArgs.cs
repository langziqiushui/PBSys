using System;
using System.ComponentModel;

namespace YX.Core
{
    /// <summary>
    /// 当属性值发生改变或将要发生改变的事件委托。
    /// </summary>
    /// <param name="sender">事件主体对象</param>
    /// <param name="e">EntityPropertyChangedEventArgs事件</param>
    public delegate void EntityPropertyChangedEventHandler(object sender,EntityPropertyChangedEventArgs e);

    /// <summary>
    /// 当属性值发生改变或将要发生改变的自定义事件。
    /// </summary>
    public class EntityPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="_propertyName">已经更改或将要更改的属性名称</param>
        public EntityPropertyChangedEventArgs(string _propertyName)
            : base(_propertyName)
        {
            
        }

        /// <summary>
        /// 构造器(重载二)。
        /// </summary>
        /// <param name="_propertyName">已经更改或将要更改的属性名称</param>
        /// <param name="_newValue">新值</param>
        public EntityPropertyChangedEventArgs(string _propertyName, object _newValue)
            : this(_propertyName)
        {
            this.NewValue = _newValue;
        }

        /// <summary>
        /// 获取或设置 将要设置的新值。
        /// </summary>
        public object NewValue { get; set; }      

        /// <summary>
        /// 获取或设置 是否取消修改。
        /// </summary>
        public bool Cancel { get; set; }
    }
}
