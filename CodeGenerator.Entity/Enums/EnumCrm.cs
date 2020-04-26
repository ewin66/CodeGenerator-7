using System.ComponentModel;

namespace CodeGenerator.Entity.Enums
{

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum EnumCusGroupStatus
    {
        [Description("非默认")]
        NotDefault = 0,
        [Description("默认")]
        Default = 1
    }

    /// <summary>
    /// 销售状态
    /// </summary>
    public enum EnumGroupDetailStatus
    {
        [Description("不可售")]
        NotSell = 0,
        [Description("可售")]
        Sell = 1
    }



    /// <summary>
    /// 用户类型
    /// </summary>
    public enum EnumCustomerType
    {
        [Description("客户")]
        Client = 0,
        [Description("业务员")]
        Salesman = 1
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum EnumCustomerStatus
    {
        [Description("正常")]
        Normal = 0,
        [Description("未审核")]
        NotAudit = 1,
        [Description("禁用")]
        Forbidden = 2
    }

}
