using BC6200.Data;
using BC6200.Config;
using BC6200.Log;
using BC6200.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC6200;
using BC6200.DbContext;
using BC6200.Models;
using BC6200.Query;


namespace BC6200.Metodos
{
    public static class  Procesar
    {
        static ResultadoQuery resultadoQuery = new();
        static ResultadoStatement resultadoStatement = new();
        private static readonly BDquery dbQuery = new();
        public static string registraevento = "N";
        public static bool SobreEscribeResultado = InterfaceConfig.SobreEscribeResultado;
        public static string GrupoExamen = InterfaceConfig.GrupoExamen;

        public static bool caratulaPorEquipo = InterfaceConfig.CaratulaPorEquipo;

        public static void PorcesarResultados(List<string> PaqueteResultado)
        {
            try
            {
                
                string codigoPaciente = string.Empty;
                string analito = string.Empty;
                string resultado = string.Empty;
                string unidades = string.Empty;
                // No contiene el OBX se considera una trama de tipo control de calidad
                bool IQC = !PaqueteResultado.Any(linea => linea != null && linea.Contains("OBX"));

                if (IQC)
                {
                    // Haz algo si no se encuentra "OBX"
                    RegistroLog.Instance.RegistraEnLog("OBX no encontrado en el paquete de resultados. Se identifica que la trama pertenece a un control de calidad", InterfaceConfig.NombreLog, EnumEstados.Ok);
                }
                else
                {
                    // Haz algo si se encuentra "OBX"
                    RegistroLog.Instance.RegistraEnLog("Se encontró OBX en el paquete de resultados. Se identifica que la trama pertenece a diagnostico", InterfaceConfig.NombreLog, EnumEstados.Ok);
                }

                for (int x = 0; x < PaqueteResultado.Count; x++)
                {
                    if (!string.IsNullOrEmpty(PaqueteResultado[x]))
                    {
                        RegistroLog.Instance.RegistraEnLog("-->" + PaqueteResultado[x], InterfaceConfig.NombreLog, EnumEstados.Ok);
                        string strlinea = PaqueteResultado[x];
                        string[] arrLinea = strlinea.Split('|');
                        string strencabezado = arrLinea[0];

                        if (strencabezado == "MSH")
                        {
                            
                        }

                        if (strencabezado == "MSA")
                        {
                            
                        }

                        if (strencabezado == "PID")
                        {

                        }

                        if (strencabezado == "OBR")
                        {

                            codigoPaciente = arrLinea[2].ToString().Trim();
                            try
                            {
                                ////if (!ctroActivarValiQC.Equals("S") && IQC)
                                ////{
                                ////    RegistroLog.Instance.RegistraEnLog("Control ValiQC desactivado", InterfaceConfig.NombreLog, EnumEstados.Ok);
                                ////    return;
                                ////}
                                //else if (IQC)
                                //{
                                //    string strLote = arrLinea[14];
                                //    string strNivel = arrLinea[17];
                                //    string strFechaProcesamiento = arrLinea[6];
                                //    string strFechaVencimiento = arrLinea[15]; // fecha de vencimiento
                                //    string strExamen = arrLinea[3];
                                //    string strResult = arrLinea[20];

                                //    RegistroLog.Instance.RegistraEnLog("Inicia proceso Control de calidad ValiQC", InterfaceConfig.NombreLog, EnumEstados.Ok);

                                //    // AgregarResultQC(nbrMachineQC, strLoteR, strNivel, strFecha, strExamen, strResult, strFechaVencimiento);
                                //    //AgregarResultQC(nbrMachineQC, strLote, strNivel, strFechaProcesamiento, strExamen, strResult, strFechaVencimiento);
                                //    RegistroLog.Instance.RegistraEnLog("Proceso de control de calidad terminado", InterfaceConfig.NombreLog, EnumEstados.Ok);
                                //    continue;
                                //}
                            }
                            catch (Exception ex)
                            {
                                RegistroLog.Instance.RegistraEnLog("Error en estructura de la trama para control de calidad", InterfaceConfig.NombreLog, EnumEstados.Ok);
                                RegistroLog.Instance.RegistraEnLog("Valide si la trama pertenece a un control de calidad", InterfaceConfig.NombreLog, EnumEstados.Ok);
                                continue;
                            }

                          

                          
                        }

                        if (strencabezado == "OBX")
                        {
                            analito = arrLinea[4].ToString().Trim();
                            resultado = arrLinea[5].ToString().Trim();
                            unidades = arrLinea[6].ToString().Trim();
                            try
                            {
                                
                            }
                            catch (Exception ex)
                            {
                                RegistroLog.Instance.RegistraEnLog("Error Cargando Linea: " + strlinea, InterfaceConfig.NombreLog, EnumEstados.Ok);
                                continue;
                            }

                            // Aquí puedes registrar o procesar los datos según sea necesario

                            RegistraResultados(codigoPaciente, analito, resultado, unidades);
                        }

                        if (strencabezado == "QRD")
                        {
                            //try
                            //{
                            //    strOrdenResultado = arrLinea[8];
                            //}
                            //catch (Exception ex)
                            //{
                            //    RegistroLog.Instance.RegistraEnLog("Error Cargando Linea 2: " + strlinea, InterfaceConfig.NombreLog, EnumEstados.Ok);
                            //    continue;
                            //}

                            //RegistroLog.Instance.RegistraEnLog("Nro Tubo Query: " + strOrdenResultado, InterfaceConfig.NombreLog, EnumEstados.Ok);
                            ////QueryMuestra(strOrdenResultado.Trim());
                            ////RegistroLog.Instance.RegistraEnLog("Envio Paquete al BS--> " + strLineaOrdenBS, InterfaceConfig.NombreLog, EnumEstados.Ok);
                            ////socketServidor.enviarMensajeTodosClientes(strLineaOrdenBS);
                        }

                        if (strencabezado == "QRF")
                        {
                            // RegistroLog.Instance.RegistraEnLog("Paquete--> " + strLineaOrdenBS, InterfaceConfig.NombreLog, EnumEstados.Ok);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistroLog.Instance.RegistraEnLog("Se generó un error procesando la trama, verifique logs", InterfaceConfig.NombreLog, EnumEstados.Ok);
                RegistroLog.Instance.RegistraEnLog($"Error: [{ex}]", InterfaceConfig.NombreLog, EnumEstados.Ok);
            }
        }


        static private void RegistraResultados(string muestra, string variable, string resultado, string unidades)
        {
             RegistroLog.Instance.RegistraEnLog($"Número de tubo a procesar:[{muestra}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

            try
            {
                string[] arrNroPac2 = muestra.Split('-');

                bool estiquetaUnificada = false;
                string[] arrNroPac = muestra.Split('-');
                string StrNroTubo = arrNroPac[0];
                string arrNroPac0 = "";
                string arrNroPac1 = "";
                string strSeccion;
                string strTapa;
                string vPositividad;
                string v_sede_codigo = "";
                string v_tipoDoc = "";
                string v_Documento = "";
                string strPrefijo = "";
                string ConsecutivoCurva = "";
                string ExamenHomologado = "";
                int intCantidadRegistrosContestado = 0;
                int validarCondiciones = 0;
                strSeccion = "";
                strTapa = "";

                if (arrNroPac.Length >= 1)
                {
                    arrNroPac0 = arrNroPac[0];

                }

                if (arrNroPac.Length == 2)
                {
                    arrNroPac1 = arrNroPac[1];
                }

                if (arrNroPac.Length >= 1)
                {
                    arrNroPac0 = arrNroPac[0];

                }

                if (arrNroPac.Length == 2)
                {
                    arrNroPac1 = arrNroPac[1];
                }

                if (arrNroPac.Length > 1)
                {
                    strSeccion = arrNroPac[1].Substring(0, 1);
                    if (strSeccion == "0")
                    {
                        strSeccion = "";
                    }

                    try
                    {
                        strTapa = arrNroPac[1].Substring(1, 1); ;
                    }
                    catch
                    {
                        strTapa = "0";
                    }

                    if (strTapa == "0")
                    {
                        strTapa = "";
                    }
                }

                strPrefijo = arrNroPac[0].Substring(1, 1);
                if (StrNroTubo.Length == 10 || StrNroTubo.Length == 12)
                {
                    if (arrNroPac[0].Substring(0, 1).Contains("0"))
                    {
                        strPrefijo = arrNroPac[0].Substring(1, 1);
                        ConsecutivoCurva = arrNroPac[0].Substring(0, 1);
                    }
                    else
                    {
                        strPrefijo = arrNroPac[0].Substring(0, 1);
                        ConsecutivoCurva = arrNroPac[0].Substring(1, 1);
                    }
                }
                else if (StrNroTubo.Length == 9 || StrNroTubo.Length == 11)
                {
                    strPrefijo = arrNroPac[0].Substring(0, 1);
                }
                else if (StrNroTubo.Length == 8)
                {
                    strPrefijo = "0";
                }
                if (strPrefijo == "0")
                {
                    strPrefijo = "";
                }


                if (StrNroTubo.Length == 10)
                {
                    StrNroTubo = StrNroTubo.Substring(StrNroTubo.Length - 8);
                }
                else if (StrNroTubo.Length == 9)
                {
                    StrNroTubo = StrNroTubo.Substring(StrNroTubo.Length - 8);
                }

                string strHora = String.Format("{0:HH:mm:ss}", DateTime.Now);
                string dtFecha = DateTime.Now.Date.ToString("yyyy-MM-dd");





               bool  existePaciente = false;

                resultadoQuery = dbQuery.BuscarPaciente(StrNroTubo);
                if (resultadoQuery.Equals("ERROR"))
                {
                     RegistroLog.Instance.RegistraEnLog($"Error buscando Examenes Homologados, Paciente_cod = " + StrNroTubo + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    return;
                }

                if (resultadoQuery.Tabla.Rows.Count > 0)
                {

                    existePaciente = true;

                    v_sede_codigo = resultadoQuery.Tabla.Rows[0]["sede_codigo"].ToString();
                    v_tipoDoc = resultadoQuery.Tabla.Rows[0]["tipodcto_cod"].ToString();
                    v_Documento = resultadoQuery.Tabla.Rows[0]["nit"].ToString();


                     RegistroLog.Instance.RegistraEnLog($"Paciente encontrado:[{StrNroTubo}], Documento:[{v_Documento}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);


                }
                else
                {
                     RegistroLog.Instance.RegistraEnLog($"No se ha encontrado registro de paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    return;
                }
                vPositividad = "N";




                // *****************************************************************
                // ** Esta sección verifica la consulta de homologacion en detalle
                // *****************************************************************

                resultadoQuery = dbQuery.VerificarSoloHomologacion( StrNroTubo, variable);
                if (resultadoQuery.Equals("ERROR"))
                {
                     RegistroLog.Instance.RegistraEnLog($"Error buscando Examenes Homologados, Paciente_cod = " + StrNroTubo + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    return;
                }

                if (resultadoQuery.Tabla.Rows.Count > 0)
                {
                    ExamenHomologado = resultadoQuery.Tabla.Rows[0]["examen_cod"].ToString();
                    //   RegistroLog.Instance.RegistraEnLog($"Examen homologado:[{ExamenHomologado}]--> Correctamente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                }
                else
                {
                     RegistroLog.Instance.RegistraEnLog($"Este Examen:[{variable}] no está Homologado",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    return;
                }

                if (strPrefijo.Length >= 1)
                {

                    resultadoQuery = dbQuery.BuscarSufijo(strPrefijo, StrNroTubo, ExamenHomologado, arrNroPac, strSeccion, strTapa);
                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando prefijo:[" + strPrefijo + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }

                    if (resultadoQuery.Tabla.Rows.Count > 0)
                    {
                        // RegistroLog.Instance.RegistraEnLog($"Examen sufijo homologado:[{strPrefijo}]--> Correctamente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    }
                    else
                    {
                         RegistroLog.Instance.RegistraEnLog($"No se encuentra prefijo:[{strPrefijo}] para el  Examen:[{ExamenHomologado}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }

                }

                if (strSeccion == "E" && strTapa == "U")
                {
                     RegistroLog.Instance.RegistraEnLog($"Los resultados  pertenecen a etiquetas unificada ",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                    if (etiquetasUnificadas.ValidarEtiquetasInificadas(StrNroTubo, strSeccion, strTapa) == null)
                    {
                         RegistroLog.Instance.RegistraEnLog($"Los datos no cumplen las validaciones necesarias para insertar un resultado de etiquetas unificadas",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    }
                    estiquetaUnificada = true;
                }
                else
                {



                    if (strSeccion.Length >= 1 && strTapa.Length >= 1)
                    {

                        resultadoQuery = dbQuery.BuscarSeccion( StrNroTubo, ExamenHomologado,  strSeccion);
                        if (resultadoQuery.Equals("ERROR"))
                        {
                             RegistroLog.Instance.RegistraEnLog($"Error buscando sección:[" + strSeccion + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }

                        if (resultadoQuery.Tabla.Rows.Count > 0)
                        {
                            //    RegistroLog.Instance.RegistraEnLog($"Examen seccion homologado:[{strSeccion}]--> Correctamente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        }
                        else
                        {
                             RegistroLog.Instance.RegistraEnLog($"No se encuentra Sección:[{strSeccion}] para el Examen:[{ExamenHomologado}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }


                        resultadoQuery = dbQuery.BuscarTapa( StrNroTubo, ExamenHomologado,  strTapa);
                        if (resultadoQuery.Equals("ERROR"))
                        {
                             RegistroLog.Instance.RegistraEnLog($"Error buscando Tapa:[" + strSeccion + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }

                        if (resultadoQuery.Tabla.Rows.Count > 0)
                        {
                            // RegistroLog.Instance.RegistraEnLog($"Examen Tapa homologado:[{strTapa}]--> Correctamente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        }
                        else
                        {
                             RegistroLog.Instance.RegistraEnLog($"No se encuentra Tapa:[{strTapa}] para el Examen:[{ExamenHomologado}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }

                    }
                }

                if (!estiquetaUnificada)
                {
                    resultadoQuery = dbQuery.BuscarGrupoExamen(StrNroTubo, ExamenHomologado);
                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando GrupoExamen:[" + GrupoExamen + "]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }

                    if (resultadoQuery.Tabla.Rows.Count > 0)
                    {
                        // RegistroLog.Instance.RegistraEnLog($"Examen GrupoExamen homologado:[{GrupoExamen}]--> Correctamente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    }
                    else
                    {
                         RegistroLog.Instance.RegistraEnLog($"No se encuentra Grupo Examen:[{GrupoExamen}] para el Examen:[{ExamenHomologado}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }


                }



                // *****************************************************************
                // ** Aqui termina homologacion en detalle
                // *****************************************************************







                //Si existe paciente
                if (existePaciente)
                {
                    string Examenhomologado = null;
                    string analitohomologado = null;

                    string decimales = string.Empty;

                     RegistroLog.Instance.RegistraEnLog($"Inicia búsqueda de examen:[{variable}] Homologado, Paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    // Buscar examen homologado

                    if (estiquetaUnificada)
                    {
                        resultadoQuery = etiquetasUnificadas.ObtenerExamenes(StrNroTubo, strSeccion, strTapa, variable);
                    }
                    else
                    {
                        resultadoQuery = dbQuery.ConsultaHomologacionExamen(strPrefijo, StrNroTubo, variable, arrNroPac, strSeccion, strTapa);
                    }



                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando Examenes Homologados, Paciente_cod = " + StrNroTubo + " Examen = " + Examenhomologado + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }
                    if (resultadoQuery.Tabla.Rows.Count == 0)
                    {

                         RegistroLog.Instance.RegistraEnLog($"Homologación de examen:[{variable}]--->Sin resultados.",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        return;
                    }

                    foreach (DataRow reader1 in resultadoQuery.Tabla.Rows)
                    {
                        Examenhomologado = reader1["examen_cod"].ToString();
                        analitohomologado = reader1["analito"].ToString();
                        //reg_exam = reader1["reg_exa"].ToString();
                        decimales = reader1["decimales"].ToString();

                         RegistroLog.Instance.RegistraEnLog($"Homologación de examen:[{Examenhomologado}]---> Correctamente.",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        if (arrNroPac1 == "EU")
                        {
                             RegistroLog.Instance.RegistraEnLog($"Etiqueta Unificada Paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        }

                    }



                    resultado = obtenerDecimal(decimales, resultado, analitohomologado);

                     RegistroLog.Instance.RegistraEnLog($"Examen:[{Examenhomologado}]--> homologado, Analito:[{analitohomologado}]--> homologado",  InterfaceConfig.NombreLog, EnumEstados.Ok);



                     RegistroLog.Instance.RegistraEnLog($"Inicia búsqueda EXAMEN:[{Examenhomologado}], RESULTADO:[{resultado}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                    //Buscar en resul_lab si existe registro
                    resultadoQuery = dbQuery.BuscarResult_Lab(StrNroTubo, Examenhomologado);

                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando resul_lab, Paciente_cod = " + StrNroTubo + " Examen = " + Examenhomologado + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }




                    if (resultadoQuery.Tabla.Rows.Count == 0)
                    {
                         RegistroLog.Instance.RegistraEnLog($"Sin resultados, Paciente:[{StrNroTubo}], Examen:[{Examenhomologado}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        if (caratulaPorEquipo)
                        {

                            // Verifica si existe caratula por equipo
                            resultadoQuery = dbQuery.BuscarCaratulaEquipo(StrNroTubo, Examenhomologado);
                            if (resultadoQuery.Resultado.Equals("[ERROR]"))
                            {
                                 RegistroLog.Instance.RegistraEnLog($"Error verificando caractula por equipo, examen:[{Examenhomologado}], Paciente:[{StrNroTubo}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            }
                            if (resultadoQuery.Tabla.Rows.Count > 0)
                            {
                                 RegistroLog.Instance.RegistraEnLog($"Inicia Inserción Caratula Equipo, examen:[{Examenhomologado}] Homologado, Paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                                // Caratula por equipos
                                resultadoStatement = dbQuery.RegistroCaratulaEquipoResultados(StrNroTubo, Examenhomologado);
                                if (resultadoStatement.Resultado.Equals("[ERROR]"))
                                {
                                     RegistroLog.Instance.RegistraEnLog($"Error creando resul_lab, examen:[{Examenhomologado}], Paciente:[{StrNroTubo}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                                }
                                else
                                {
                                     RegistroLog.Instance.RegistraEnLog($"Inserción del examen:[{Examenhomologado}], Paciente:[{StrNroTubo}]--->Registrado exitosamente ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                                }

                            }
                            else
                            {

                                 RegistroLog.Instance.RegistraEnLog($"No se puede insertar resultado debido a que no se encontro caratula por equipo Caratula por equipo ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                                return;
                            }
                        }
                        else
                        {
                             RegistroLog.Instance.RegistraEnLog($"Inicia Inserción Caratula basica, examen:[{Examenhomologado}]--> Homologado, Paciente:[{StrNroTubo}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                            // Caratula basica
                            resultadoStatement = dbQuery.RegistroCaratulaBasica(StrNroTubo, Examenhomologado);
                            if (resultadoStatement.Resultado.Equals("[ERROR]"))
                            {
                                 RegistroLog.Instance.RegistraEnLog($"Error creando resul_lab, examen:[{Examenhomologado}], Paciente:[{StrNroTubo}]--->Registrado exitosamente ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            }
                            else
                            {
                                 RegistroLog.Instance.RegistraEnLog($"Inserción del examen:[{Examenhomologado}], Paciente:[{StrNroTubo}]--->Registrado exitosamente ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            }
                        }
                        resultadoStatement = dbQuery.InsertarAnalitosAdicionalesResulLab(StrNroTubo, Examenhomologado);
                        if (resultadoStatement.FilasAfectadas == 0)
                        {
                             RegistroLog.Instance.RegistraEnLog("No se inserto ningunos  analitos adicionales ",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        }
                        else if (resultadoStatement.FilasAfectadas == 0)
                        {
                             RegistroLog.Instance.RegistraEnLog("No se inserto ninguna analaito adicional a la caratula",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        }
                    }
                    else
                    {
                         RegistroLog.Instance.RegistraEnLog($"Resultado encontrado, Paciente_cod = [{StrNroTubo}] Examen = [{Examenhomologado}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                    }


                    // Verifica si hay resultado para esa variable
                     string EscribeResultado = "S";

                     RegistroLog.Instance.RegistraEnLog("Buscando resultados previos",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                    resultadoQuery = dbQuery.BuscarPacienteExamenesContestado(StrNroTubo, Examenhomologado);
                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando Pacientes_Examenes, Paciente_cod = " + StrNroTubo + " Examen = " + Examenhomologado + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }
                    else
                    {
                        intCantidadRegistrosContestado = int.Parse(resultadoQuery.Tabla.Rows[0]["count"].ToString());
                    }

                    //Buscando Resultado Pacientes_Examenes
                    resultadoQuery = dbQuery.BuscarResultPacientes_Examenes(StrNroTubo, Examenhomologado);

                    if (resultadoQuery.Equals("ERROR"))
                    {
                         RegistroLog.Instance.RegistraEnLog($"Error buscando Pacientes_Examenes, Paciente_cod = " + StrNroTubo + " Examen = " + Examenhomologado + "",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        return;
                    }



                    if (resultadoQuery.Tabla.Rows.Count > 0)
                    {
                         RegistroLog.Instance.RegistraEnLog("Se han encontrados Resultados Validados",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        EscribeResultado = "N";
                    }


                    if (SobreEscribeResultado || EscribeResultado == "N")
                    {
                        EscribeResultado = "N";
                    }
                    else if (SobreEscribeResultado  && EscribeResultado == "S")
                    {
                        EscribeResultado = "S";
                    }

                    if (resultadoQuery.Tabla.Rows.Count == 0 && intCantidadRegistrosContestado == 0)
                    {
                        EscribeResultado = "S";
                    }



                    // Guarda resultado en result_lab si esta permitido por EscribeResultado= "S"
                    if (EscribeResultado == "S")
                    {
                        resultadoQuery = dbQuery.ObtenerAnalitoResulLab(resultado, v_Documento, StrNroTubo, Examenhomologado, analitohomologado);
                        if (resultadoQuery.Tabla.Rows.Count > 0)
                        {
                            //Actualizar result_lab
                             RegistroLog.Instance.RegistraEnLog($"Inicia Actualización, examen:[{Examenhomologado}] Homologado, Paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            resultadoStatement = dbQuery.ActualizaResult_Lab(resultado, vPositividad, StrNroTubo, Examenhomologado, analitohomologado);
                            if (resultadoStatement.Equals("ERROR"))
                            {
                                 RegistroLog.Instance.RegistraEnLog("Error Actualizando resultado paciente",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                                return;
                            }
                        }
                        else
                        {
                            resultadoStatement = dbQuery.InsertarAnalitoResultado(StrNroTubo, Examenhomologado, analitohomologado, resultado);
                            if (resultadoStatement.Equals("ERROR"))
                            {
                                 RegistroLog.Instance.RegistraEnLog("Error insertando analito con resultadco ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                                return;

                            }
                        }



                         RegistroLog.Instance.RegistraEnLog($"Registrar Estado de examen:[{Examenhomologado}] Homologado, Paciente:[{StrNroTubo}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                        // Cambia estado a validado
                        resultadoQuery = dbQuery.ObtenerFechaHoraServidor();
                        if (resultadoQuery.Resultado.Equals("ERROR"))
                        {
                             RegistroLog.Instance.RegistraEnLog("Error: consultado la fecha y hora del servidor",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }
                        if (resultadoQuery.Tabla.Rows.Count == 0)
                        {
                             RegistroLog.Instance.RegistraEnLog("Error: No se pudo obtener fecha hora del servidor no se puede continuar con la inserción del evento",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }

                        string horaEvento = Convert.ToDateTime(resultadoQuery.Tabla.Rows[0]["server_time"].ToString()).ToString("HH:mm:ss");
                        string fechaEvento = Convert.ToDateTime(resultadoQuery.Tabla.Rows[0]["server_time"].ToString()).ToString("yyyy-MM-dd");
                        resultadoStatement = dbQuery.CambiarEstadoValidado(StrNroTubo, Examenhomologado, fechaEvento, horaEvento);

                        if (resultadoStatement.Equals("ERROR"))
                        {
                             RegistroLog.Instance.RegistraEnLog("Error Actualizando Estados de pacientes_examenes",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                            return;
                        }

                         RegistroLog.Instance.RegistraEnLog("Examen:[" + Examenhomologado + "] Paciente_cod: [" + StrNroTubo + "]--> Resultados Actualizados Exitosamente.   ",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                        if (registraevento.Equals("S"))
                        {
                            registraeventoResultado(StrNroTubo.Trim(), v_sede_codigo.Trim(), v_tipoDoc.Trim(), v_Documento.Trim(), Examenhomologado);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                 RegistroLog.Instance.RegistraEnLog($"Error Registrando resultados , mensaje[{ex.Message}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                return;
            }
        }
    }
}
