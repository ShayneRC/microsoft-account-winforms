using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftAccount.WindowsForms
{
    public static class Logging
    {
        public static void Log(string message)
        {
            string fileLoc = $"{AppDomain.CurrentDomain.BaseDirectory}/MicrosoftAccount.WindowsForms.txt";

            string contents = string.Empty;
            if (File.Exists(fileLoc))
                contents = File.ReadAllText(fileLoc);

            File.WriteAllText(fileLoc, $"{contents}{(string.IsNullOrEmpty(contents) ? "" : "\n")}{message}");
        }
    }
}
