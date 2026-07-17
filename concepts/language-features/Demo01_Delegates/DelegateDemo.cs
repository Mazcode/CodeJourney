using System;
using System.Collections.Generic;
using System.Text;

namespace Demo01_Delegates
{
    internal class DelegateDemo
    {
        #region 多播委托
        //多播委托===当委托执行时，触发一系列的动作。
        public delegate void EmployeeAction(string message);
        /// <summary>
        /// 多播委托的演示
        /// </summary>
        public static void MulticastDelegateRun()
        {
            //2. 实例化
            EmployeeAction actions = new EmployeeAction(Order);

            actions += Product;
            actions += Storge;

            actions("搞点香蕉");


            EmployeeAction actions2 = Storge;
            actions2 += Order;

            //使用匿名方法
            actions2 += (todo) => Console.WriteLine($"嗨嗨嗨~~~{todo}");

            actions2("去码头搞点薯条");
        }

        static void Order(string message)
        {
            Console.WriteLine($"Order phase :{message}");
        }

        static void Product(string message)
        {
            Console.WriteLine($"Product phase :{message}");
        }

        static void Storge(string message)
        {
            Console.WriteLine($"Storge phase :{message}");
        }

        #endregion

        #region 自带的Action和Func

        /// <summary>
        /// 预先定义了的Action和Func
        /// </summary>
        public static void UsingActionAndFunc()
        {
            //Action不返回值
            Action<string> steps = Order;
            steps += Product;
            steps += (message) => Console.WriteLine($"匿名方法:{message}");

            steps("表！你好啊");

            //Func带返回值

            Func<int,bool> f1 = Check;
            bool result = f1(-1);
            Console.WriteLine($"传-1 给 f1 的结果{result}");

            f1 += num => { return num % 3 == 0; };
           
            Console.WriteLine($"lambda 的结果{f1(-9)}");
            //这里有坑，多播委托只返回最后一个执行的结果，虽然上面的语句输出是正确的，但是当f1(-9)执行的时候
            //check又被执行了一次，返回的是false，但是最后结果只输出true。也就是被覆盖了。
        }

        /// <summary>
        /// 检查给定的整数是否为正数。
        /// </summary>
        /// <param name="a">待验证的整数值。</param>
        /// <returns>如果数字大于 0 则返回 true；否则返回 false。</returns>
        static bool Check(int a)
        {
            return a > 0;
        }



        #endregion
    }
}
