using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace APreShutdownService
{
    public partial class PreShutdownService : ServiceBase
    {
        public const int SERVICE_ACCEPT_PRESHUTDOWN = 0x100;
        public const int SERVICE_CONTROL_PRESHUTDOWN = 0xf;
        private string logPath = @"C:\output.txt";

        public PreShutdownService()
        {
            CanHandleSessionChangeEvent = true;
            CanShutdown = true;
            CanStop = true;
            RegisterPreShutdownEvent();
            InitializeComponent();
        }

        private void RegisterPreShutdownEvent()
        {
            FieldInfo acceptedCommandsFieldInfo = typeof(ServiceBase).GetField("acceptedCommands", BindingFlags.Instance | BindingFlags.NonPublic);
            if (acceptedCommandsFieldInfo == null)
            {
                throw new Exception("acceptedCommands field not found");
            }
            int value = (int)acceptedCommandsFieldInfo.GetValue(this);
            acceptedCommandsFieldInfo.SetValue(this, value | SERVICE_ACCEPT_PRESHUTDOWN);
        }

        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);
            switch (command)
            {
                case SERVICE_CONTROL_PRESHUTDOWN:
                    OnPreShutdown();
                    break;

                default:
                    break;
            }
        }

        private void OnPreShutdown()
        {
            //this should be the preshutdown code
            File.AppendAllText(logPath, $"PreShutdown called at {DateTime.Now}\n");
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText(logPath, $"OnStart called at {DateTime.Now}\n");
        }

        protected override void OnStop()
        {
            File.AppendAllText(logPath, $"OnStop called at {DateTime.Now}\n");
        }
    }
}
