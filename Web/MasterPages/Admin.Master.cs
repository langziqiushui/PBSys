using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;

using YX.Core;

namespace YX.Web.MasterPages.Normal
{
    /// <summary>
    /// 后台管理模板。
    /// </summary>
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //载入数据。
            this.LoadData();
        }

        /// <summary>
        /// 载入数据。
        /// </summary>
        private void LoadData()
        {
            //获取用户类别。
            var userType = ((int)YX.Factory.AdminUserManager.UserType).ToString();
            //载入菜单。
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("/App_Data/Admin.xml"));

            //是否为系统管理员主账号。
            var isHostAdmin = true;

            //json格式
            var sb = new StringBuilder();
            sb.Append("[");
            var nodeModules = xmlDoc.DocumentElement.SelectNodes("Module");
            foreach (XmlNode _nodeModule in nodeModules)
            {

                //判断用户类别。
                var arrUserType = _nodeModule.Attributes["UserType"].Value.Split(',');
                if (!arrUserType.Contains(userType))
                    continue;


                sb.Append("{");
                sb.AppendFormat("\"Caption\":\"{0}\",\"List\":[", _nodeModule.Attributes["Caption"].Value);
                var nodeList = _nodeModule.SelectNodes("List");
                foreach (XmlNode _nodeList in nodeList)
                {
                    if (null != _nodeList.Attributes["IsActived"] && _nodeList.Attributes["IsActived"].Value == "0")
                        continue;

                    if (null != _nodeList.Attributes["UserType"])
                    {
                        arrUserType = _nodeList.Attributes["UserType"].Value.Split(',');
                        if (!arrUserType.Contains(userType))
                            continue;
                    }

                    //获取所有有效的子级节点集合。
                    var nodeItems = this.GetEffectivedNodes(_nodeList.SelectNodes("Item"));
                    //如果没有子级。
                    if (nodeItems.Count == 0)
                        continue;

                    

                    sb.Append("{");
                    sb.AppendFormat("\"TarID\":\"{0}\", \"Caption\":\"{1}\", \"Class\":\"{2}\", \"Items\":[", null != _nodeList.Attributes["TarID"] ? _nodeList.Attributes["TarID"].Value : "", _nodeList.Attributes["Caption"].Value, null != _nodeList.Attributes["Class"] ? _nodeList.Attributes["Class"].Value : "");

                    foreach (XmlNode _nodeItem in nodeItems)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"TarID\":\"{0}\", \"Caption\":\"{1}\",\"Url\":\"{2}\"", null != _nodeItem.Attributes["TarID"] ? _nodeItem.Attributes["TarID"].Value : "", _nodeItem.Attributes["Caption"].Value, _nodeItem.Attributes["Url"].Value);
                        sb.Append("},");
                    }

                    //去除最后一个,
                    if (nodeItems.Count > 0)
                        sb.Remove(sb.Length - 1, 1);

                    sb.Append("]},");
                }
                if (nodeList.Count > 0)
                    sb.Remove(sb.Length - 1, 1);

                sb.Append("]},");
            }

            if (nodeModules.Count > 0)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("]");



            Web.Framework.WFUtil.RegisterStartupScript("var jsonNav = " + sb.ToString() + ";");
        }

        /// <summary>
        /// 从传入的节点集合中返回有效的节点集合。
        /// </summary>
        /// <param name="nodeItems">节点集集合</param>
        /// <returns></returns>
        private List<XmlNode> GetEffectivedNodes(XmlNodeList nodeItems)
        {
            var nodeList = new List<XmlNode>();
            foreach (XmlNode _node in nodeItems)
            {
                //如果没有设置权限或有操作权限。
                if (null == _node.Attributes["IsSupperAdmin"])
                {
                    nodeList.Add(_node);
                }
                else
                {
                    if (_node.Attributes["IsSupperAdmin"].Value == this.Page.User.Identity.Name)
                        nodeList.Add(_node);
                }
            }

            return nodeList;
        }
    }
}