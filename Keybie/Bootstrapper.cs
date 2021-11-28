using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    static class Bootstrapper
    {
        public static void Start()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
