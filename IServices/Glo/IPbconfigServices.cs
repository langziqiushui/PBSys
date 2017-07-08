// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/9 12:35:57 
// Description：业务服务接口。 
// ===============================================================================

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using YX.Core;
using YX.Domain;

using YX.Domain.Glo;

namespace YX.IServices.Glo
{
    /// <summary> 
    /// 业务服务接口(对应表 Glo.PBConfig (屏蔽表) )。 
    /// </summary> 
    public interface IPbconfigServices : IDisposable
    {
        #region 获取数据

        /// <summary> 
        /// 获取所有屏蔽。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        IList<PbconfigInfo> GetData_All();

        /// <summary> 
        /// 根据条件获取是否有屏蔽数据。 
        /// </summary> 
        /// <param name="applicationtype">应用程序类别[ApplicationTypes] 1= yx 2=anxiu 3=pp8 4=...</param> 
        /// <param name="pbtype">屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2</param> 
        /// <param name="keydata">主键</param> 
        /// <returns>查询获取的数据</returns> 
        object GetData_Bywhere(byte? applicationtype, byte? pbtype, int? keydata);

        /// <summary> 
        /// 获取某一屏蔽。 
        /// </summary> 
        /// <param name="id">屏蔽ID主键</param> 
        /// <returns>查询获取的数据</returns> 
        PbconfigInfo GetData(int? id);

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
        IList<PbconfigInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, byte? applicationtype, byte? pbtype, int? keydata);

        #endregion

        #region 添加数据

        /// <summary> 
        /// 创建屏蔽。 
        /// </summary> 
        /// <param name="entity">PbconfigInfo业务实体对象</param> 
        /// <param name="id">插入对象的自动递增编号</param> 
        /// <returns>数据添加是否成功</returns> 
        object Add(PbconfigInfo entity, ref int id);

        #endregion

        #region 修改数据

        /// <summary> 
        /// 修改某一屏蔽。 
        /// </summary> 
        /// <param name="entity">PbconfigInfo业务实体对象</param> 
        /// <returns>数据修改是否成功</returns> 
        object Modify(PbconfigInfo entity);

        #endregion

        #region 删除数据

        /// <summary> 
        /// 删除某一屏蔽。 
        /// </summary> 
        /// <param name="id">屏蔽ID主键</param> 
        /// <returns>数据删除是否成功</returns> 
        object Delete(int? id);

        /// <summary> 
        /// 启用事务一次性删除多条数据。 
        /// </summary> 
        bool Delete(params int[] ids);

        #endregion

    }

}
