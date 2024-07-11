using BC6200.Config;
using BC6200.Log;
using BC6200.Metodos;
using BC6200.Service;
using BC6200.SocketClient;
using BC6200.views;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace BC6200
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        public const char EOT = (char)4;
        public const char US = (char)31;
        public const char RS = (char)30;
        public const char ACK = (char)6;
        public const char NAK = (char)21;
        public const char ENQ = (char)5;
        public const char STX = (char)2;
        public const char ETX = (char)3;
        public const char LF = (char)10;
        public const char CR = (char)13;
        public const char VT = (char)11;
        public const char FS = (char)28;
        public string strRecibido;
        public string CharEnviado;
        public List<string> ArrPaquete = new List<string>();
        public string[] ArrPaquete1 = new string[900];
        public string StrLineaEstudio;
        public string consecutivoMsg;
        public string messageDateTime;
        public int inc;
        public int incEnv;
        public string vCodPaciente;
        public string strExamen = "";
        public string strregExa = "";
        public string strExamenCent = "";
        public int intCantidadRegistrosResulLab;
        public int intCantidadResultadosValidados;
        public bool tubo_ind;
        public string strLineaOrdenBS = "";
        public bool existePaciente;
        public bool RegistraEventoExamen;
        public string estudioAnterior = "X";
        public string EscribeResultado;
        public string ExamenHomologadoAnterior;
        public bool mostrarImagen = false;

        public int cantdigitos;
        public string NroGrupo;
        public string NroTapa;
        public string strEstFuncional = "";

        public string strLinea = "";

        public bool vContRegistraEvento;

        public string strTramaRecibida = "";
        public string strTramaAnterior = "";
        public int cantVt = 0;
        public int cantFs = 0;

        public int ciclosActuales = 0;
        public string strHORQUI;
        public string strSUFIJO;
        public string strSEDES;
        public string vCodigoSede = "-1";
        public string Unidades = "";

        public string[] strValoresLimites = new string[49];
        public string[] strValoresSerie = new string[249];
        public bool StartGraphics = false;

        public int Ancho = 128;
        public int Alto = 128;
        // Lineas Horizontales
        public int NL = 0;
        public int NE = 0;
        public int RMN = 0;
        public int NoL = 0;
        public int NoN = 0;
        public int NoE = 0;
        public int LN = 0;
        public int RN = 0;
        public int LL = 0;
        public int AL = 0;
        public int LMU = 0;
        public int LMD = 0;
        public int LMN = 0;
        public int MN = 0;
        public int RM = 0;

        SocketCliente socketClient = new SocketCliente();
        public MainWindow()
        {

            InitializeComponent();

            // Obtener la versión del ensamblado
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version.ToString();

            // Inicializar SocketClient y suscribir eventos

            socketClient.ConexionTerminada += SocketCliente_ConexionTerminada;
            socketClient.DatosRecibidos += SocketCliente_DatosRecibidos;

            // Configuración de la interfaz
            InterfaceConfig.InitializeConfig();
            DashBoradPanelFrame.NavigationService.Navigate(new CargueResultados());
            RegistroLog.Instance.RegistraEnLog("Interfaz iniciada", InterfaceConfig.NombreLog, EnumEstados.Ok);
            RegistroLog.Instance.RegistraEnLog("Precione Conectar para inciar con la lectura de archivos CSV", InterfaceConfig.NombreLog, EnumEstados.Info);
            TextoVersion.Content = $"V{version}";
            this.Title = $"{InterfaceConfig.NombreLog} v{version}";
        }



        private bool isConnected = false;
        private bool isResultado = true;
        private Thickness originalMargin;
        private Thread hilo;
        private bool detener;
        private CancellationTokenSource cancellationTokenSource;
        private Task procesoArchivosTask;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isResultado)
            {
                DashBoradPanelFrame.NavigationService.Navigate(new CargueResultados());
                isResultado = true;
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isResultado)
            {
                DashBoradPanelFrame.NavigationService.Navigate(new ConfiguracionPage());
                isResultado = false;
            }
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight >= this.MaxHeight && this.ActualWidth >= this.MaxWidth)
            {
                this.Padding = new Thickness(50);
            }
            else
            {
                this.Margin = new Thickness(0);
            }
        }

        private void ConectarButton_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        public void Connect()
        {
            BtnConfiguracion.IsEnabled = false;
            isConnected = true;
            AnimateButton(true);
            ConectarButton.Template = (ControlTemplate)FindResource("BtnDesconectar");

            if (!isResultado)
            {
                DashBoradPanelFrame.NavigationService.Navigate(new CargueResultados());
            }
            isResultado = true;

            RegistroLog.Instance.RegistraEnLog($"Inicia proceso de lectura de archivos", InterfaceConfig.NombreLog, EnumEstados.Ok);
            try
            {
                socketClient.IP = InterfaceConfig.IpServidor;
                socketClient.Puerto = int.Parse(InterfaceConfig.PuertoServidor);
                socketClient.Conectar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Disconnect()
        {
            isConnected = false;
            AnimateButton(false);
            ConectarButton.Template = (ControlTemplate)FindResource("BtnConectar");
            BtnConfiguracion.IsEnabled = true;

            RegistroLog.Instance.RegistraEnLog($"Proceso de lectura cancelado", InterfaceConfig.NombreLog, EnumEstados.Ok);

            // Cancelar el proceso de lectura de archivos

            socketClient.Desconectar();

        }



        private void SocketCliente_ConexionTerminada()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Conexión terminada.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void SocketCliente_DatosRecibidos(string datos)
        {
            try
            {
                RegistroLog.Instance.RegistraEnLog($"Paquete parcial recibido --> [{datos}]", InterfaceConfig.NombreLog, EnumEstados.Ok, false);

                // Agregar el dato recibido al buffer de trama recibida
                strTramaRecibida += datos;

                // Contar la cantidad de VT y FS en la trama recibida
                int countVT = strTramaRecibida.Count(c => c == VT);
                int countFS = strTramaRecibida.Count(c => c == FS);

                // Procesar mensajes solo si la cantidad de VT y FS son iguales
                if ((countVT == countFS) && (countFS != 0))
                {
                    // Registrar la trama recibida
                    RegistroLog.Instance.RegistraEnLog($"Paquete completo recibido --> [{strTramaRecibida.Replace('\0', ' ')}]", InterfaceConfig.NombreLog, EnumEstados.Ok);
                    countVT = 0;
                    countFS = 0;
                    strTramaRecibida = strTramaRecibida.Replace(VT.ToString(), "");
                    strTramaRecibida = strTramaRecibida.Trim('\0');
                    strTramaRecibida = strTramaRecibida.Trim();
                    strTramaRecibida = strTramaRecibida.Replace(LF.ToString(), "");

                    string[] arrTramaRecibida = strTramaRecibida.Split(FS);
                    string[] substrings = strTramaRecibida.Split(CR);

                    if (arrTramaRecibida.Length > 1)
                    {
                        strTramaAnterior = arrTramaRecibida[1].Trim('\0');
                    }

                    int x = 0;

                    for (int i = 0; i < substrings.Length; i++)
                    {
                        ArrPaquete.Add(substrings[i]);
                    }

                    Procesar.PorcesarResultados(ArrPaquete);

                    for (int i = 0; i < substrings.Length; i++)
                    {
                        ArrPaquete[i] = "";
                    }

                    strTramaAnterior = "";
                    strTramaRecibida = "";
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante el procesamiento
                RegistroLog.Instance.RegistraEnLog("Error en SocketCliente_DatosRecibidos: " + ex.Message, InterfaceConfig.NombreLog, EnumEstados.Error);
            }
        }

       




        private void AnimateButton(bool isConnecting)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = ConectarButton.Margin,
                To = isConnecting ? new Thickness(originalMargin.Left + 50, originalMargin.Top, originalMargin.Right, originalMargin.Bottom) : originalMargin,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            ConectarButton.BeginAnimation(Button.MarginProperty, animation);
        }
    }
}