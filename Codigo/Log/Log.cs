using BC6200.Config;
using BC6200.Service;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading; // Asegúrate de tener esta referencia si es una aplicación WPF

namespace BC6200.Log
{
    public class RegistroLog
    {
        private static readonly Lazy<RegistroLog> instance = new Lazy<RegistroLog>(() => new RegistroLog());
        public static RegistroLog Instance => instance.Value;

        private bool logIniciado = false;
        private string logName = "Log_";
        private bool catchEjecutado = false;
        private static readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        private RegistroLog() { }

        public void InicializaLog(string p_equipo)
        {
            string RutaLog = InterfaceConfig.RutaLog;

            try
            {
                logName = Path.Combine(RutaLog, $"Log_{p_equipo}_v{Application.ResourceAssembly.GetName().Version}_{DateTime.Now:ddMMyyyy}");
                using (StreamWriter w = File.AppendText($"{logName}.txt"))
                {
                    w.WriteLine($"\r\nLog {p_equipo} v{Application.ResourceAssembly.GetName().Version} : ");
                    w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                    w.WriteLine("------------------------------------------------------------------------------------------------");
                }
                if (p_equipo != "Desconocido") logIniciado = true;
            }
            catch (Exception)
            {
                
            }
        }

        public void RegistraEnLog(string logMessage, string p_equipo, EnumEstados estado, bool proyectarEnInterfaz = true)
        {
            bool logActivo = InterfaceConfig.ActivaLog;
            string RutaLog = InterfaceConfig.RutaLog;

            try
            {
                if (logActivo)
                {
                    logName = Path.Combine(RutaLog, $"Log_{p_equipo}_v{Application.ResourceAssembly.GetName().Version}_{DateTime.Now:ddMMyyyy}");
                    if (!logIniciado) InicializaLog(p_equipo);
                    using (StreamWriter w = File.AppendText($"{logName}.txt"))
                    {
                        w.WriteLine($"{DateTime.Now}  :  {logMessage}");
                    }
                }
            }
            catch (Exception)
            {
               
            }

            if (proyectarEnInterfaz)
            {
                dispatcher.Invoke(() => MessageService.Instance.AddMessage(logMessage, estado));
            }
        }
    }

    public static class ILog
    {
        public static string RutaLog { get; set; } = InterfaceConfig.RutaLog;
        public static bool activaLog { get; set; }
    }
}
