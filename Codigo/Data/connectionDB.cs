
using BC6200.Config;
using BC6200.Log;
using BC6200.Models;
using BC6200.Service;
using Npgsql;
using System.Data;


namespace BC6200.DbContext
{
    public class connectionDB
    {

      

       

        public string coneccion = InterfaceConfig.StrCadenaConeccion;
        public string nombreLog = InterfaceConfig.NombreLog;
        public bool ImprimirQuery = InterfaceConfig.ImprimirQuery;
        public int intentosReconexionDB = InterfaceConfig.IntentosReconexionDB; 
        public ResultadoQuery RunQuery(string sqlStr, List<NpgsqlParameter> ParametersList, CommandType type)
        {
            ResultadoQuery resultadoQuery = new ResultadoQuery();
            resultadoQuery.Tabla = new DataTable();
            resultadoQuery.Resultado = "";
            resultadoQuery.ResultadoMensaje = "";

            bool respuestaPersistencia = false;
            int intentosConexion = 0;

            while (!respuestaPersistencia)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(coneccion))
                {

                    intentosConexion++;

                    try
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connection.Open();
                        }
                        else
                        {
                            connection.Open();
                        }

                        using (NpgsqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = type;
                            command.CommandText = sqlStr;

                            if (ParametersList != null)
                            {
                                foreach (var parameter in ParametersList)
                                {
                                    command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
                                }
                            }

                            DataTable dt = new DataTable();
                            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(command))
                            {
                                da.Fill(dt);
                            }


                            connection.Close();

                            if (ImprimirQuery.Equals("S")) RegistroLog.Instance.RegistraEnLog($"Sentencia ejecutada[{sqlStr}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                            respuestaPersistencia = true;
                            resultadoQuery.Tabla = dt;

                        }

                    }
                    catch (Exception ex)
                    {
                        RegistroLog.Instance.RegistraEnLog("Error consultando la base de datos --> Mensaje[" + ex.Message + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        if (ImprimirQuery.Equals("S")) RegistroLog.Instance.RegistraEnLog($"Sentencia ejecutada --> [" + sqlStr + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        respuestaPersistencia = false;
                        resultadoQuery.Resultado = "ERROR";
                        resultadoQuery.ResultadoMensaje = ex.Message;
                        resultadoQuery.ResultadoMensaje = ex.Message;
                    }
                }

                if (!respuestaPersistencia && intentosConexion == intentosReconexionDB)
                {
                    RegistroLog.Instance.RegistraEnLog("Se finaliza con los reintentos...",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    respuestaPersistencia = true;
                }
            }

            return resultadoQuery;
        }

        public ResultadoStatement RunStatement(string sqlStr, List<NpgsqlParameter> ParametersList, CommandType type)
        {
            ResultadoStatement resultadoStatement = new ResultadoStatement();
            resultadoStatement.Resultado = "";
            resultadoStatement.ResultadoMensaje = "";
            bool respuestaPersistencia = false;
            int intentosConexion = 0;

            while (!respuestaPersistencia)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(coneccion))
                {
                    intentosConexion++;

                    try
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connection.Open();
                        }
                        else
                        {
                            connection.Open();
                        }

                        using (NpgsqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = type;
                            command.CommandText = sqlStr;

                            if (ParametersList != null)
                            {
                                foreach (var parameter in ParametersList)
                                {
                                    command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
                                }
                            }

                            resultadoStatement.FilasAfectadas = command.ExecuteNonQuery();
                        }

                        connection.Close();
                        respuestaPersistencia = true;
                        if (ImprimirQuery.Equals("S")) RegistroLog.Instance.RegistraEnLog($"Sentencia ejecutada[{sqlStr}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    }
                    catch (Exception ex)
                    {
                        RegistroLog.Instance.RegistraEnLog("Error ejecutando en la base de datos --> Mensaje[" + ex.Message + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        RegistroLog.Instance.RegistraEnLog("Sentencia ejecutada --> [" + sqlStr + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        respuestaPersistencia = false;
                        resultadoStatement.Resultado = "ERROR";
                        resultadoStatement.ResultadoMensaje = ex.Message;
                    }
                }
                if (!respuestaPersistencia && intentosConexion == intentosReconexionDB)
                {
                    RegistroLog.Instance.RegistraEnLog("Se finaliza con los reintentos...",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    respuestaPersistencia = true;
                }
            }

            return resultadoStatement;
        }

    }
}
