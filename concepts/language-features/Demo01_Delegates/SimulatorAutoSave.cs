using System;
using System.Collections.Generic;
using System.Text;

namespace Demo01_Delegates
{
    //模拟通用自动保存组件（完全不知道使用者是谁）
    internal class SimulatorAutoSave
    {
        //通过使用Action<T>和Func<T,bool>如何实现模板方法和插件化
        // 插件点1：保存前校验钩子（Func<T,bool>）→ 决定是否允许保存
        public Func<string,bool> BeforeSave {  get; set; }
        // 插件点2：保存后通知钩子（Action<T>）→ 通知使用者完成
        public Action<string> AfterSaved { get; set; }

        // 模板方法：固定流程（使用者无法修改核心步骤）
        public void Save(string content)
        {
            Console.WriteLine("模板方法开始执行流程");

            //钩子1，执行用户自定义校验
            if (BeforeSave!=null&&!BeforeSave(content))
            {
                Console.WriteLine("校验失败！保存中断!");
                return;
            }

            //核心逻辑
            Console.WriteLine($"保存内容:{content}");

            //钩子2 通知用户完成
            AfterSaved?.Invoke(content);

            Console.WriteLine("保存结束");
        }
    }

    //调用例子
    public class SASUsingCase
    {
        public static void Run()
        {
            var sas = new SimulatorAutoSave();

            //开始注册自定义的逻辑
            sas.BeforeSave = content =>
            {
                Console.WriteLine("[自定义校验] 开始执行......");
                bool isValid = !string.IsNullOrEmpty(content);
                Console.WriteLine($"[自定义校验] 校验结果: {(isValid ? "通过" : "拒绝")}");
                return isValid;
            };

            // 插件2：保存后通知方式（Action<string>）
            sas.AfterSaved = content =>
            {
                Console.WriteLine("[插件] 执行自定义通知...");
                Console.WriteLine($"[插件] 保存成功！内容长度: {content.Length}");
            };


            // 模拟业务场景调用
            Console.WriteLine("===== 场景1：有效内容 =====");
            sas.Save("Server=prod;Timeout=30");

            Console.WriteLine("\n===== 场景2：空内容 =====");
            sas.Save("");

        }
    }
}
