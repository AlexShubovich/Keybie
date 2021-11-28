using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    class Input : BindableBase
    {
        public string Raw { get; private set; }
        public string Key { get; private set; }

        public Input(string input)
        {
            Raw = input;
            int num = int.Parse(input);

            switch (num)
            {
                case 9:
                    Key = "[Tab]";
                    break;
                case 13:
                    Key = "[Enter]";
                    break;
                case 10:
                    Key = "[LineFeed]";
                    break;
                case 127:
                    Key = "[BackSpace]";
                    break;
                case 27:
                    Key = "[Escape]";
                    break;
                case 25:
                    Key = "[PageUp]";
                    break;
                case 26:
                    Key = "[PageDown]";
                    break;
                case 11:
                    Key = "[Up]";
                    break;
                case 8:
                    Key = "[Left]";
                    break;
                case 12:
                    Key = "[Down]";
                    break;
                case 21:
                    Key = "[Right]";
                    break;
                case 32:
                    Key = "[Space]";
                    break;
                default:
                    Key = ((char)num).ToString();
                    break;
            }

            
        }
    }
}
