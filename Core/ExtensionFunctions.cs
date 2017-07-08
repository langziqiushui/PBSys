using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Globalization;

namespace YX.Core
{
    /// <summary>
    /// 扩展方法集。
    /// </summary>
    public static class ExtensionFunctions
    {
        /// <summary>
        /// 确定两个对象是否具有相同的值。
        /// </summary>
        /// <param name="v1">要比较的对象1</param>
        /// <param name="v2">要比较的对象2</param>
        /// <returns></returns>
        private static bool _ObjectEquals(this object v1, object v2)
        {
            //字符串判断。
            if (v1 is string)
            {
                if (v2 is ValueType)
                    v2 = v2.ToString();
                else if (v2 is string) { }
                else
                    return false;
                return ((string)v1).CompareTo(v2) == 0;
            }

            //值类型对象判断。
            if (v1 is ValueType)
            {
                if (v2 is string)
                    return (v1.ToString()).CompareTo(v2) == 0;
                else if (v2 is ValueType)
                    return v1.Equals(v2);
                else
                    return false;
            }

            //以下为引用类型判断-----------//
            //判断是否都为null。
            if (null == v1)
                return null == v2;
            //如果v2是字符串或值类型返回false。
            if (v2 is string || v2 is ValueType)
                return false;
            //如果为相同引用，直接返回true。
            if (object.ReferenceEquals(v1, v2))
                return true;
            //如果实现了IComparable接口，使用IComparable接口中方法CompareTo进行判断。
            if (v1 is IComparable)
                return ((IComparable)v1).CompareTo(v2) == 0;
            //object默认判断。
            return v1.Equals(v2);
        }

        /// <summary>
        /// 确定两个对象是否具有相同的值。
        /// </summary>
        /// <param name="_this">要比较的对象1</param>
        /// <param name="v2">要比较的对象2</param>
        /// <returns></returns>
        public static bool ObjectEquals<T>(this T _this, T v2)
        {
            return _ObjectEquals(_this, v2);
        }

        /// <summary>
        /// 从当前字符串构建N个字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string New(this string _this, int c)
        {
            return (new string('a', c)).Replace("a", _this);
        }

        /// <summary>
        /// 确定两个对象是否具有相同的值。
        /// </summary>
        /// <param name="_this">要比较的对象1</param>
        /// <param name="v2">要比较的对象2</param>
        /// <returns></returns>
        public static bool ObjectEquals(this object _this, object v2)
        {
            return _ObjectEquals(_this, v2);
        }

        /// <summary>
        /// 判断字符串是否为空 或 空字符串(Trim空格，所以空格也会判断为空字符串)。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string _this)
        {
            return string.IsNullOrEmpty(_this) || _this.Trim().Length == 0;
        }

        /// <summary>
        /// 删除字符串中的所有空格。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string RemoveBlankSpace(this string _this)
        {
            if (string.IsNullOrEmpty(_this))
                return string.Empty;
            return _this.Replace(" ", string.Empty);
        }

