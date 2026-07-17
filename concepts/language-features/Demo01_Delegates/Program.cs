namespace Demo01_Delegates
{
    internal class Program
    {

        //1. 定义委托
        // 委托就是拿来存放方法的。
        // 这个委托MyDelegate就是接收一个string类型参数，并且没有返回的方法
        public delegate void MyDelegate(string message);//这个委托不需要返回值

        static void Main(string[] args)
        {
            ////2. 实例化委托，把方法塞进去
            //MyDelegate myDel = new MyDelegate(Greeting);

            ////3. 调用，跟方法一样,实际就是调用myDel指向的方法Greeting。
            //myDel("Banana");

            ////另外的写法，简单直接
            //MyDelegate myDel2 = Greeting;
            //myDel2("Crazy");


            //多播委托
            //DelegateDemo.MulticastDelegateRun();

            // DelegateDemo.UsingActionAndFunc();

            //GetInvocationListDemo.Run();
            //Console.WriteLine("华丽的分割线================");
            //GetInvocationListDemo.SafeInvoke();


            //回调函数
            //CallBackDemo.Run();

            SASUsingCase.Run();

            Console.ReadKey();
        }

        //符合委托签名的方法
        static void Greeting(string name)
        {
            Console.WriteLine($"Hello {name}");
        }
    }
}
