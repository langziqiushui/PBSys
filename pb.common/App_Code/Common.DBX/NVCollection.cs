using System.Collections.Generic;

namespace Common.DBX
{
    /// <summary>
    /// KeyValueCollection 的摘要说明
    /// </summary>
    public class NVCollection : Dictionary<string, object>
    {
        public NVCollection()
            : base()
        {
        }

        public NVCollection(int capacity)
            : base(capacity)
        {
        }

        public new NVCollection Add(string key, object value)
        {
            base.Add(key, value);
            return this;
        }

        public object this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }

                return null;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}