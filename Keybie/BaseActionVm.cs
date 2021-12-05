using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    class BaseActionVm : BindableBase
    {

        private string _Name;
        public string Name { get { return _Name; } set { SetProperty(ref _Name, value); } }

        private string _TriggerCode;
        public string TriggerCode { get { return _TriggerCode; } set { SetProperty(ref _TriggerCode, value); } }


        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null) _SaveCommand = new DelegateCommand(OnSaveCommand);
                return _SaveCommand;
            }
        }

        public virtual void OnSaveCommand()
        {

        }

    }
}
