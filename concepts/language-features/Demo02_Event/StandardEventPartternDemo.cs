using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace Demo02_Event
{

    // 事件“标准”写法
    //1. 声明一个EventArgs
    //没数据也建议声明定义一个，以后扩展用得着。
    public class DownloadProgressEventArgs : EventArgs
    {
        public int Percentage { get; }
        public long BytesReceived { get; }

        public DownloadProgressEventArgs(int percentage, long bytes)
        {
            Percentage = percentage;
            BytesReceived = bytes;
        }
    }


    //2. 声明发布者
    public class FileDownloader
    {
        //2.1 使用EventHandler<T> 标准参数签名(sender,e)
        //2.2 使用event声明这是一个事件。
        //2.3 通常用ed结尾，毕竟都是干完活了。
        //2.4 事件有可能为空，记得处理。
        public event EventHandler<DownloadProgressEventArgs>? ProgressChanged;

        public event EventHandler? DownloadCompleted;

        //模拟过程
        public void Start()
        {
            Console.WriteLine($"开始下载，当前时间{DateTime.Now}");

            for (int i = 0; i <= 100; i += 20)
            {
                //3. 触发事件 
                OnProgressChanged(new DownloadProgressEventArgs(i, i * 1024));
                //模拟耗时
                Thread.Sleep(500);
            }

            //模拟完成
            OnDownloadCompleted(EventArgs.Empty);
        }

        // 4. 定义触发方法
        //protected virtual:允许子类重写，在触发前后插入逻辑
        protected virtual void OnProgressChanged(DownloadProgressEventArgs e)
        {
            //this 表示，告诉订阅者这是我发出的消息
            ProgressChanged?.Invoke(this, e);
        }

        protected virtual void OnDownloadCompleted(EventArgs e)
        {
            DownloadCompleted?.Invoke(this, e);
        }
    }

    /// <summary>
    /// 用例
    /// </summary>
    public class UsingCase
    {
        public static void Run()
        {
            var downloader = new FileDownloader();

            //订阅进度
            //sender
            downloader.ProgressChanged += (sender, e) =>
            {
                Console.WriteLine($"[进度更新]：{new string('#', e.Percentage / 5),-20} {e.Percentage}%；{e.BytesReceived}");
            };

            downloader.DownloadCompleted += (sender, e) =>
            {
                Console.WriteLine($"[完成时间]：{DateTime.Now}");
            };

            //发布事件
            downloader.Start();
        }
    }
}
