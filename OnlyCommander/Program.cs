using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OnlyCommander
{
    class Program
    {
        private static void Main(string[] args)
        {
            var mainFrame = new MainFrame();
            while (mainFrame.Working)
            {
                mainFrame.DispatchMessage();
            }
        }
    }   
}
