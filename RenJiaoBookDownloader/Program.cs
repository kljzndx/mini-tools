using System.Diagnostics;
using System.Linq;
using System.Text;

using static System.Net.WebRequestMethods;

namespace RenJiaoBookDownloader
{
    internal class Program
    {
        const string Host = "https://book.pep.com.cn";
        const string IndexPage = "/mobile/index.html";
        const string ConfigUrl = "/mobile/javascript/config.js";

        const string Cf_bookTitle = "bookTitle";
        const string Cf_totalPageCount = "totalPageCount";
        const string Cf_CreatedTime = "CreatedTime";
        const string Cf_largePath = "largePath";
        const string Cf_largePageWidth = "largePageWidth";
        const string Cf_largePageHeight = "largePageHeight";

        static HttpClient Http;

        static Program()
        {
            Http = new HttpClient();
            Http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0");
        }

        static async Task Main(string[] args)
        {
            Console.Title = "人教版教材下载器";
            Console.WriteLine("人教版教材下载器 v1.0.0");

            Console.Write("请输入书籍预览页面地址（留空则会打开书籍目录）：");
            string address = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(address))
                Process.Start(new ProcessStartInfo("https://jc.pep.com.cn/") { UseShellExecute = true });

            if (address.StartsWith(Host) && address.EndsWith(IndexPage))
            {
                Http.DefaultRequestHeaders.Referrer = new Uri(address);
                address = address.Replace(IndexPage, string.Empty);

                try
                {
                    await ParseConfig(address);
                }
                catch (Exception)
                {
                }
            }

            Console.WriteLine("按下回车键即可退出……");
            Console.ReadLine();
        }

        static async Task<Dictionary<string, string>> ParseConfig(string url)
        {
            var dc = new Dictionary<string, string>();

            string[] keys = new string[]
            {
                Cf_bookTitle,
                Cf_totalPageCount,
                Cf_CreatedTime,

                Cf_largePath,
                Cf_largePageWidth,
                Cf_largePageHeight,
            };

            Console.Write("正在获取书籍配置文件...");
            string js = "";

            try
            {
                js = await Http.GetStringAsync(url + ConfigUrl);
                Console.WriteLine("成功");
            }
            catch (Exception)
            {
                Console.WriteLine("失败");
                throw;
            }

            foreach (string key in keys)
            {
                Console.Write($"正在读取 {key} ... ");
                string jKey = $"bookConfig.{key}";

                int iId = js.IndexOf(jKey);
                if (iId < 0)
                {
                    Console.WriteLine("失败");
                    throw new Exception();
                }
                int jId = iId + jKey.Length;
                StringBuilder sb = new StringBuilder();

                while (js[jId] != ';')
                {
                    if (js[jId] != '=' && js[jId] != '"')
                        sb.Append(js[jId]);

                    jId++;
                }

                string val = sb.ToString().Trim().TrimStart('.');
                dc[key] = val;
                Console.WriteLine(val);
            }

            return dc;
        }
    }
}
