namespace Demo02_Event
{

    public class NewPublisher
    {
        // 直接暴露委托字段
        public Action<string>? OnNewPublished;

        public void Publish(string news)
        {
            OnNewPublished?.Invoke(news);
        }
    }

    #region 事件标准用法

   
    //1. 自定义事件参数
    public class NewEventArgs : EventArgs
    {
        public string HeadLine { get; set; }

        public DateTime PublishTime { get; set; }

        public NewEventArgs(string headline)
        {
            HeadLine = headline;
            PublishTime = DateTime.Now;
        }
    }
    //2.使用事件给委托加一把"安全锁",
    //  声明事件发布者
    public class NewsPublisher
    {
        //使用event 关键字 声明事件
        public event EventHandler<NewEventArgs>? NewsPublished;

        public void Publish(string headlines)
        {
            Console.WriteLine($"发布新闻");
            //触发事件
            OnNewPublished(new NewEventArgs(headlines));
        }

        //protected 修饰，类内部调用保护触发方法
        //这里封装了触发逻辑：未来可以添加日志之类的工具调用。
        //使用virtual ：则支持继承，子类（派生类）可以重写override这个方法
        //一般用OnXXX的方式命名
        protected virtual void OnNewPublished(NewEventArgs e)
        {
            Console.WriteLine($"当前订阅数量:{NewsPublished?.GetInvocationList().Count()}");
            //集中处理，避免到处空引用检查
            NewsPublished?.Invoke(this, e);
        }
    }

    //3. 事件订阅者，事件触发处理事件
    public class NewsSubscriber
    {
        private string  _name;

        private NewsPublisher _publisher;

        public NewsSubscriber(string name,NewsPublisher publisher)
        {
            _name = name;
            //订阅事件
            _publisher=publisher;//这里大概要判断传进来的publisher是否为空好点。

            _publisher.NewsPublished += HandleNews;
        }

        private void HandleNews(object? sender, NewEventArgs e)
        {
            Console.WriteLine($"[{_name}] 收到消息：{e.HeadLine}；时间：{e.PublishTime}");
        }

        public void Unsubscribe()
        {
            //仅卸载当前类实例的事件订阅
            _publisher?.NewsPublished -= HandleNews;
        }
    }
    #endregion


    internal class Program
    {
        static void Main(string[] args)
        {
            #region 如果直接暴露委托
            //BadBad();
            #endregion

            //标准事件
            UsingCase.Run();
        }

        private static void BadBad()
        {
            var publisher = new NewPublisher();

            //订阅者A注册回调
            publisher.OnNewPublished += msg => Console.WriteLine($"A收到：{msg}");
            //订阅者B注册回调
            publisher.OnNewPublished += msg => Console.WriteLine($"B收到：{msg}");

            publisher.Publish("Breaking News!!!!");

            //===如果订阅突然被清空了
            publisher.OnNewPublished = null;
            //Guess what
            publisher.Publish("Morning News!!!!!");
        }

        private static void GoodGood()
        {
            //事件通过+=、-= 这两个操作符来执行订阅和取消订阅
            //创建发布者实例
            var publisher = new NewsPublisher();

            //创建订阅者，构造方法内部自动订阅事件
            var subA = new NewsSubscriber("订阅者A",publisher);
            var subB = new NewsSubscriber("订阅者B",publisher);

            //发布新闻

            publisher.Publish("财经新闻");
            publisher.Publish("科技新闻");

            //A 取消订阅

            subA.Unsubscribe();
            publisher.Publish("体育新闻");

        }


    }
}
