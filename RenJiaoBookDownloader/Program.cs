using System.Diagnostics;

namespace RenJiaoBookDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "人教版教材下载器";
            Console.WriteLine("人教版教材下载器 v1.0.0");
            Console.Write("请输入书籍预览页面地址（留空则会打开书籍目录）：");
            string address = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(address))
                Process.Start(new ProcessStartInfo("https://jc.pep.com.cn/") { UseShellExecute = true });

            Console.WriteLine("按下回车键即可退出……");
            Console.ReadLine();
        }
    }
}
