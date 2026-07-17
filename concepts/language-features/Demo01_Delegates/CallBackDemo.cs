using System;
using System.Collections.Generic;
using System.Text;

namespace Demo01_Delegates
{
    /// <summary>
    /// 回调函数
    /// </summary>
    internal class CallBackDemo
    {
        public static void Run()
        {
            Console.WriteLine("委托练习-----回调函数");
            Console.WriteLine("模拟下载文件.........");

            FileDownloader downloader = new FileDownloader();
            downloader.Download("Crazy_Banana.exe", fileName => Console.WriteLine($"文件 {fileName} 开始自动安装..."));
            //无回调
            downloader.Download("readme.txt", null); // 无回调
        }

    }

    internal class FileDownloader
    {
        /// <summary>
        /// 模拟文件下载方法
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="onCompleted">完成时回调函数</param>
        public void Download(string fileName, Action<string> onCompleted)
        {
            //模拟耗时操作
            Thread.Sleep(2000);

            Console.WriteLine($"{fileName} 下载完成！");

            if (onCompleted != null)
            {
                try
                {
                    onCompleted(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{fileName},回调调用异常"); // （生产环境应替换为日志记录）
                    throw new FileProcessingException(
                        "CALLBACK_FAILED",
                        $"文件 {fileName} 回调失败",
                        ex //传入原始异常作为内层异常
                    );
                }
            }
        }

    }



    public class FileProcessingException : Exception
    {
        public string ErrorCode { get; }

        public FileProcessingException(string errorCode, string message, Exception? innerException)
            : base(message, innerException)//保留异常链
        {
            ErrorCode = errorCode;

        }
    }

    /// <summary>
    /// 聚合异常类
    /// </summary>
    public class AggregateFileProcessingException : FileProcessingException
    {
        public IReadOnlyList<Exception> InnerExceptions { get; }

        public AggregateFileProcessingException(
            string errorCode,
            string message,
            IEnumerable<Exception> innerExceptions
        ) : base(errorCode, message, null) // 注意：这里不传单个 innerException
        {
            InnerExceptions = innerExceptions.ToList().AsReadOnly();
        }

        // 重写异常消息，包含所有子异常
        public override string Message =>
            $"{base.Message} [{string.Join("; ", InnerExceptions.Select(e => e.Message))}]";
    }


  

}
