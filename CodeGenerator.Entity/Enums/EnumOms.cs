using System.ComponentModel;

namespace CodeGenerator.Entity.Enums
{

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum EnumOrderStatus
    {
        [Description("未付款")]
        NonPay = 100,
        [Description("支付成功")]
        PaySuccess = 200,
        [Description("已发货")]
        Delivered = 300,
        [Description("付款失败")]
        Failed = 400,
        [Description("客户取消")]
        Cancel = 404,
        [Description("已完成")]
        Finish = 900

    }

    /// <summary>
    /// 订单来源
    /// </summary>
    public enum EnumOrderSource
    {
        [Description("微信商城客户")]
        WeChatClient = 100,
        [Description("微信商城业务员")]
        WeChatMerchandiser = 101,
        [Description("小程序")]
        Applet = 200
    }

    
}
