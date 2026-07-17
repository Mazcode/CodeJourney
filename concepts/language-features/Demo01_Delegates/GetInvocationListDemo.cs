using System;
using System.Collections.Generic;
using System.Text;

namespace Demo01_Delegates
{
    internal class GetInvocationListDemo
    {
        public static void Run()
        {
            //一个多播委托，如果直接执行，它会把链表里的方法逐个执行一次。但是只保留最后一个方法的返回值
            //但在实际开发中，我们经常会有这样的需求：
            //“我有 5 个校验规则（Func），只要有一个返回 False，我就认为校验失败；或者我想收集所有校验规则的报错信息。”

            //核心 GetInvocationList方法
            //这个方法返回的是一个Delegate[]数组，因为这是基类，所以要在使用这个数组的元素的时候进行类型转换
            //一般用for或者foreach循环

            //假设我们要校验一个数字，有三个规则：
            //必须大于 0。
            //必须小于 100。
            //必须是偶数。

            Func<int, bool> checklist = (num) => num > 0;

            checklist += (num) => num < 100;

            checklist += (num) => num % 2 == 0;

            var deleArr = checklist.GetInvocationList();
            //存放执行结果
            List<bool> results = new List<bool>();
            var count = deleArr.Count();
            for (int i = 0; i < count; i++)
            {
                Func<int, bool> func = (Func<int, bool>)deleArr[i];//类型转换
                results.Add(func(20));//收集结果
            }
            //这里要取反
            var isPassed = !results.Any(a => a == false);
            if (isPassed)
            {
                Console.WriteLine("通过校验");
            }
            else
            {
                Console.WriteLine("校验失败");
            }


            // linq写法
            //“拆链用 GetInvocationList，强转靠 Cast，结果用 Select 收”
            var results2 = checklist?.GetInvocationList().Cast<Func<int, bool>>().Select(f => f(9)).ToList().All(a => a == true);
            Console.WriteLine($"results2;{results2}");

            //两种写法的对比，其实结果都是一样的，如果要处理异常的就用for循环或者foreach循环，适合生产环境，需要精细控制的场景
            //linq 就是简洁，适合简单的，爽写。
        }


        public static void SafeInvoke()
        {
            Func<int, bool> validators = num => num > 0;
            validators += num => num < 100;
            validators += num => num %2 == 0;

            var result = SafeInvokeAll(validators, 146);
            Console.WriteLine($"所有结果：{string.Join('-', result)}");

            //塞个异常进去看看
            validators += num => throw new Exception("故意塞个异常");
            result = SafeInvokeAll(validators, 9);
            Console.WriteLine($"所有结果：{string.Join('-', result)}");
        }

        private static List<bool> SafeInvokeAll(Func<int, bool> multiCastDelegate, int input)
        {
            var result = new List<bool>();

            if (multiCastDelegate==null)//当多播委托没有订阅任何方法，直接调用GetInvocationList()会抛出NullReferenceException
            {
                //为空直接返回，验证失败
                return result;
            }

            //拆链+循环
            foreach (var del in multiCastDelegate.GetInvocationList())
            {
                try
                {
                    //类型转换，执行方法
                    var func = (Func<int, bool>) del;
                    result.Add(func(input));
                }
                catch (Exception ex)
                {
                    //多播中单个委托的崩溃不应该阻断全局流程
                    Console.WriteLine($"[警告] 委托 {del.Method.Name} 执行失败：{ex.Message}");
                    //默认给false
                    result.Add(false);
                }
            }

            return result;

        }
    
    }
}
