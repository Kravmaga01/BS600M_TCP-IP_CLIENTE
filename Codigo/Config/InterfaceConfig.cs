using System.Configuration;

namespace BC6200.Config
{
    public static class InterfaceConfig
    {
        // Configuración servidor
        internal static string? IpServidor;
        internal static string? PuertoServidor;

        // Configuración Interfaz
        internal static string? NombreEquipo;

        // Configuración Log
        internal static bool ActivaLog;
        internal static string? RutaLog;
        internal static string? NombreLog;

        // Configuración Conexión a Base de Datos
        internal static int IntentosReconexionDB;

        // Configuración parámetros QC
        internal static string? IndentificadorNivel1;
        internal static string? IndentificadorNivel2;
        internal static string? IndentificadorNivel3;

        // Nuevas variables
        internal static int? Ciclos;
        internal static int? Tiempo;
        internal static int? DiasAtras;
        internal static string? GrupoExamen;
        internal static bool ProgramarExamenesConResultados;
        internal static bool ImprimirQuery;
        internal static bool CaratulaPorEquipo;
        internal static string? EquipoCodigoCaratula;
        internal static string? ReactivoCodigoCaratula;
        internal static bool SobreEscribeResultado;
        internal static string? StrCadenaConeccion;

        internal static void InitializeConfig()
        {
            // Configuración servidor
            IpServidor = ConfigurationManager.AppSettings["IpServidor"];
            PuertoServidor = ConfigurationManager.AppSettings["PuertoServidor"];

            // Configuración Interfaz
            NombreEquipo = ConfigurationManager.AppSettings["NombreEquipo"];

            // Configuración Log
            ActivaLog = ConfigurationManager.AppSettings["ActivaLog"].ToString() == "S";
            RutaLog = ConfigurationManager.AppSettings["RutaLog"];
            NombreLog = ConfigurationManager.AppSettings["NombreLog"];

            // Configuración Conexión a Base de Datos
            IntentosReconexionDB = int.Parse(ConfigurationManager.AppSettings["intentosReconexionDB"]);

            // Configuración parámetros QC
            IndentificadorNivel1 = ConfigurationManager.AppSettings["IndentificadorNivel1"];
            IndentificadorNivel2 = ConfigurationManager.AppSettings["IndentificadorNivel2"];
            IndentificadorNivel3 = ConfigurationManager.AppSettings["IndentificadorNivel3"];

            // Inicialización de nuevas variables
            Ciclos = int.Parse(ConfigurationManager.AppSettings["ciclos"]);
            Tiempo = int.Parse(ConfigurationManager.AppSettings["tiempo"]);
            DiasAtras = int.Parse(ConfigurationManager.AppSettings["diasatras"]);
            GrupoExamen = ConfigurationManager.AppSettings["GrupoExamen"];
            ProgramarExamenesConResultados = ConfigurationManager.AppSettings["programarExamenesConResultados"].ToString() == "S";
            ImprimirQuery = ConfigurationManager.AppSettings["ImprimirQuery"].ToString() == "S";
            CaratulaPorEquipo = ConfigurationManager.AppSettings["caratulaPorEquipo"].ToString() == "S";
            EquipoCodigoCaratula = ConfigurationManager.AppSettings["equipoCodigoCaratula"];
            ReactivoCodigoCaratula = ConfigurationManager.AppSettings["reactivoCodigoCaratula"];
            SobreEscribeResultado = ConfigurationManager.AppSettings["SobreEscribeResultado"].ToString() == "S";
            StrCadenaConeccion = ConfigurationManager.AppSettings["StrCadenaConeccion"];
        }
    }
}
