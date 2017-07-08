// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/9 12:35:57 
// Description：实现数据查询相关功能(适用于Access数据库)。 
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using YX.Domain.Glo;
using YX.Core;

namespace YX.SqlserverDAL.Glo
{
    /// <summary> 
    /// 提供数据查询查关功能(对应表 Glo.PBConfig(屏蔽表) )。
    /// </summary> 
    public partial class PbconfigDAL : AbstractDAL
    {
        public PbconfigDAL() : base() { }

        #region 获取数据

        /// <summary> 
        /// 获取所有屏蔽。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        public IList<PbconfigInfo> GetData_All()
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<PbconfigInfo> entities = null;

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_PBConfig_All]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<PbconfigInfo>(reader);
            }
            return entities;
        }

        /// <summary> 
        /// 根据条件获取是否有屏蔽数据。 
        /// </summary> 
        /// <param name="applicationtype">应用程序类别[ApplicationTypes] 1= yx 2=anxiu 3=pp8 4=...</param> 
        /// <param name="pbtype">屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2</param> 
        /// <param name="keydata">主键</param> 
        /// <returns>查询获取的数据</returns> 
        public object GetData_Bywhere(byte? applicationtype, byte? pbtype, int? keydata)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, applicationtype));
            parms.Add(SqlUtil.CreateParameter("@PbType", DbType.Byte, pbtype));
            parms.Add(SqlUtil.CreateParameter("@KeyData", DbType.Int32, keydata));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Sel_PBConfig_bywhere]", parms.ToArray());
            return r;
        }

        /// <summary> 
        /// 获取某一屏蔽。 
        /// </summary> 
        /// <param name="id">屏蔽ID主键</param> 
        /// <returns>查询获取的数据</returns> 
        public PbconfigInfo GetData(int? id)
        {
            IDataReader reader = null;
            PbconfigInfo entity = null;
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ID", DbType.Int32, id));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_PBConfig_PrimaryKey]", parms.ToArray()))
            {
                entity = DomainUtil.PopulateData<PbconfigInfo>(reader);
            }
            return entity;
        }

        /// <summary> 
        /// 分页获取屏蔽。 
        /// </summary> 
        /// <param name="pagesize">单页显示的记录数</param> 
        /// <param name="pageindex">当前页码</param> 
        /// <param name="numrecord">所有符合查询条件的总记录数</param> 
        /// <param name="applicationtype">应用程序类别[ApplicationTypes] 1= yx 2=anxiu 3=pp8 4=...</param> 
        /// <param name="pbtype">屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2</param> 
        /// <param name="keydata">主键</param> 
        /// <returns>查询获取的数据集</returns> 
        public IList<PbconfigInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, byte? applicationtype, byte? pbtype, int? keydata)
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<PbconfigInfo> entities = null;

            parms.Add(SqlUtil.CreateParameter("@pageSize", DbType.Int32, pagesize));
            parms.Add(SqlUtil.CreateParameter("@pageIndex", DbType.Int32, pageindex));
            parms.Add(SqlUtil.CreateOutputParameter("@numRecord", DbType.Int32, 4));
            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, applicationtype));
            parms.Add(SqlUtil.CreateParameter("@PbType", DbType.Byte, pbtype));
            parms.Add(SqlUtil.CreateParameter("@KeyData", DbType.Int32, keydata));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_PBConfig_Paging]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<PbconfigInfo>(reader);
            }

            #region 存储过程输出参数 

            if (DBNull.Value != parms["@numRecord"].Value)
                numrecord = Convert.ToInt32(parms["@numRecord"].Value);

            #endregion 存储过程输出参数 

            return entities;
        }

        #endregion

        #region 添加数据

        /// <summary> 
        /// 创建屏蔽。 
        /// </summary> 
        /// <param name="entity">PbconfigInfo业务实体对象</param> 
        /// <param name="id">插入对象的自动递增编号</param> 
        /// <returns>数据添加是否成功</returns> 
        public object Add(PbconfigInfo entity, ref int id)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, entity.ApplicationType));
            parms.Add(SqlUtil.CreateParameter("@PbType", DbType.Byte, entity.PbType));
            parms.Add(SqlUtil.CreateParameter("@KeyData", DbType.Int32, entity.KeyData));
            parms.Add(SqlUtil.CreateParameter("@Title", DbType.String, entity.Title));
            parms.Add(SqlUtil.CreateOutputParameter("@ID", DbType.Int32, 4));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Ins_PBConfig]", parms.ToArray());

            #region 存储过程输出参数 

            if (DBNull.Value != parms["@ID"].Value)
                id = Convert.ToInt32(parms["@ID"].Value);

            #endregion 存储过程输出参数 

            return r;
        }

        #endregion

        #region 修改数据

        /// <summary> 
        /// 修改某一屏蔽。 
        /// </summary> 
        /// <param name="entity">PbconfigInfo业务实体对象</param> 
        /// <returns>数据修改是否成功</returns> 
        public object Modify(PbconfigInfo entity)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ID", DbType.Int32, entity.ID));
            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, entity.ApplicationType));
            parms.Add(SqlUtil.CreateParameter("@PbType", DbType.Byte, entity.PbType));
            parms.Add(SqlUtil.CreateParameter("@KeyData", DbType.Int32, entity.KeyData));
            parms.Add(SqlUtil.CreateParameter("@Title", DbType.String, entity.Title));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Upd_PBConfig_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

        #region 删除数据

        /// <summary> 
        /// 删除某一屏蔽。 
        /// </summary> 
        /// <param name="id">屏蔽ID主键</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete(int? id)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ID", DbType.Int32, id));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_PBConfig_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

    }
}
