using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;
using Sites.Core;

namespace Sites
{
    class Program
    {


        private static void Main()
        {
            Console.Write("Drag file with URL's: ");
            string[] urls = File.ReadAllLines(Console.ReadLine());
            Console.Write("Enter number of threads: ");
            int Threads = Convert.ToInt32(Console.ReadLine());
            CheryCore chery = new CheryCore(urls);
            chery.Starter(Threads);

            Console.ReadKey();

        }
    }
}
