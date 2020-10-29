using Leaf.xNet;
using Sites.EncryptionFilter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sites.Core
{
    class CheryCore
    {

        private static int Good = default;
        private static int Bad = default;



        private string[] StaticPages = File.ReadAllLines("Pages.txt");
        private ConcurrentQueue<string> UrlQueue = new ConcurrentQueue<string>();


        public CheryCore(string[] urls)
        {
            foreach (var url in urls)
                UrlQueue.Enqueue(url);


        }
        public void Starter(int threads)
        {
            Console.Title = "Work start";
            for (int i = 0; i <= threads; i++)
                new Thread(ManagedWorker) { IsBackground = true }.Start();
        }

        void ManagedWorker()
        {
            while (UrlQueue.TryDequeue(out var url))
                FetchSite(url);
        }


        void FetchSite(string Url)
        {
            try
            {
                using (HttpRequest httpRequest = new HttpRequest { UserAgent = Http.RandomUserAgent(), IgnoreProtocolErrors = true })
                {
                    foreach (var page in StaticPages)
                    {
                        string Response = httpRequest.Get(string.Concat(Url, page)).ToString(); // Получаем ответ от ссылки
                         // ? Тернарный оператор должен присваивать гуды и беды ?
                        Console.Title = $"Good attempt: {Good} | Bad attempt: {Bad} | Available: {UrlQueue.Count}";
                        var result = BasicFilter.DemoFilter(Response).Length > 0 ? Good++ : Bad++;
                        if (BasicFilter.DemoFilter(Response).Length > 0)
                        {
                            Console.WriteLine($"{Url} ----------------> {BasicFilter.DemoFilter(Response)}");
                            File.AppendAllText($"Good{DateTime.Now.ToShortDateString()}.txt", $"{Url}{page} ---> {BasicFilter.DemoFilter(Response)}{Environment.NewLine}");
                        }

                        Interlocked.Increment(ref result);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger("Debug", "==[EXCEPTION]==");
                Logger("Debug", ex.Message);
                Logger("Debug", ex.StackTrace);
                Logger("Debug", "==[EXCEPTION]==");
                Logger("Debug", Environment.NewLine);
            }

        }

        static object LockLogger = new object();
        static void Logger(string type, string data)
        {
            lock (LockLogger)
            {
                using (StreamWriter sw = new StreamWriter($"{type}.log", true))
                    sw.WriteLine(data);
            }
        }
    }
}
