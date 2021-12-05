using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    class RunProcessActionVm : BaseActionVm
    {
        private string _Path;
        public string Path { get { return _Path; } set { SetProperty(ref _Path, value); } }

        private string _Arguments;
        public string Arguments { get { return _Arguments; } set { SetProperty(ref _Arguments, value); } }

        private bool _IsRunAsAdministrator;
        public bool IsRunAsAdministrator { get { return _IsRunAsAdministrator; } set { SetProperty(ref _IsRunAsAdministrator, value); } }

        public void ReadModel(RunProcessAction action)
        {
            TriggerCode = action.Trigger;
            Name = action.Name;
            Path = action.Path;
            Arguments = action.Arguments;
            IsRunAsAdministrator = action.IsRunAsAdministartor;
        }

        public RunProcessAction GetModel()
        {
            return new RunProcessAction()
            {
                Trigger = TriggerCode,
                Name = Name,
                Path = Path,
                Arguments = Arguments,
                IsRunAsAdministartor = IsRunAsAdministrator
            };
        }
    }
}
