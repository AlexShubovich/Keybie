using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Keybie
{
    class Main : BindableBase
    {
        public Subject<string> InputEvent = new Subject<string>();
        private SerialPort _serial = new SerialPort("COM8");
        public ObservableCollection<Input> Inputs { get; set; }

        public Main()
        {
            Inputs = new ObservableCollection<Input>();
            InputEvent.ObserveOnDispatcher().Subscribe(OnInputEvent);
            Task.Factory.StartNew(() =>
            {
                _serial.BaudRate = 115200;
                _serial.Open();
                _serial.DataReceived += _serial_DataReceived;
            });
        }

        private void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string input = string.Empty;
            DateTime dateTime = DateTime.Now;
            while (DateTime.Now - dateTime < TimeSpan.FromMilliseconds(500))
            {
                if (_serial.BytesToRead > 0)
                {
                    int read = _serial.ReadChar();
                    input += read.ToString("X2");
                }
            }
            Console.WriteLine(input);
        }

        private void OnInputEvent(string obj)
        {



        }


    }
}
