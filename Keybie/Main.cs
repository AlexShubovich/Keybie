using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
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
            Task.Factory.StartNew(() => {
            _serial.Open();
            while (_serial.IsOpen)
            {
                if(_serial.BytesToRead > 0)
                {
                    InputEvent.OnNext(_serial.ReadExisting());
                }
            }
            });
        }

        private void OnInputEvent(string obj)
        {
            if (!Inputs.Any(x=>x.Raw == obj))
            {
                Inputs.Add(new Input(obj));
            }
        }


    }
}
