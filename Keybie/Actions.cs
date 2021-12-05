using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    interface IAction
    {
        void Execute();
    }

    class BaseAction
    {
        public string Name { get; set; }
        public string Trigger { get; set; }

        [JsonIgnore]
        public DateTime LastRun { get; set; } = DateTime.MinValue;

        public virtual void Execute() 
        {

        }
    }

    class RunProcessAction : BaseAction
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
        public bool IsRunAsAdministartor { get; internal set; }

        public override void Execute()
        {
            if (DateTime.Now - LastRun > TimeSpan.FromSeconds(2))
            {
                LastRun = DateTime.Now;
                try
                {
                    Process process = new Process();
                    // Configure the process using the StartInfo properties.
                    process.StartInfo.FileName = Path;
                    process.StartInfo.Arguments = Arguments;
                    process.Start();
                }
                catch { }
            }
           
        }
    }

    class SendKeysAction : BaseAction
    {
        public string Path { get; set; }
        public string Arguments { get; set; }

        public override void Execute()
        {
            try
            {
                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = Path;
                process.StartInfo.Arguments = Arguments;
                process.Start();
            }
            catch { }
        }
    }
}
