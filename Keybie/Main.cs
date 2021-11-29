using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private readonly string _configFile = ".config";
        public Subject<string> InputEvent = new Subject<string>();
        private SerialPort _serial;
        public ObservableCollection<Input> Inputs { get; set; }
        public ObservableCollection<string> Ports { get; set; }
        public ObservableCollection<int> BaudRates { get; set; }
        public ObservableCollection<RunProcessAction> Actions { get; set; }

        private string _SelectedPort;
        public string SelectedPort { get { return _SelectedPort; } set { SetProperty(ref _SelectedPort, value); RefreshCommands(); } }

        private int _SelectedBaudRate;
        public int SelectedBaudRate { get { return _SelectedBaudRate; } set { SetProperty(ref _SelectedBaudRate, value); RefreshCommands(); } }


        private bool _UnLockUi = true;
        public bool UnLockUi { get { return _UnLockUi; } set { SetProperty(ref _UnLockUi, value); } }


        private string _LastInput;
        public string LastInput { get { return _LastInput; } set { SetProperty(ref _LastInput, value); } }


        private DelegateCommand _StartCommand;
        public DelegateCommand StartCommand
        {
            get
            {
                if (_StartCommand == null) _StartCommand = new DelegateCommand(OnStartCommand, () => _serial == null && !string.IsNullOrEmpty(SelectedPort));
                return _StartCommand;
            }
        }

        public void OnStartCommand()
        {
            if (_serial == null)
                _serial = new SerialPort();

            if (_serial.IsOpen)
                _serial.Close();

            _serial.PortName = SelectedPort;
            _serial.BaudRate = SelectedBaudRate;
            _serial.DataReceived += _serial_DataReceived;
            _serial.Open();
            UnLockUi = false;

            RefreshCommands();
        }

        private void RefreshCommands()
        {
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        private DelegateCommand _StopCommand;
        public DelegateCommand StopCommand
        {
            get
            {
                if (_StopCommand == null) _StopCommand = new DelegateCommand(OnStopCommand, () => _serial != null && _serial.IsOpen);
                return _StopCommand;
            }
        }

        public void OnStopCommand()
        {
            if (_serial != null)
            {
                _serial.DataReceived -= _serial_DataReceived;
                _serial.Dispose();
                _serial = null;
            }
            UnLockUi = true;
            RefreshCommands();
        }


        private DelegateCommand _RefreshCommand;
        public DelegateCommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null) _RefreshCommand = new DelegateCommand(OnRefreshCommand);
                return _RefreshCommand;
            }
        }

        public void OnRefreshCommand()
        {
            GetPorts();
        }


        private DelegateCommand _AddRunProcessCommand;
        public DelegateCommand AddRunProcessCommand
        {
            get
            {
                if (_AddRunProcessCommand == null) _AddRunProcessCommand = new DelegateCommand(OnAddRunProcessCommand);
                return _AddRunProcessCommand;
            }
        }

        public void OnAddRunProcessCommand()
        {
            Actions.Add(new RunProcessAction());
        }


        private DelegateCommand _SaveConfigCommand;
        public DelegateCommand SaveConfigCommand
        {
            get
            {
                if (_SaveConfigCommand == null) _SaveConfigCommand = new DelegateCommand(OnSaveConfigCommand);
                return _SaveConfigCommand;
            }
        }

        public void OnSaveConfigCommand()
        {
            try
            {
                var config = JsonConvert.SerializeObject(Actions);
                File.WriteAllText(_configFile, config);
            }
            catch { }
        }


        public Main()
        {
            Actions = new ObservableCollection<RunProcessAction>();
            BaudRates = new ObservableCollection<int>() { 9600, 115200 };
            SelectedBaudRate = 115200;
            Ports = new ObservableCollection<string>();
            Inputs = new ObservableCollection<Input>();

            try
            {
                var config = File.ReadAllText(_configFile);
                Actions.AddRange(JsonConvert.DeserializeObject<List<RunProcessAction>>(config));
            }
            catch { }

            InputEvent.ObserveOnDispatcher().Subscribe(OnInputEvent);
            GetPorts();
        }

        private void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string input = string.Empty;
            DateTime dateTime = DateTime.Now;
            while (DateTime.Now - dateTime < TimeSpan.FromMilliseconds(250))
            {
                if (_serial.BytesToRead > 0)
                {
                    int read = _serial.ReadChar();
                    input += read.ToString("X2");
                }
            }
            InputEvent.OnNext(input);
            Console.WriteLine(input);
        }

        private void OnInputEvent(string obj)
        {
            LastInput = obj;
            var act = Actions.FirstOrDefault(x => x.Trigger == obj);
            if (act != null)
            {
                act.Execute();
            }
        }

        private void GetPorts()
        {
            var ports = SerialPort.GetPortNames();
            Ports.Clear();
            Ports.AddRange(ports);
        }
    }
}
