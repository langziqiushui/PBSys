using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace YX.Core
{
    /// <summary>
    /// 枚举对象说明。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        #region 构造器

        /// <summary> 
        /// 描述枚举值，默认排序为5 
        /// </summary>  
        /// <param name="enumDisplayText">描述内容</param> 
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, "")
        {
        }

        /// <summary> 
        /// 描述枚举值，默认排序为5 
        /// </summary>  
        /// <param name="enumDisplayText">枚举显示文本</param> 
        /// <param name="colorOrDescription">枚举颜色值或详细说明</param>
        public EnumDescription(string enumDisplayText, string colorOrDescription)
        {
            this.displayText = enumDisplayText;
            this.colorOrDescription = colorOrDescription;
            this.enumRank = 5;
        }

        #endregion

        #region 变量

        /// <summary>
        /// 枚举显示文本。
        /// </summary>
        private string displayText;
        /// <summary>
        /// 枚举颜色值或详细描述。
        /// </summary>
        private string colorOrDescription;
        /// <summary>
        /// 排序值。
        /// </summary>
        private int enumRank;
        /// <summary>
        /// 发现字段特性并提供对字段元数据的访问权。
        /// </summary>
        private FieldInfo fieldIno;
        /// <summary>
        /// 缓存枚举集合对象。
        /// </summary>
        private static System.Collections.Hashtable cachedEnum = new Hashtable();
        /// <summary>
        /// 并发控制锁。
        /// </summary>
        private static readonly object __LOCK__ = new object();

        #endregion

        #region 枚举

        /// <summary>  
        /// 排序类型  
        /// </summary> 
        public enum SortType
        {
            /// <summary>  
            ///按枚举顺序默认排序  
            /// </summary>  
            Default,
            /// <summary>  
            /// 按描述值排序 
            /// </summary>
            DisplayText,
            /// <summary>  
            /// 按排序 值 
            /// </summary> 
            Rank
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取 枚举显示文本。
        /// </summary>
        private string DisplayText
        {
            get { return this.displayText; }
        }

        /// <summary>
        /// 获取 枚举颜色值或详细描述。
        /// </summary>
        private string ColorOrDescription
        {
            get { return this.colorOrDescription; }
        }

        /// <summary>
        /// 获取 排序值。
        /// </summary>
        private int EnumRank
        {
            get { return enumRank; }
        }

        /// <summary>
        /// 获取 枚举值。
        /// </summary>
        private int EnumValue
        {
            get { return (int)fieldIno.GetValue(null); }
        }

        /// <summary>
        /// 获取 成员名。
        /// </summary>
        private string FieldName
        {
            get { return fieldIno.Name; }
        }

        #endregion

        #region 公用方法

        /// <summary> 
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>  
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>  
        public static string GetFieldText(object enumValue)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString())
                    return ed.DisplayText;
            }
            return string.Empty;
        }

        /// <summary> 
        /// 获得指定枚举项的颜色或详细描述。
        /// </summary>  
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        public static string GetFieldColorOrDescription(object enumValue)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString())
                    return ed.ColorOrDescription;
            }

            return string.Empty;
        }

        /// <summary>
        /// 输出带颜色显示的html的文本。
        /// </summary>
        /// <param name="enumValue">指定枚举值</param>
        /// <returns></returns>
        public static string PrintText(object enumValue)
        {
            var t = GetFieldText(enumValue);
            if (string.IsNullOrEmpty(t))
                return string.Empty;

            var c = GetFieldColorOrDescription(enumValue);
            if (string.IsNullOrEmpty(c))
                return t;

            return "<FONT style='color:" + c + "'>" + t + "</FONT>";
        }

        /// <summary>  
        /// 得到枚举类型定义的所有文本  
        /// </summary>  
        /// <exception cref="NotSupportedException"></exception>  
        /// <param name="enumType">枚举类型</param>  
        /// <param name="sortType">指定排序类型</param> 
        /// <returns>所有定义的文本</returns>  

        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            EnumDescription[] descriptions = null;

            //缓存中没有找到，通过反射获得字段的描述信息  
            if (!cachedEnum.Contains(enumType.FullName))
            {
                lock (__LOCK__)
                {
                    if (!cachedEnum.Contains(enumType.FullName))
                    {
                        FieldInfo[] fields = enumType.GetFields();
                        ArrayList edAL = new ArrayList();
                        foreach (FieldInfo fi in fields)
                        {
                            object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                            if (eds.Length != 1)
                                continue;
                            ((EnumDescription)eds[0]).fieldIno = fi;
                            edAL.Add(eds[0]);
                        }

                        cachedEnum.Add(enumType.FullName, (EnumDescription[])edAL.ToArray(typeof(EnumDescription)));
                    }
                }
            }

            descriptions = (EnumDescription[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0)
                throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");
            //按指定的属性冒泡排序  
            for (int m = 0; m < descriptions.Length; m++)
            {
                //默认就不排序了  
                if (sortType == SortType.Default)
                    break;
                for (int n = m; n < descriptions.Length; n++)
                {
                    EnumDescription temp;
                    bool swap = false;
                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (string.Compare(descriptions[m].DisplayText, descriptions[n].DisplayText) > 0)
                                swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank)
                                swap = true;
                            break;
                    }

                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }
            return descriptions;
        }

        #endregion
    }
}
