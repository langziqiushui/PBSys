using System;

using YX.Core;

namespace YX.Domain
{
    #region 全局


    /// <summary>
    /// 应用程序类别。
    /// </summary>
    [Serializable]
    public enum ApplicationTypes
    {
        /// <summary>
        /// 易玩。
        /// </summary>
        [EnumDescription("易玩")]
        myiwan = 11,

        ///// <summary>
        ///// 本系统
        ///// </summary>
        //[EnumDescription("本系统")]
        //SYS = 0,

        /// <summary>
        /// 安秀网
        /// </summary>
        [EnumDescription("安秀网")]
        manxiu = 1,

        /// <summary>
        /// 快啦网
        /// </summary>
        [EnumDescription("快啦网")]
        mkuaila = 2,

        /// <summary>
        /// 一姐网
        /// </summary>
        [EnumDescription("一姐网")]
        m1j1j = 3,

        /// <summary>
        /// 蓝企鹅网
        /// </summary>
        [EnumDescription("蓝企鹅网")]
        mlanqie = 4,

        

        /// <summary>
        /// 巴乌下载。
        /// </summary>
        [EnumDescription("巴乌下载网")]
        myx85 = 6,
        /// <summary>
        /// 清清下载吧。
        /// </summary>
        [EnumDescription("清清下载吧")]
        mqqxzb = 7,
        /// <summary>
        /// 手机497。
        /// </summary>
        [EnumDescription("手机497")]
        m497 = 8,

        /// <summary>
        /// 手机56。
        /// </summary>
        [EnumDescription("手机56")]
        mshouji56 = 9,

        /// <summary>
        /// 手机499。
        /// </summary>
        [EnumDescription("手机499")]
        m499 = 10,

        /// <summary>
        /// 手机391K。
        /// </summary>
        [EnumDescription("391K")]
        m391k = 12,

        /// <summary>
        /// pp8。
        /// </summary>
        [EnumDescription("pp8")]
        mpp8 = 13,

        /// <summary>
        /// 草鞋网。
        /// </summary>
        [EnumDescription("草鞋网")]
        mcaoxie = 14,

        /// <summary>
        /// app抢红包。
        /// </summary>
        [EnumDescription("抢红包")]
        mqianghongbao = 15,

        /// <summary>
        /// 微信多开。
        /// </summary>
        [EnumDescription("微信多开")]
        mweixin = 16,

        /// <summary>
        /// 交友。
        /// </summary>
        [EnumDescription("交友")]
        mjiaoyou = 17,

        /// <summary>
        /// 小说。
        /// </summary>
        [EnumDescription("小说")]
        mstory = 18,

        /// <summary>
        /// BB题库。
        /// </summary>
        [EnumDescription("BB题库")]
        mbbtiku = 19,

        /// <summary>
        /// 职业心理测试。
        /// </summary>
        [EnumDescription("职业心理测试")]
        mtest = 20,

        /// <summary>
        /// 直播。
        /// </summary>
        [EnumDescription("直播")]
        mzhibo = 21,
        /// <summary>
        /// 盒子。
        /// </summary>
        [EnumDescription("盒子")]
        mhezi = 22,

        /// <summary>
        /// 盒子。
        /// </summary>
        [EnumDescription("8K9K网")]
        m8k9k = 23,

        /// <summary>
        /// 游讯网。
        /// </summary>
        [EnumDescription("游讯网-单机")]
        myxdown = 5,

        /// <summary>
        /// 游讯网。
        /// </summary>
        [EnumDescription("游讯网-手游")]
        myxdownsy = 24,

        /// <summary>
        /// 游讯网。
        /// </summary>
        [EnumDescription("游讯网-PC软件")]
        myxdownpcsoft = 25,

        //安秀网=1，快啦网=2，一姐网=3，蓝企鹅=4，游讯网=5，巴乌下载=6，清清下载吧=7,手机497=8，手机56=9,499=10，易玩=11,391K=12，
        //pp8 =13,草鞋网=14,app抢红包=15，微信多开=16，安卓交友=17，小说=18，BB题库=19，职业心理测试=20，直播=21 盒子=22
    }

    /// <summary>
    /// 日志类别。
    /// </summary>
    public enum LogTypes
    {
        /// <summary>
        /// 错误400页面
        /// </summary>
        [EnumDescription("错误400页面", "gray")]
        错误400页面 = 0,
        /// <summary>
        /// 错误500异常
        /// </summary>
        [EnumDescription("错误500异常", "black")]
        错误500异常 = 1,
        /// <summary>
        /// 自定义错误
        /// </summary>
        [EnumDescription("自定义错误", "black")]
        自定义错误 = 2,
        ///// <summary>
        ///// 接口错误
        ///// </summary>
        //[EnumDescription("接口错误", "black")]
        //接口错误 = 3,
        /////// <summary>
        /////// 接口错误
        /////// </summary>
        ////[EnumDescription("本系统错误", "black")]
        ////本系统错误 = 4

        //日志类别[LogTypes]400 = 0,500 = 1, 自定义=2 ,接口错误 = 3
    }


    /// <summary>
    /// 屏蔽类别。
    /// </summary>
    public enum PbTypes
    {
        /// <summary>
        /// 软件。
        /// </summary>
        [EnumDescription("软件", "red")]
        soft = 0,
        /// <summary>
        /// 新闻。
        /// </summary>
        [EnumDescription("新闻", "black")]
        news = 1,
        /// <summary>
        /// 专题。
        /// </summary>
        [EnumDescription("专题", "black")]
        zt = 2

        //屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2 
    }


    /// <summary>
    /// 站点类别。
    /// </summary>
    public enum Restypes
    {
        /// <summary>
        /// M站。
        /// </summary>
        [EnumDescription("M站", "red")]
        Msite = 0,
        /// <summary>
        /// PC站。
        /// </summary>
        [EnumDescription("PC站", "black")]
        PCsite = 1,
        /// <summary>
        /// 后台。
        /// </summary>
        [EnumDescription("后台", "black")]
        ADMINsite = 2,
        /// <summary>
        /// APP。
        /// </summary>
        [EnumDescription("APP", "black")]
        APP = 3,

        /// <summary>
        /// 接口。
        /// </summary>
        [EnumDescription("API接口", "black")]
        API = 4

        //类别类别[Restypes]M站 = 0,WEB站 = 1, 后台=2, APP = 3, 接口=4 
    }

    #endregion

    #region 管理员类别

    /// <summary>
    /// 管理员类别。
    /// </summary>
    public enum AdminTypes
    {
        /// <summary>
        /// 超级管理员。
        /// </summary>
        [EnumDescription("超级管理员", "red")]
        Super = 1,

        /// <summary>
        /// 主编。
        /// </summary>
        [EnumDescription("主编", "black")]
        Normal = 2,

        /// <summary>
        /// 编辑。
        /// </summary>
        [EnumDescription("编辑", "black")]
        bianji = 3
        //[AdminTypes]超级管理员=1,主编=2，编辑=3
    }

    #endregion


}
