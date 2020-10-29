using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sites.EncryptionFilter
{
    public static class BasicFilter
    {
        private static string[] FilterQuery = { "DB_NAME", "DB_USER", "DB_PASSWORD", "DB_HOST" };
        private static string[] FilterRegular = { "'(.*)'", "\"(.*)\""};
        public static string DemoFilter(string Response)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var SubQuery in FilterQuery)
            { 
                foreach (var Filter in FilterRegular)
                {
                    foreach (Match match in new Regex($"'{SubQuery}', {Filter}").Matches(Response))
                    {
                        sb.Append(string.Concat($"{SubQuery}: ", match.Groups[1].Value, " "));
                    }
                }
            }

            return sb.ToString();
        }
    }
}
