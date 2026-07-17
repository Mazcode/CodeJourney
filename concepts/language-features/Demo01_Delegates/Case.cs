using System;
using System.Collections.Generic;
using System.Text;

namespace Demo01_Delegates
{
    /// <summary>
    /// 应用例子
    /// </summary>
    internal class Case
    {
        /// <summary>
        /// 使用委托解决if-else
        /// </summary>
        private static decimal CalculateShipping(string carrier, double weight)
        {
            if (carrier == "SF") return (decimal)(weight * 20);
            else if (carrier == "YTO") return (decimal)(weight * 12);
            else if (carrier == "JD") return (decimal)(weight * 15);
            // 每加一家快递都要改这里，违反开闭原则
            return 0;
        }
    }

    //1. 定义计算策略的委托
    public delegate decimal ShippingCalculator(double weight);

    public class ShippingService
    {
        //2. 使用字典存储策略，key为快递公司代码,这里实际应该动态管理
        private Dictionary<string, ShippingCalculator> _strategies = new()
        {
            ["SF"] = w => (decimal)(w * 20),
            ["YTO"] = w => (decimal)(w * 12),
            ["JD"] = w => (decimal)(w * 15 + 5) // 考虑有可能有起步价
        };

        //3.按需调用
        public decimal Calculate(string carrier, double weight)
        {
            if (_strategies.TryGetValue(carrier,out var calculator))
            {
                return calculator(weight);
            }
            throw new ArgumentException($"Not Support: {carrier}");
        }
    }


    #region 组件钩子,模板方法

    #endregion
}
