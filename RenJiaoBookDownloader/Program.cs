using QuestPDF.Fluent;
using QuestPDF.Helpers;

using SkiaSharp;

using System.Diagnostics;
using System.Text;

using File = System.IO.File;

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

        static HttpClient Http;

        static Program()
        {
            Http = new HttpClient();
            Http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0");
        }

        static async Task Main(string[] args)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            Console.Title = "人教版教材下载器";

            while (true)
            {
                Console.Out.Flush();

                Console.WriteLine("人教版教材下载器 v1.0.0");

                Console.Write("请输入书籍预览页面地址（留空则会打开书籍目录）：");
                string address = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(address))
                {
                    Process.Start(new ProcessStartInfo("https://jc.pep.com.cn/") { UseShellExecute = true });
                    continue;
                }

                Dictionary<string, string> cfs = new Dictionary<string, string>();
                if (address.StartsWith(Host) && address.EndsWith(IndexPage))
                {
                    Http.DefaultRequestHeaders.Referrer = new Uri(address);
                    address = address.Replace(IndexPage, string.Empty);

                    try
                    {
                        cfs = await ParseConfig(address);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                var imgsData = await GetImageData(address + cfs[Cf_largePath], int.Parse(cfs[Cf_totalPageCount]), cfs[Cf_CreatedTime]);

                string filesDir = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, $"files/")).FullName;
                string savePath = Path.Combine(filesDir, $"{cfs[Cf_bookTitle]}.pdf");

                await SavePdf(savePath, imgsData);
                Console.WriteLine($"已保存至：{savePath}");

                Console.WriteLine();
                Console.WriteLine("是否需要继续下载另一本书？ (yes/No)");
                string exitMess = (Console.ReadLine() ?? string.Empty).ToLower();
                if (exitMess != "yes")
                    break;
            }

            Console.WriteLine();
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
            };

            Console.Write("正在获取书籍信息...");
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

        static async Task<List<byte[]>> GetImageData(string dirUrl, int pageCount, string createdTime)
        {
            Console.Write("正在下载 ");
            List<byte[]> data = new List<byte[]>();

            for (int i = 1; i < pageCount + 1; i++)
            {
                Console.SetCursorPosition(9, Console.CursorTop);
                Console.Write($"({i}/{pageCount})");

                data.Add(await Http.GetByteArrayAsync($"{dirUrl}{i}.jpg?{createdTime}"));
                await Task.Delay(100);
            }

            Console.WriteLine();
            return data;
        }

        static async Task SavePdf(string path, List<byte[]> images)
        {
            Console.WriteLine("正在生成PDF...");
            var firstImage = SKBitmap.Decode(images.First());
            var width = firstImage.Width;
            var height = firstImage.Height;

            var data = Document.Create(d =>
            {
                foreach (var image in images)
                {
                    d.Page(p =>
                    {
                        p.Size(new PageSize(width, height));
                        p.Margin(0);

                        p.Content().Image(image).UseOriginalImage();
                        p.PageColor(Colors.White);
                    });
                }
            }).GeneratePdf();
            
            await File.WriteAllBytesAsync(path, data);
        }

    }
}