        /// <summary>
        /// 不区分大不写比较两个字符是否相等(已自动去除空格)。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool IgnoreCaseEquals(this string _this, string v2)
        {
            if (null == _this)
            {
                return null == v2;
            }
            else
            {
                if (null == v2)
                    return false;
            }

            return _this.Trim().Equals(v2.Trim(), StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 不区分大小写替换字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="oldValue">旧文本</param>
        /// <param name="newValue">新文本</param>
        /// <returns></returns>
        public static string IgnoreCaseReplace(this string _this, string oldValue, string newValue)
        {
            return Regex.Replace(_this, oldValue, newValue, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 返回的字符串数组包含此字符串中的子字符串（已自动去除空白项）。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="separator">分隔字符串</param>
        /// <returns></returns>
        public static string[] Split(this string _this, string separator)
        {
            if (string.IsNullOrEmpty(_this))
                return new string[0];

            return _this.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 不区分大小写报告指定的字符串在当前 System.String 对象中的第一个匹配项的索引。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int IgnoreCaseIndexOf(this string _this, string v2)
        {
            if (null == _this)
                return -1;

            return _this.IndexOf(v2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 不区分大小写确定使用指定的比较选项进行比较时此字符串实例的结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool IgnoreCaseEndsWith(this string _this, string v2)
        {
            if (null == _this)
                return false;

            return _this.EndsWith(v2, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 从0索引处截取指定字符长度(区分全角半角)的字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="charLength">指定的字符半角长度</param>
        /// <returns></returns>
        public static string SubString2(this string _this, int charLength)
        {
            return CoreUtil.SubString2(_this, charLength);
        }

        /// <summary>
        /// 获取字符长度(区别全角和半角)。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static int GetCharLength(this string _this)
        {
            var len = 0;

            foreach (char _c in _this.ToCharArray())
            {
                len += _c > 255 ? 2 : 1;
            }

            return len;
        }

        /// <summary>
        /// 不区分大不写判断某个集合是否包含某个字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool IgnoreCaseContains(this IEnumerable<string> _this, string v2)
        {
            return _this.Contains(v2, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 不区分大不写判断某个集合项是否包含某个字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int IgnoreCaseIndexOf(this IEnumerable<string> _this, string v2)
        {
            foreach (string _t in _this)
            {
                int index = _t.IgnoreCaseIndexOf(v2);
                if (index >= 0)
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// 将NameValueCollection对象转换为SortedDictionary。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> ToSortedDictionary(this NameValueCollection _this)
        {
            var sd = new SortedDictionary<string, string>();
            foreach (string _key in _this.Keys)
            {
                if (!string.IsNullOrEmpty(_key) && !sd.ContainsKey(_key.Trim().ToLower()))
                    sd.Add(_key.Trim().ToLower(), _this[_key]);
            }

            return sd;
        }

        /// <summary>
        /// 将Dictionary转换为QueryString字符串形式。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<string, string> _this)
        {
            int index = 0;
            string s = string.Empty;
            foreach (string _key in _this.Keys)
            {
                s += _key + "=" + System.Web.HttpUtility.UrlEncode(_this[_key]);
                if (index != _this.Count - 1)
                    s += "&";

                index += 1;
            }

            return s;
        }

        /// <summary>
        /// 不区分大不写移除集合中的项。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int IgnoreCaseRemove(this IList<string> _this, string v2)
        {
            int removeIndex = -1;

            for (int i = _this.Count - 1; i >= 0; i--)
            {
                if (_this[i].IgnoreCaseEquals(v2))
                {
                    _this.RemoveAt(i);
                    if (removeIndex == -1)
                        removeIndex = i;
                }
            }

            return removeIndex;
        }

        /// <summary>
        /// 查找列表控件指定值获取匹配项的索引号。
        /// </summary>
        /// <param name="control">ListControl及继承对象</param>
        /// <param name="value">指定值</param>
        /// <returns></returns>
        public static int GetIndex(this System.Web.UI.WebControls.ListControl control, string value)
        {
            if (null == value)
                return -1;

            for (int i = 0; i < control.Items.Count; i++)
            {
                if (control.Items[i].Value.IgnoreCaseEquals(value))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 取消选择所有项。
        /// </summary>
        /// <param name="control">ListControl及继承对象</param>
        /// <returns></returns>
        public static void UnCheckAll(this System.Web.UI.WebControls.ListControl control)
        {
            foreach (ListItem _item in control.Items)
                _item.Selected = false;
        }

        /// <summary>
        /// 根据指定的数组值设置选择状态。
        /// </summary>
        /// <param name="control">CheckBoxList象</param>
        /// <param name="values">将要选定的值数组</param>
        /// <returns></returns>
        public static void CheckValues(this System.Web.UI.WebControls.CheckBoxList control, string[] values)
        {
            foreach (var _value in values)
            {
                foreach (ListItem _item in control.Items)
                {
                    if (_item.Value.IgnoreCaseEquals(_value))
                    {
                        _item.Selected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有选中项值(值之间以|号分隔)。
        /// </summary>
        /// <param name="control">CheckBoxList象</param>
        /// <returns></returns>
        public static string GetCheckedValues(this System.Web.UI.WebControls.CheckBoxList control)
        {
            var s = "";
            foreach (ListItem _item in control.Items)
            {
                if (_item.Selected)
                    s += _item.Value + "|";
            }
            if (s.Length > 0)
                s = s.Substring(0, s.Length - 1);

            return s;
        }

        /// <summary>
        /// 获取列表控件所有处于选择状态的项。
        /// </summary>
        /// <param name="control">ListControl及继承对象</param>
        /// <returns></returns>
        public static IList<ListItem> SelectedItems(this System.Web.UI.WebControls.ListControl control)
        {
            var items = new List<ListItem>();
            foreach (ListItem _item in control.Items)
            {
                if (_item.Selected)
                    items.Add(_item);
            }

            return items;
        }

        /// <summary>
        /// 将文本集转换为数字集合。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static IList<int> ToIntList(this IList<string> _this)
        {
            return (from g in _this select int.Parse(g)).ToList();
        }

        /// <summary>
        /// 将文本集转换为数字数组。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this IList<string> _this)
        {
            return (from g in _this select int.Parse(g)).ToArray();
        }

        /// <summary>
        /// 将文本集合转换为拼接的字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="splitChar">字符串分隔符</param>
        /// <returns></returns>
        public static string ToJoinString<T>(this IList<T> _this, string splitChar)
        {
            if (string.IsNullOrEmpty(splitChar))
                splitChar = ",";

            var str = string.Empty;
            foreach (var _str in _this)
                str += _str.ToString() + splitChar;

            if (str.EndsWith(splitChar))
                str = str.Substring(0, str.Length - splitChar.Length);

            return str;
        }

        /// <summary>
        /// 将指定集合的元素添加到集合的末尾。
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="_this"></param>
        /// <param name="source"></param>
        public static void AddRange<T1, T2>(this IDictionary<T1, T2> _this, IDictionary<T1, T2> source)
        {
            foreach (var _key in source.Keys)
            {
                if (!_this.ContainsKey(_key))
                    _this.Add(_key, source[_key]);
            }
        }

        /// <summary>
        /// 将NameValueCollection对象转换为Dictionary。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this NameValueCollection _this)
        {
            var dic = new Dictionary<string, string>();
            foreach (string _key in _this.Keys)
            {
                if (!string.IsNullOrEmpty(_key) && !dic.ContainsKey(_key.Trim().ToLower()))
                    dic.Add(_key.Trim().ToLower(), _this[_key]);
            }

            return dic;
        }

        /// <summary>
        /// 客房端显示或隐藏控件。
        /// </summary>
        /// <param name="_this">WebControl</param>
        /// <param name="visible">是否显示</param>
        public static void ClientVisiblue(this System.Web.UI.HtmlControls.HtmlControl _this, bool visible)
        {
            _this.Style.Add("display", visible ? "" : "none");
        }

        /// <summary>
        /// 客房端显示或隐藏控件。
        /// </summary>
        /// <param name="_this">WebControl</param>
        /// <param name="visible">是否显示</param>
        public static void ClientVisiblue2(this WebControl _this, bool visible)
        {
            _this.Style.Add("display", visible ? "" : "none");
        }

        /// <summary>
        /// 显示当前对象所有信息。
        /// </summary>
        /// <returns></returns>
        public static string ToFullString(this object _self)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var _piInfo in _self.GetType().GetProperties())
                {
                    object value = _piInfo.GetValue(_self, null);
                    string valueText = null == value ? "null" : value.ToString();
                    sb.Append(_piInfo.Name + "：" + valueText + "\n");
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region 将集合转换为json字符串

        /// <summary>
        /// 将实体转换为Json(格式："AAA":"11111", "BBB":"2222")。
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T _this)
        {
            var sb = new StringBuilder();
            foreach (System.Reflection.PropertyInfo _pi in _this.GetType().GetProperties())
            {
                object o = _pi.GetValue(_this, null);

                sb.Append("\"");
                sb.Append(_pi.Name);
                sb.Append("\":\"");
                sb.Append(null == o ? "" : CoreUtil.ReplaceQUOT(o.ToString()));
                sb.Append("\",");
            }

            //去除最后一个,号。
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// 将集合转换为json字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ListToJson<T>(this IList<T> _this)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var _entity in _this)
            {
                sb.Append("{");
                sb.Append(_entity.ToJson());
                sb.Append("},");
            }

            if (_this.Count > 0)
                sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }

        #endregion

        #region 以安全方式转换为字符串

        /// <summary>
        /// 以安全方式转换为字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ToSafeString(this object _this)
        {
            if (null == _this)
                return string.Empty;

            return _this.ToString();
        }

        #endregion

        #region 将某个可空的枚举值转换为byte或null

        /// <summary>
        /// 将某个可空的枚举值转换为byte或null。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static byte? EnumValueToByte<T>(this T _this)
        {
            if (null == _this)
                return null;

            return Convert.ToByte(_this);
        }

        #endregion

        #region 以安全的方式获取键值集合对象的值

        /// <summary>
        /// 以安全的方式获取键值集合对象的值(如果键是字符串忽略字符串大小写)。
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="_this">Dictionary字典集合</param>
        /// <param name="key">需要获取数据的键</param>
        /// <returns></returns>
        public static TValue SafeGetDictionaryValue<TKey, TValue>(this Dictionary<TKey, TValue> _this, TKey key)
        {
            //如果键是字符串类型，则以忽略大小写的方式获取值。
            if (typeof(TKey) == typeof(System.String))
            {
                foreach (var _key in _this.Keys)
                {
                    if (_key.ToString().IgnoreCaseEquals(key.ToString()))
                        return _this[_key];
                }
            }

            //普通方式获取值。
            if (_this.ContainsKey(key))
                return _this[key];

            //未找到返回默认值。
            return default(TValue);
        }

        #endregion
    }
}
