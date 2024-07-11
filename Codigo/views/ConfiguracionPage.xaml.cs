using BC6200.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BC6200.views
{
    /// <summary>
    /// Lógica de interacción para ConfiguracionPage.xaml
    /// </summary>
    public partial class ConfiguracionPage : Page
    {
        public ConfiguracionPage()
        {
            InitializeComponent();
            InitializeForms();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private void InitializeForms()
        {
            InterfaceConfig.InitializeConfig();
            Header.InputText = InterfaceConfig.Header;
            UserName.InputText = InterfaceConfig.UserName;
            Pass.InputText = InterfaceConfig.Pass;
            TokenURl.InputText = InterfaceConfig.UrlToken;
            ResultadoURl.InputText = InterfaceConfig.UrlRegistroResultados;
            RutaArchivo.InputText = InterfaceConfig.RutaArchivos;
            RutaArchivoError.InputText = InterfaceConfig.RutaArchivosError;
            RutaArchivoErrorOK.InputText = InterfaceConfig.RutaArchivosOK;
            IndentificadorEquipoUno.InputText = InterfaceConfig.IndetificadorEquipoUno;
            IndentificadorEquipoDos.InputText = InterfaceConfig.IdentificadorEquipoDos;
            NombreEquipo.InputText = InterfaceConfig.NombreEquipoUno;
            NombreEquipoRET.InputText = InterfaceConfig.NombreEquipoDos;
            IdentificadorMuestra.InputText = InterfaceConfig.IdentificadorMuestra;
            IdentificadorMuestraRET.InputText = InterfaceConfig.IdentificadorMuestraRET;
            IndentificadorNivel1.InputText = InterfaceConfig.IndentificadorNivel1;
            Indentificadornivel2.InputText = InterfaceConfig.IndentificadorNivel2;
            IndentificadorNivel3.InputText = InterfaceConfig.IndentificadorNivel3;
            //NombreLog.InputText = InterfaceConfig.NombreLog;
            RutaLog.InputText = InterfaceConfig.RutaLog;
            CaracteresLoteDisminuir.InputText = InterfaceConfig.CantidadCaracteresLoteDisminuir;
            CaracteresLoteDisminuirRET.InputText = InterfaceConfig.CantidadCaracteresLoteDisminuirRET;
            SetLogButtonTemplate();
        }
        private void SetLogButtonTemplate()
        {
            if (InterfaceConfig.ActivaLog)
            {
                BtnLog.Template = (ControlTemplate)FindResource("btnToggleLog");
            }
            else
            {
                BtnLog.Template = (ControlTemplate)FindResource("btnToggleLogFalse");
            }
        }

        private void OnGuardarButtonClick(object sender, RoutedEventArgs e)
        {
            // Update the configuration values from the form inputs
            InterfaceConfig.Header = Header.InputText;
            InterfaceConfig.UserName = UserName.InputText;
            InterfaceConfig.Pass = Pass.InputText;
            InterfaceConfig.UrlToken = TokenURl.InputText;
            InterfaceConfig.UrlRegistroResultados = ResultadoURl.InputText;
            InterfaceConfig.RutaArchivos = RutaArchivo.InputText;
            InterfaceConfig.RutaArchivosError = RutaArchivoError.InputText;
            InterfaceConfig.RutaArchivosOK = RutaArchivoErrorOK.InputText;


            InterfaceConfig.NombreEquipoUno = NombreEquipoRET.InputText;
            InterfaceConfig.NombreEquipoDos = NombreEquipo.InputText;
            InterfaceConfig.IndetificadorEquipoUno = IdentificadorMuestra.InputText;
            InterfaceConfig.IdentificadorEquipoDos = IdentificadorMuestraRET.InputText;
            InterfaceConfig.IdentificadorMuestra = IdentificadorMuestra.InputText;
            InterfaceConfig.IdentificadorMuestraRET = IdentificadorMuestraRET.InputText;
            InterfaceConfig.IndentificadorNivel1 = IndentificadorNivel1.InputText;
            InterfaceConfig.IndentificadorNivel2 = Indentificadornivel2.InputText;
            InterfaceConfig.IndentificadorNivel3 = IndentificadorNivel3.InputText;

            InterfaceConfig.RutaLog = RutaLog.InputText;
            InterfaceConfig.CantidadCaracteresLoteDisminuir = CaracteresLoteDisminuir.InputText;
            InterfaceConfig.CantidadCaracteresLoteDisminuirRET = CaracteresLoteDisminuirRET.InputText;

            // Save the updated values to the configuration file
            SaveConfiguration();
            
            SetLogButtonTemplate();
           
        }

        private void SaveConfiguration()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["Header"].Value = InterfaceConfig.Header;
            config.AppSettings.Settings["UserName"].Value = InterfaceConfig.UserName;
            config.AppSettings.Settings["Pass"].Value = InterfaceConfig.Pass;
            config.AppSettings.Settings["UrlToken"].Value = InterfaceConfig.UrlToken;
            config.AppSettings.Settings["UrlRegistroResultados"].Value = InterfaceConfig.UrlRegistroResultados;
            config.AppSettings.Settings["rutaArchivos"].Value = InterfaceConfig.RutaArchivos;
            config.AppSettings.Settings["rutaArchivosError"].Value = InterfaceConfig.RutaArchivosError;
            config.AppSettings.Settings["rutaArchivosOK"].Value = InterfaceConfig.RutaArchivosOK;
            config.AppSettings.Settings["NombreEquipoDos"].Value = InterfaceConfig.NombreEquipoDos;
            config.AppSettings.Settings["NombreEquipoUno"].Value = InterfaceConfig.NombreEquipoUno;
            config.AppSettings.Settings["IdentificadorMuestra"].Value = InterfaceConfig.IndetificadorEquipoUno;
            config.AppSettings.Settings["IdentificadorMuestraRET"].Value = InterfaceConfig.IdentificadorEquipoDos;
            config.AppSettings.Settings["IndentificadorNivel1"].Value = InterfaceConfig.IndentificadorNivel1;
            config.AppSettings.Settings["IndentificadorNivel2"].Value = InterfaceConfig.IndentificadorNivel2;
            config.AppSettings.Settings["IndentificadorNivel3"].Value = InterfaceConfig.IndentificadorNivel3;
            config.AppSettings.Settings["RutaLog"].Value = InterfaceConfig.RutaLog;
            config.AppSettings.Settings["CantidadCaracteresLoteDisminuir"].Value = InterfaceConfig.CantidadCaracteresLoteDisminuir;
            config.AppSettings.Settings["CantidadCaracteresLoteDisminuirRET"].Value = InterfaceConfig.CantidadCaracteresLoteDisminuirRET;
            config.AppSettings.Settings["activaLog"].Value = InterfaceConfig.ActivaLog ? "S":"N";

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            InterfaceConfig.InitializeConfig();
            MessageBox.Show("Configuración guardada exitosamente.");
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            if (InterfaceConfig.ActivaLog)
            {
                InterfaceConfig.ActivaLog = false;
            }
            else {
                InterfaceConfig.ActivaLog = true;
            }
            SetLogButtonTemplate();
        }
    }
}
