
using BC6200.Config;
using BC6200.Data;
using BC6200.DbContext;
using BC6200.Log;
using BC6200.Models;
using BC6200.Service;
using System.Data;



namespace BC6200.Query

{
    public class BDquery
    {
        connectionDB _conexion = new connectionDB();
        
        //ResultadoQuery resultadoQuery = new ResultadoQuery();
        //ResultadoStatement resultadoStatement = new ResultadoStatement();
        public string diasatras = InterfaceConfig.DiasAtras.ToString();
        public string equipo = InterfaceConfig.NombreEquipo;
        public string GrupoExamen = InterfaceConfig.GrupoExamen;
        public bool programarExamenesConResultados = InterfaceConfig.ProgramarExamenesConResultados;
        public string nombreLog = InterfaceConfig.NombreLog;

        public bool verificaSufijo;

        ResultadoQuery resultadoQuery = new ResultadoQuery();

        public string strQuery = "";


        public ResultadoQuery ConsultaExamenesHomologados(string muestra, string strAnoToma)
        {
            ///Consulta Examenes homologados
            strQuery = "";
            strQuery = $@"SELECT DISTINCT pa.paciente_cod,pa.tipodcto_cod,pa.nit,pa.nacio, pa.nom1, pa.ape1,pa.sexo
                                                            FROM paciente pa
                                                            JOIN paciente_examenes pe on pe.paciente_cod = pa.paciente_cod  
                                                            AND pe.fecha = pa.fecha 
                                                            AND pe.hora = pa.hora
                                                            JOIN examenes ex on ex.examen_cod = pe.examen
                                                            JOIN homologacion ho on ho.equipo_cod = '{equipo}'
                                                            AND ho.examen_cod = pe.examen  
                                                            AND (ho.analito is null or trim(ho.analito) = '')
                                                            WHERE pe.validado = false AND pe.paciente_cod='{muestra}' AND pa.fecha >= current_date - {diasatras}
                                                            AND date_part('year',pa.fecha)= {strAnoToma}";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery BusquedaPacientes(string muestra)
        {
            ///BUSQUEDA DE PACIENTE EN ANNARLAB
            strQuery = "";
            strQuery = $@"SELECT DISTINCT pa.paciente_cod,pa.tipodcto_cod,pa.nit,pa.nacio, pa.nom1, pa.ape1,pa.sexo, pa.edad
                                                               FROM paciente pa
                                                               JOIN paciente_examenes pe ON pe.paciente_cod = pa.paciente_cod
                                                               AND pe.fecha = pa.fecha
                                                               AND pe.hora = pa.hora
                                                               JOIN examenes ex ON ex.gruexa_cod in ({GrupoExamen})   
                                                               WHERE pe.fecha >= CURRENT_DATE - {diasatras} AND pe.paciente_cod = '{muestra}'
                                                               GROUP BY pa.paciente_cod,pa.tipodcto_cod,pa.nit,pa.nacio, pa.nom1, pa.ape1,pa.sexo, pa.edad ORDER BY pa.paciente_cod LIMIT 1 ";



            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery ConsultaExamenes(string muestra, string strPrefijo, string strSeccion, string strTapa, string arrNroPac1)
        {
            ///BUSQUEDA DE ESTUDIOS DE PACIENTE       
            strQuery = "";





            if (strPrefijo.Length >= 1 && strPrefijo != "0")
            {
                strQuery = $@"SELECT paciente_cod, examenes.gruexa_cod, paciente_examenes.examen examen_annar, (paciente_examenes.fecha+paciente_examenes.hora) fecha, paciente_examenes.reg_exa reg_exa, examenes.tub_ind, examenes.prefijo
                                                                            FROM paciente_examenes 
                                                                            LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                            JOIN examenes ON examenes.examen_cod = paciente_examenes.examen 
                                                                            WHERE paciente_examenes.validado = False
                                                                            AND paciente_examenes.si_recsuero = True 
                                                                            AND examenes.prefijo ='{strPrefijo}' 
                                                                            AND paciente_examenes.paciente_cod ='{muestra}' 
                                                                            AND examenes.gruexa_cod  in ({GrupoExamen}) 
                                                                            AND paciente_examenes.fecha >= current_date - {diasatras}";
            }
            else if (strPrefijo == "" || strPrefijo == "0")
            {
                strQuery = $@"SELECT paciente_cod, examenes.gruexa_cod, paciente_examenes.examen examen_annar, (paciente_examenes.fecha+paciente_examenes.hora) fecha, paciente_examenes.reg_exa reg_exa, examenes.tub_ind, examenes.prefijo                                                                            FROM paciente_examenes 
                                                                            LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                            JOIN examenes ON examenes.examen_cod = paciente_examenes.examen 
                                                                            JOIN GRUPOS_EXAMENES G ON examenes.gruexa_cod =G.gruexa_cod 
                                                                            WHERE paciente_examenes.validado = False 
                                                                            AND paciente_examenes.si_recsuero = True 
                                                                            AND paciente_examenes.paciente_cod ='{muestra}' 
                                                                            AND examenes.gruexa_cod in ({GrupoExamen}) 
                                                                            AND paciente_examenes.fecha >= current_date - {diasatras} ";

                if (!string.IsNullOrEmpty(strSeccion))
                {
                    strQuery += " AND g.CCTO_CG1= '" + strSeccion + "' AND  examenes.TIPOENV_COD ='" + strTapa + "' ";
                }
                else if ((strPrefijo == "" || strPrefijo == "0") && string.IsNullOrEmpty(strSeccion))
                {
                    strQuery += " AND g.CCTO_CG1= ''";
                }
            }

            if (programarExamenesConResultados.Equals("S"))
            {
                strQuery += " AND (paciente_examenes.contestado = true OR paciente_examenes.contestado = false) ";
            }
            else
            {
                strQuery += " AND paciente_examenes.contestado = false ";
            }


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery Homologacion(string examen_annar)
        {
            ///BUSQUEDA DE ESTUDIOS DE PACIENTE
            strQuery = "";
            strQuery = $@"SELECT examen_cod_equipo FROM homologacion
                                                                       WHERE equipo_cod = '{equipo}' 
                                                                       AND   examen_cod = '{examen_annar}' 
                                                                       AND   (analito is null or trim(analito) = '')";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement UpdateEnviadoInterfaz(string vCodPaciente, string strExamen, string strregExa)
        {
            ///BUSQUEDA DE ESTUDIOS DE PACIENTE
            strQuery = "";
            strQuery = $"update paciente_examenes set enviado_interfaz = 'S' where paciente_cod = '" + vCodPaciente + "' and examen in('" + strExamen + "') and reg_exa  in(" + strregExa + ")";

            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery EventoResultPaciente(string vCodPaciente, string strExamen)
        {
            //evento resultado paciente
            strQuery = "";
            strQuery = $@"SELECT reg_exa,fecha,hora FROM eventos_paciente_exam 
 
                                                                 WHERE paciente_cod = '{vCodPaciente}' 
                                                                   AND fecha >= current_date - {diasatras}
                                                                   AND tipo_even_cod='020' 
                                                                   AND examen =  '{strExamen}'";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement DeleteEventosPacientes(string vCodPaciente, string strExamen)
        {

            //Eliminar eventos pacientes
            strQuery = "";
            strQuery = $@"DELETE from eventos_paciente_exam WHERE paciente_cod ='{vCodPaciente.Trim()}' AND fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                                                                        AND tipo_even_cod = '020' AND examen ='{strExamen}'";

            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        }
        public ResultadoQuery ConsultEventosPacientes(string vCodPaciente, string strExamen)
        {
            //Eliminar eventos pacientes
            strQuery = "";
            strQuery = $"SELECT * FROM eventos_paciente_exam where paciente_cod ='" + vCodPaciente.Trim() +
                       "' AND fecha >= current_date - " + diasatras + "  and examen ='" + strExamen + "'";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement InsertEventosPacienteExam(string codigopaciente, string varHora, string varFecha, string cod_sede, string numdoc, string tipdoc, string examen, string regexa, string dtFecha, string strHora)
        {
            //Insertar eventos pacientes
            strQuery = "";
            strQuery = $@"INSERT INTO eventos_paciente_exam(paciente_cod, hora, fecha, sede_codigo, historia,
                                                                                 tipodcto_cod,nit, examen, reg_exa, tipo_even_cod, 
                                                                                 fecha_event, hora_event, observ_event, usr_codigo, activo, secuencia)
                                                                                 VALUES('{codigopaciente}', '{varHora} ', '{varFecha}', '{cod_sede}', '{numdoc}', '{tipdoc}', 
                                                                                         '{numdoc}','{examen}', {regexa}, '020', '{dtFecha}', '{strHora}',
                                                                                         'ADICIONO RESULTADO POR {equipo}', 'INTERFAZ', true, 1)";

            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery BuscarPaciente(string StrNroTubo)
        {
            //Buscar paciente
            strQuery = "";
            strQuery = $@"SELECT tipodcto_cod,nit,sede_codigo 
                                                                      FROM paciente WHERE paciente_cod = '{StrNroTubo}'
                                                                       AND fecha >= current_date- {diasatras} ";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery VerificarSoloHomologacion( string strNroTubo, string variable)
        {
            //verificar homologacion
            strQuery = "";
            strQuery = $@"	SELECT homologacion.examen_cod,examen_cod_equipo
                                                                FROM homologacion
                                                                WHERE 
                                                                    equipo_cod = '{equipo}'
                                                                    AND examen_cod_equipo = '{variable.Trim()}'
                                                                    AND analito > ''
                                                                    AND examen_cod IN (
                                                                        SELECT 
                                                                            CASE 
                                                                                WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                                THEN examen 
                                                                                ELSE perfil_etiquetas.examen_cod 
                                                                            END AS examen 
                                                                        FROM 
                                                                            paciente_examenes
                                                                            LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                            JOIN examenes ON examenes.examen_cod = CASE 
                                                                                                                        WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                                                                        THEN examen 
                                                                                                                        ELSE perfil_etiquetas.examen_cod 
                                                                                                                    END 
                                                                        WHERE 
                                                                            paciente_examenes.validado = FALSE 
                                                                            AND paciente_examenes.si_recsuero = TRUE 
                                                                            AND paciente_examenes.paciente_cod = '{strNroTubo.Trim()}'
                                                                            AND paciente_examenes.fecha >= current_date - {diasatras})";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery BuscarSufijo(string vPrefijo, string strNroTubo, string variable, string[] arrNroPac, string strSeccion, string strTapa)
        {
            //Buscar Sufijo
            strQuery = "";
            strQuery = $@"	SELECT DISTINCT *
                                                        FROM examenes ex
                                                        WHERE 
                                                            ex.prefijo IN ('{vPrefijo}')
                                                            AND ex.examen_cod IN (
                                                                SELECT 
                                                                    CASE 
                                                                        WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                        THEN examen 
                                                                        ELSE perfil_etiquetas.examen_cod 
                                                                    END AS examen_cod
                                                                FROM 
                                                                    paciente_examenes
                                                                    LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                    JOIN examenes ex2 ON 
                                                                        (perfil_etiquetas.examen_cod IS NULL AND ex2.examen_cod = paciente_examenes.examen) OR 
                                                                        (perfil_etiquetas.examen_cod = ex2.examen_cod)
                                                                WHERE 
                                                                    paciente_examenes.validado = FALSE 
                                                                    AND paciente_examenes.si_recsuero = TRUE 
                                                                    AND paciente_examenes.paciente_cod = '{strNroTubo.Trim()}'
                                                                    AND paciente_examenes.fecha >= current_date - {diasatras}
                                                                    AND ex2.examen_cod = '{variable.Trim()}')";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }

        public ResultadoQuery BuscarSeccion( string strNroTubo, string variable, string strSeccion)
        {
            //Buscar Seccion
            strQuery = "";
            strQuery = $@"SELECT DISTINCT ex.*      FROM examenes ex
                                                            JOIN homologacion h ON ex.examen_cod = h.examen_cod
                                                            JOIN grupos_examenes g ON g.gruexa_cod = ex.gruexa_cod
                                                            WHERE 
                                                                h.examen_cod IN (
                                                                    SELECT 
                                                                        CASE 
                                                                            WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                            THEN examen 
                                                                            ELSE perfil_etiquetas.examen_cod 
                                                                        END AS examen 
                                                                    FROM 
                                                                        paciente_examenes
                                                                        LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                        JOIN examenes ON examenes.examen_cod = CASE 
                                                                                                                    WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                                                                    THEN examen 
                                                                                                                    ELSE perfil_etiquetas.examen_cod 
                                                                                                                END 
                                                                    WHERE 
                                                                        paciente_examenes.validado = FALSE 
                                                                        AND paciente_examenes.si_recsuero = TRUE 
	     	                                                            AND paciente_examenes.examen = '{variable}'
                                                                        AND paciente_examenes.paciente_cod = '{strNroTubo}'
                                                                        AND paciente_examenes.fecha >= current_date - {diasatras}
                                                                )
                                                                AND g.ccto_cg1 = '{strSeccion}'";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }

        public ResultadoQuery BuscarTapa(string strNroTubo, string variable,  string strTapa)
        {
            //Buscar Tapa
            strQuery = "";
            strQuery = $@"SELECT DISTINCT ex.*  FROM examenes ex
                                                    JOIN homologacion h ON ex.examen_cod = h.examen_cod
                                                    WHERE 
                                                        h.examen_cod IN (
                                                            SELECT 
                                                                CASE 
                                                                    WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                    THEN examen 
                                                                    ELSE perfil_etiquetas.examen_cod 
                                                                END AS examen 
                                                            FROM 
                                                                paciente_examenes
                                                                LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                JOIN examenes ON examenes.examen_cod = CASE 
                                                                                                            WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                                                            THEN examen 
                                                                                                            ELSE perfil_etiquetas.examen_cod 
                                                                                                        END 
                                                            WHERE 
                                                                paciente_examenes.validado = FALSE 
                                                                AND paciente_examenes.si_recsuero = TRUE 
                                                                AND paciente_examenes.paciente_cod = '{strNroTubo}'
                                                                AND paciente_examenes.fecha >= current_date - {diasatras}
                                                        )
                                                        AND ex.TIPOENV_COD ='{strTapa}'
	                                                    AND ex.examen_cod ='{variable}'";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }


        public ResultadoQuery BuscarGrupoExamen(string strNroTubo, string variable)
        {
            //Buscar Tapa
            strQuery = "";
            strQuery = $@"SELECT DISTINCT ex.*  FROM examenes ex
                                                        JOIN homologacion h ON ex.examen_cod = h.examen_cod
                                                        WHERE 
                                                            h.examen_cod IN (
                                                                SELECT 
                                                                    CASE 
                                                                        WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                        THEN examen 
                                                                        ELSE perfil_etiquetas.examen_cod 
                                                                    END AS examen 
                                                                FROM 
                                                                    paciente_examenes
                                                                    LEFT JOIN perfil_etiquetas ON perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                                                                    JOIN examenes ON examenes.examen_cod = CASE 
                                                                                                                WHEN perfil_etiquetas.examen_cod IS NULL 
                                                                                                                THEN examen 
                                                                                                                ELSE perfil_etiquetas.examen_cod 
                                                                                                            END 
                                                                WHERE 
                                                                    paciente_examenes.validado = FALSE 
                                                                    AND paciente_examenes.si_recsuero = TRUE 
                                                                    AND paciente_examenes.paciente_cod = '{strNroTubo}'
                                                                    AND paciente_examenes.fecha >= current_date - {diasatras}
                                                            )
                                                            AND ex.gruexa_cod IN ({GrupoExamen})
	                                                        AND ex.examen_cod ='{variable}'";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }

        public ResultadoQuery ConsultaHomologacionExamen(string vPrefijo, string strNroTubo, string variable,  string[] arrNroPac, string strSeccion, string strTapa)
        {
            var query = "";



            query = $@"select h.*, ex.etiquetas
                             from homologacion h
                             join examenes ex on ex.examen_cod = h.examen_cod
								 and ex.prefijo in('{vPrefijo}')
							join grupos_examenes g on g.gruexa_cod = ex.gruexa_cod
                            where h.examen_cod In (select case when perfil_etiquetas.examen_cod Is null then examen else perfil_etiquetas.examen_cod end examen 
                             from paciente_examenes
                        left Join perfil_etiquetas on perfil_etiquetas.perfil_cod = paciente_examenes.examen 
                             join examenes on examenes.examen_cod =  CASE WHEN perfil_etiquetas.examen_cod Is null then examen else perfil_etiquetas.examen_cod end 
                            where paciente_examenes.validado = False
                              AND h.analito > ''
                              And paciente_examenes.si_recsuero = True 
                              AND examenes.gruexa_cod in({GrupoExamen})
                              And paciente_examenes.paciente_cod ='{strNroTubo.Trim()}'
                              and paciente_examenes.fecha >= current_date - {diasatras})
                              and h.equipo_cod = '{equipo}' 
                              and h.examen_cod_equipo = '{variable.Trim()}'";

            if (arrNroPac.Length > 1)
            {
                string respuesta = strSeccion + strTapa;
                if (respuesta.Contains("EU"))
                {
                    RegistroLog.Instance.RegistraEnLog($"Etiqueta unificada: [{strSeccion + strTapa}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                }
                else
                {


                    //RegistroLog.Instance.RegistraEnLog($"Etiqueta [{strSeccion + strTapa}] no corresponde a la etiqueta Unificada", InterfaceConfig. InterfaceConfig.NombreLog, EnumEstados.Ok);
                    if (string.IsNullOrEmpty(strSeccion)) query = query + " and (g.ccto_cg1 is null or trim(g.ccto_cg1) = '')";
                    else query = query + " and g.ccto_cg1='" + strSeccion + "'";

                    if (string.IsNullOrEmpty(strTapa)) query = query + " and (ex.tipoenv_cod is null or trim(ex.tipoenv_cod) = '')";
                    else query = query + " and ex.tipoenv_cod='" + strTapa + "'";


                    RegistroLog.Instance.RegistraEnLog($"Etiqueta unificada: [{strSeccion + strTapa}] ",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                }

            }

            return _conexion.RunQuery(query, null, CommandType.Text);
        }



        public ResultadoQuery ConsultaHomologacionExamen(string strPrefijo, string StrNroTubo, string variable, string strTipoResultado, string strSeccion, string strTapa, string arrNroPac, string arrNroPac1)
        {
            // Buscar examen homologado

            strQuery = "";


            if (strPrefijo.Length >= 1 && strPrefijo != "0")
            {
                strQuery = $@"SELECT h.examen_cod ,h.analito,p.reg_exa,h.decimales
                                                                      FROM examenes e
                                                                      JOIN grupos_examenes g ON e.gruexa_cod = g.gruexa_cod
                                                                      JOIN homologacion h ON h.examen_cod = e.examen_cod
                                                                        AND h.equipo_cod = '{equipo}'
                                                                        AND h.analito > ''
                                                                      JOIN paciente_examenes p ON p.examen = h.examen_cod
                                                                      WHERE h.examen_cod_equipo = '{variable}' 
                                                                      AND e.gruexa_cod IN ({GrupoExamen}) 
                                                                      AND e.prefijo ='{strPrefijo}'
                                                                      AND p.fecha >= current_date- {diasatras}
                                                                      AND p.paciente_cod = '{StrNroTubo}'";

                VariablesGlobal.Prefijo = true;

            }
            else if (strPrefijo == "" || strPrefijo == "0")
            {

                strQuery = $@"SELECT h.examen_cod ,h.analito,p.reg_exa,h.decimales
                                                                      FROM examenes e
                                                                      JOIN grupos_examenes g ON e.gruexa_cod = g.gruexa_cod
                                                                      JOIN homologacion h ON h.examen_cod = e.examen_cod
                                                                        AND h.equipo_cod = '{equipo}'
                                                                        AND h.analito > ''
                                                                      JOIN paciente_examenes p ON p.examen = h.examen_cod
                                                                      WHERE h.examen_cod_equipo = '{variable}'
                                                                      AND e.gruexa_cod IN({GrupoExamen}) 
                                                                      AND p.fecha >= current_date - {diasatras}
                                                                      AND p.paciente_cod = '{StrNroTubo}'";


            }

            if (arrNroPac1.Length == 2)
            {
                if (string.IsNullOrEmpty(strSeccion))
                {
                    strQuery += " AND (g.ccto_cg1 is null or trim(g.ccto_cg1) = '')";
                }
                else
                {
                    strQuery += " AND g.ccto_cg1='" + strSeccion + "'";
                }

                if (string.IsNullOrEmpty(strTapa))
                {
                    strQuery += " AND (e.tipoenv_cod is null or trim(e.tipoenv_cod) = '')";
                }
                else
                {
                    strQuery += " AND e.tipoenv_cod='" + strTapa + "'";
                }


                resultadoQuery = _conexion.RunQuery(strQuery, null, CommandType.Text);

                if (resultadoQuery.Tabla.Rows.Count == 0)
                {
                    VariablesGlobal.False = true;

                }


            }
            else
            {
                string respuesta = strSeccion + strTapa;

                if (respuesta == "")
                {

                }
                else
                {
                    if (!respuesta.Contains("EU"))
                        RegistroLog.Instance.RegistraEnLog($"Etiqueta [{strSeccion + strTapa}] no corresponde a la etiqueta Unificada",  InterfaceConfig.NombreLog, EnumEstados.Ok);
                }

            }


            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery BuscarResult_Lab(string StrNroTubo, string Examenhomologado)
        {
            //Buscar en resul_lab si existe registro
            strQuery = "";
            strQuery = $@"SELECT * FROM resul_lab 
                                                                    WHERE paciente_cod = '{StrNroTubo}' 
                                                                    AND examen_cod='{Examenhomologado}' 
                                                                    AND fecha >= current_date- {diasatras}";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery BuscarResult_LabExamen(string StrNroTubo, string Examenhomologado, string v_Documento)
        {
            //Buscar en resul_lab si existe registro
            strQuery = "";
            strQuery = $@"SELECT *  FROM resul_lab 
                                                                    WHERE paciente_cod = '{StrNroTubo}' 
                                                                    AND examen_cod='{Examenhomologado}' 
                                                                    AND historia='{v_Documento}'
                                                                    AND fecha >= current_date- {InterfaceConfig.DiasAtras}";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement RegistroCaratulaBasica(string StrNroTubo, string Examenhomologado)
        {
            //Insertar Resul_Lab Registro
            strQuery = "";


            strQuery = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa, 
                                                    posiexa,  secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula) 
                                                    select paciente_examenes.paciente_cod,paciente_examenes.hora,paciente_examenes.fecha,paciente_examenes.sede_codigo,
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen, 
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratula_basica.examen_cod,caratula_basica.analito_cod,
                                                    caratula_basica.activo,caratula_basica.nombre,caratula_basica.unid,caratula_basica.tablav, caratula_basica.minimo,
                                                    caratula_basica.intermedio,caratula_basica.maximo,'_',caratula_basica.equipo_cod, caratula_basica.reactivo_cod,
                                                    caratula_basica.tablaa,caratula_basica.tabla1,caratula_basica.tabla2,caratula_basica.resequi,caratula_basica.resequi 
                                               from paciente_examenes 
                                               join caratula_basica on caratula_basica.examen_cod = paciente_examenes.examen 
                                              where fecha >= current_date - {diasatras} 
                                                and paciente_examenes.paciente_cod = '{StrNroTubo.Trim()}' 
                                                and paciente_examenes.examen='{Examenhomologado.Trim()}'
												and caratula_basica.unid in ('Q','F','V')";


            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        }

        public ResultadoQuery ConsultaResulLabCaratula(string StrNroTubo, string Examenhomologado)
        {
            //Buscar en resul_lab si existe registro
            strQuery = "";
            strQuery = $@"SELECT * FROM paciente_examenes
                                                                       JOIN caratula_basica ON caratula_basica.examen_cod = paciente_examenes.examen
                                                                      WHERE fecha >= current_date - {diasatras}
                                                                        AND paciente_examenes.paciente_cod = '{StrNroTubo}'
                                                                        AND paciente_examenes.examen='{Examenhomologado}'";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }
        public ResultadoQuery BuscarPacienteExamenesContestado(string StrNroTubo, string Examenhomologado)
        {
            strQuery = "";
            strQuery = $@"SELECT count(*)   FROM paciente_examenes
                                                                                    WHERE paciente_cod = '{StrNroTubo}'
                                                                                    AND examen = '{Examenhomologado}'
                                                                                    AND contestado = True
                                                                                    AND fecha >= current_date - {diasatras} ";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }
        public ResultadoQuery BuscarResultPacientes_Examenes(string StrNroTubo, string Examenhomologado)
        {
            //Buscando Resultado Pacientes_Examenes
            strQuery = "";
            strQuery = $@"SELECT * FROM paciente_examenes WHERE paciente_cod = '{StrNroTubo}' 
                                                                   AND examen='{Examenhomologado}' 
                                                                   AND validado = True 
                                                                   AND fecha >= current_date-{diasatras}";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }
        public ResultadoQuery BuscarCaratulaEquipo(string StrNroTubo, string Examenhomologado)
        {
            //Buscando Resultado Pacientes_Examenes
            strQuery = "";
            strQuery = $@"SELECT paciente_examenes.paciente_cod, paciente_examenes.hora, paciente_examenes.fecha, paciente_examenes.sede_codigo, 
                                                             paciente_examenes.historia, paciente_examenes.tipodcto_cod, paciente_examenes.nit, paciente_examenes.examen,
                                                             paciente_examenes.reg_exa, paciente_examenes.reg_exa, '1', 1, caratulas_equipos.examen_cod, caratulas_equipos.codili,
                                                             caratulas_equipos.activo, caratulas_equipos.nombre, caratulas_equipos.unid, caratulas_equipos.tablav, caratulas_equipos.minimo,
                                                             caratulas_equipos.intermedio, caratulas_equipos.maximo, ' ', caratulas_equipos.equipo_cod, caratulas_equipos.reactivo_cod,
                                                             caratulas_equipos.tablaa, caratulas_equipos.tabla1, caratulas_equipos.tabla2, caratulas_equipos.resequi, caratulas_equipos.resequi
                                                      FROM paciente_examenes
                                                      JOIN caratulas_equipos ON caratulas_equipos.examen_cod = paciente_examenes.examen
                                                      WHERE paciente_examenes.fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                        AND paciente_examenes.paciente_cod = '{StrNroTubo.Trim()}' 
                                                        AND paciente_examenes.examen = '{Examenhomologado.Trim()}' 
                                                        AND caratulas_equipos.equipo_cod = '{InterfaceConfig.EquipoCodigoCaratula}' 
                                                        AND reactivo_cod = '{InterfaceConfig.ReactivoCodigoCaratula}'";

            return _conexion.RunQuery(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement RegistroCaratulaEquipoResultados(string strNroTubo, string examenHomologado)
        {


            string query = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa,
                                                    posiexa, secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula)
					                                select paciente_examenes.paciente_cod,paciente_examenes.hora, paciente_examenes.fecha, paciente_examenes.sede_codigo, 
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen,
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratulas_equipos.examen_cod,caratulas_equipos.codili,
                                                    caratulas_equipos.activo, caratulas_equipos.nombre, caratulas_equipos.unid,caratulas_equipos.tablav, caratulas_equipos.minimo,
                                                    caratulas_equipos.intermedio,caratulas_equipos.maximo,'_',caratulas_equipos.equipo_cod, caratulas_equipos.reactivo_cod,
                                                    caratulas_equipos.tablaa,caratulas_equipos.tabla1,caratulas_equipos.tabla2,caratulas_equipos.resequi,caratulas_equipos.resequi
                                               from paciente_examenes
                                               join caratulas_equipos on caratulas_equipos.examen_cod = paciente_examenes.examen
                                              where paciente_examenes.fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                and paciente_examenes.paciente_cod = '{strNroTubo.Trim()}' 
                                                and paciente_examenes.examen = '{examenHomologado.Trim()}' 
                                                and caratulas_equipos.equipo_cod = '{InterfaceConfig.EquipoCodigoCaratula}' 
                                                and reactivo_cod = '{InterfaceConfig.ReactivoCodigoCaratula}'
                                                and caratulas_equipos.unid in ('Q','F','V')";


            return _conexion.RunStatement(query, null, CommandType.Text);
        }


        public ResultadoStatement ActualizaResult_Lab(string resultado, string vPositividad, string StrNroTubo, string Examenhomologado, string analitohomologado)
        {
            //Actualizar result_lab
            strQuery = "";
            strQuery = $@"UPDATE resul_lab 
                                                          SET resultado = '{resultado}'
                                                                , tabla2 = '{vPositividad}'
                                                                 WHERE paciente_cod = '{StrNroTubo}'
                                                                   AND fecha >= current_date- {diasatras}  
                                                                   AND examen_cod='{Examenhomologado}' 
                                                                   AND analito_cod='{analitohomologado}'";

            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        }

        public ResultadoStatement CambiarEstadoValidado(string StrNroTubo, string Examenhomologado,string FechaResultado,string horaResultado)
        {
            // Cambia estado a validado
            strQuery = "";
            strQuery =$@"UPDATE paciente_examenes
                              SET contestado = TRUE,
                                  modificado = TRUE,
                                  respondido_por = 'INTERFAZ',
                                  fec_res = '{FechaResultado}',
                                  hora_resp = '{horaResultado}',
                                  fec_mod = CURRENT_DATE
                              WHERE paciente_cod = '{StrNroTubo}'
                                AND examen = '{Examenhomologado}'
                                AND fecha >= CURRENT_DATE - {diasatras}
            ";

            return _conexion.RunStatement(strQuery, null, CommandType.Text);

        

        }

        public ResultadoStatement InsertarAnalitosAdicionalesResulLab(string strNroTubo, string examenhomologado)
        {
            string query = string.Empty;

            if (InterfaceConfig.CaratulaPorEquipo)
            {
                RegistroLog.Instance.RegistraEnLog($"Registra caratula equipo[{InterfaceConfig.EquipoCodigoCaratula}], reactivo[{InterfaceConfig.ReactivoCodigoCaratula}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                query = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa,
                                                    posiexa, secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula)
					                                select paciente_examenes.paciente_cod,paciente_examenes.hora, paciente_examenes.fecha, paciente_examenes.sede_codigo, 
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen,
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratulas_equipos.examen_cod,caratulas_equipos.codili,
                                                    caratulas_equipos.activo, caratulas_equipos.nombre, caratulas_equipos.unid,caratulas_equipos.tablav, caratulas_equipos.minimo,
                                                    caratulas_equipos.intermedio,caratulas_equipos.maximo,'_',caratulas_equipos.equipo_cod, caratulas_equipos.reactivo_cod,
                                                    caratulas_equipos.tablaa,caratulas_equipos.tabla1,caratulas_equipos.tabla2,caratulas_equipos.resequi,caratulas_equipos.resequi
                                               from paciente_examenes
                                               join caratulas_equipos on caratulas_equipos.examen_cod = paciente_examenes.examen
                                                join homologacion h on h.examen_cod  = paciente_examenes.examen
                                                and h.examen_cod_equipo  = caratulas_equipos.codili
                                              where paciente_examenes.fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                and paciente_examenes.paciente_cod = '{strNroTubo.Trim()}' 
                                                and paciente_examenes.examen = '{examenhomologado.Trim()}' 
                                                and caratulas_equipos.equipo_cod = '{InterfaceConfig.EquipoCodigoCaratula}' 
                                                and reactivo_cod = '{InterfaceConfig.ReactivoCodigoCaratula}'
                                                and h.equipo_cod = '{InterfaceConfig.NombreEquipo}_ADI'";

            }
            else
            {
                RegistroLog.Instance.RegistraEnLog($"Registra caratula basica",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                query = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa, 
                                                    posiexa,  secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula) 
                                                    select paciente_examenes.paciente_cod,paciente_examenes.hora,paciente_examenes.fecha,paciente_examenes.sede_codigo,
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen, 
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratula_basica.examen_cod,caratula_basica.analito_cod,
                                                    caratula_basica.activo,caratula_basica.nombre,caratula_basica.unid,caratula_basica.tablav, caratula_basica.minimo,
                                                    caratula_basica.intermedio,caratula_basica.maximo,'_',caratula_basica.equipo_cod, caratula_basica.reactivo_cod,
                                                    caratula_basica.tablaa,caratula_basica.tabla1,caratula_basica.tabla2,caratula_basica.resequi,caratula_basica.resequi 
                                               from paciente_examenes 
                                               join caratula_basica on caratula_basica.examen_cod = paciente_examenes.examen 
                                               join homologacion h on h.examen_cod  = paciente_examenes.examen
                                               and h.examen_cod_equipo  = caratula_basica.analito_cod
                                              where fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                and paciente_examenes.paciente_cod = '{strNroTubo.Trim()}' 
                                                and paciente_examenes.examen='{examenhomologado.Trim()}'
												and h.equipo_cod = '{InterfaceConfig.NombreEquipo}_ADI'";
            }

            return _conexion.RunStatement(query, null, CommandType.Text);
        }

        public ResultadoQuery ObtenerAnalitoResulLab(string resultado, string v_Documento, string strNroTubo, string examenhomologado, string analitohomologado)
        {
            string query = $@"select *
                                from resul_lab 
                               where paciente_cod = '{strNroTubo.Trim()}' 
                                 and nit = '{v_Documento}' 
                                 and pac_examen='{examenhomologado.Trim()}' 
                                 and analito_cod = '{analitohomologado}' 
                                 and fecha >= current_date - {InterfaceConfig.DiasAtras}";

            return _conexion.RunQuery(query, null, CommandType.Text);
        }

        public ResultadoStatement InsertarAnalitoResultado(string strNroTubo, string examenhomologado, string analitohomologado, string resultado)
        {
            string query = string.Empty;

            if (InterfaceConfig.CaratulaPorEquipo)
            {
                RegistroLog.Instance.RegistraEnLog($"Registra caratula equipo[{InterfaceConfig.EquipoCodigoCaratula}], reactivo[{InterfaceConfig.ReactivoCodigoCaratula}]",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                query = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa,
                                                    posiexa, secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula)
					                                select paciente_examenes.paciente_cod,paciente_examenes.hora, paciente_examenes.fecha, paciente_examenes.sede_codigo, 
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen,
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratulas_equipos.examen_cod,caratulas_equipos.codili,
                                                    caratulas_equipos.activo, caratulas_equipos.nombre, caratulas_equipos.unid,caratulas_equipos.tablav, caratulas_equipos.minimo,
                                                    caratulas_equipos.intermedio,caratulas_equipos.maximo,'{resultado}',caratulas_equipos.equipo_cod, caratulas_equipos.reactivo_cod,
                                                    caratulas_equipos.tablaa,caratulas_equipos.tabla1,caratulas_equipos.tabla2,caratulas_equipos.resequi,caratulas_equipos.resequi
                                               from paciente_examenes
                                               join caratulas_equipos on caratulas_equipos.examen_cod = paciente_examenes.examen
                                                and caratulas_equipos.codili = '{analitohomologado}'
                                              where paciente_examenes.fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                and paciente_examenes.paciente_cod = '{strNroTubo.Trim()}' 
                                                and paciente_examenes.examen = '{examenhomologado.Trim()}' 
                                                and caratulas_equipos.equipo_cod = '{InterfaceConfig.EquipoCodigoCaratula}' 
                                                and reactivo_cod = '{InterfaceConfig.ReactivoCodigoCaratula}'";
            }
            else
            {
                RegistroLog.Instance.RegistraEnLog($"Registra caratula basica",  InterfaceConfig.NombreLog, EnumEstados.Ok);

                query = $@"insert into resul_lab ( paciente_cod, hora, fecha, sede_codigo, historia, tipodcto_cod, nit, pac_examen, reg_exa, 
                                                    posiexa,  secuencia, codigo, examen_cod, analito_cod, activo, analito, unidades, tablav, minimo, intermedio, maximo, 
                                                    resultado, equipo, reactivo, tablaa, tabla1, tabla2,marcaresultado,evaluacionformula) 
                                                    select paciente_examenes.paciente_cod,paciente_examenes.hora,paciente_examenes.fecha,paciente_examenes.sede_codigo,
                                                    paciente_examenes.historia, paciente_examenes.tipodcto_cod,paciente_examenes.nit,paciente_examenes.examen, 
                                                    paciente_examenes.reg_exa, paciente_examenes.reg_exa ,'1', 1, caratula_basica.examen_cod,caratula_basica.analito_cod,
                                                    caratula_basica.activo,caratula_basica.nombre,caratula_basica.unid,caratula_basica.tablav, caratula_basica.minimo,
                                                    caratula_basica.intermedio,caratula_basica.maximo,'{resultado}',caratula_basica.equipo_cod, caratula_basica.reactivo_cod,
                                                    caratula_basica.tablaa,caratula_basica.tabla1,caratula_basica.tabla2,caratula_basica.resequi,caratula_basica.resequi 
                                               from paciente_examenes 
                                               join caratula_basica on caratula_basica.examen_cod = paciente_examenes.examen 
                                               and caratula_basica.analito_cod = '{analitohomologado}'
                                              where fecha >= current_date - {InterfaceConfig.DiasAtras} 
                                                and paciente_examenes.paciente_cod = '{strNroTubo.Trim()}' 
                                                and paciente_examenes.examen='{examenhomologado.Trim()}'";
            }
            return _conexion.RunStatement(query, null, CommandType.Text);
        }


        public ResultadoQuery ConsultaHomologacionAnalitoQC(string equipoCod, string strVariable)
        {
            string query = $@"select * from homologacion 
                              where equipo_cod = '{equipoCod}' 
                                and examen_cod = '{strVariable}'";

            return _conexion.RunQuery(query, null, CommandType.Text);
        }


        public ResultadoQuery ValidarControlExistente(string analitoControlHomologado, string numlote, string nivel, string analizador,string fecha)
        {
            string query = $@"Select * from valiqc.resultados where  analito = '{analitoControlHomologado}'  and numlote = '{numlote}' and estado = 'N' and  analizador = '{analizador}' and equipo_cod = '{InterfaceConfig.NombreEquipo.Trim()}_QC' and fecha = '{fecha}'";
            return _conexion.RunQuery(query, null, CommandType.Text);
        }

        public ResultadoStatement ActualizarResultadoQC(string id, string analitoControlHomologado, string numlote, string analizador, string resultado, string nivel, string fecha, string equipo_cod)
        {
            string query = $@"Update  valiqc.resultados 
                            set resulnivel{nivel} = '{resultado}'
                            where 
                            id = '{id}' and
                            analito = '{analitoControlHomologado}' and
                            numlote = '{numlote}' and
                            analizador = '{analizador}' and 
                            fecha = '{fecha}' and
                            equipo_cod = '{equipo_cod}'and
                            estado = 'N'";

            return _conexion.RunStatement(query, null, CommandType.Text);
        }

        public ResultadoStatement InsertarResultadoQC(string analitoControlHomologado, string resultadoNumerico, string numlote, string comentario, string nivel, string fecha, string analizador)
        {
            string query = $@"insert into valiqc.resultados (analito,resulnivel{nivel},numlote,analizador,comentario,estado,fecha,equipo_cod) 
                           values('{analitoControlHomologado}','{resultadoNumerico}','{numlote}','{analizador}','{comentario}','N','{fecha}','{InterfaceConfig.NombreEquipo.Trim()}_QC')";
            return _conexion.RunStatement(query, null, CommandType.Text);
        }

        public ResultadoQuery ConsultarSecion(string strSeccion)
        {
            string query = $@"select gruexa_cod from grupos_examenes where ccto_cg1 = '{strSeccion}'";
            return _conexion.RunQuery(query, null, CommandType.Text);
        }

        public ResultadoQuery ConsultarEtiquetaUnificada(string valor)
        {
            string query = $@"SELECT * FROM def_rep_gen
                            where 
                            codigo = '99'
                            and '{valor}' = ANY(STRING_TO_ARRAY(contenido, ','))";

            return _conexion.RunQuery(query, null, CommandType.Text);
        }

        public ResultadoQuery ObtenerExamenesUnificadoProgramacionOrden( string strNroTubo, string sufijo, string sede)
        {
            string query = $@"SELECT 
                     	p.paciente_cod,
						(P.fecha+P.hora) AS fecha,
						p.examen AS examen_annar,
						P.reg_exa,
						e.gruexa_cod,
						h.examen_cod,
                        h.analito,
                        p.reg_exa,
                        h.resultado,
                        h.decimales,
                        h.posicion 
                  FROM 
                        examenes e
                  JOIN 
                        grupos_examenes g ON g.gruexa_cod = e.gruexa_cod 
                  JOIN 
                        homologacion h ON h.examen_cod = e.examen_cod 
                            AND h.equipo_cod = '{equipo}'
	                        AND (h.analito = '' or h.analito is null)
                  JOIN 
                        paciente_examenes p ON p.examen = h.examen_cod
                            AND p.fecha >= current_date - {InterfaceConfig.DiasAtras}                
                  JOIN 
                        def_rep_gen def ON def.codigo = '99' 
                  AND g.gruexa_cod IN (SELECT unnest(STRING_TO_ARRAY(contenido, ',')))
                            AND '{sufijo}' = ANY(STRING_TO_ARRAY(contenido, ','))
                            AND '{sede}' = ANY(STRING_TO_ARRAY(contenido, ','))
                  WHERE 
                        p.paciente_cod = '{strNroTubo}'";

            return _conexion.RunQuery(query, null, CommandType.Text);
        }

        public ResultadoQuery ObtenerFechaHoraServidor()
        {

            string query = $"SELECT now() AT TIME ZONE current_setting('timezone') AS server_time;";


            return _conexion.RunQuery(query, null, CommandType.Text);

        }


        public ResultadoQuery ConsultaExamenesUnificadoHomologado( string variable, string strNroTubo,  string sufijo, string sede)
        {
            string query = $@"Select h.examen_cod,h.analito,p.reg_exa, h.resultado, h.decimales, h.posicion 
                                from examenes e
                                join grupos_examenes g on g.gruexa_cod = e.gruexa_cod 
                                Join homologacion h on h.examen_cod = e.examen_cod 
                                                   and h.equipo_cod='{equipo}'
                                                   and h.examen_cod_equipo = '{variable.Trim()}' 
                                                   AND h.analito <> ''        
                                join paciente_examenes p on p.examen = h.examen_cod
                                                        and p.fecha >= current_date - {InterfaceConfig.DiasAtras}                
                                join def_rep_gen def on def.codigo = '99' 
                                AND g.gruexa_cod IN (SELECT unnest(STRING_TO_ARRAY(contenido, ',')))
                                      and '{sufijo}' = ANY(STRING_TO_ARRAY(contenido, ','))
                                      and '{sede}' = ANY(STRING_TO_ARRAY(contenido, ','))
                               where p.paciente_cod = '{strNroTubo}'";



            return _conexion.RunQuery(query, null, CommandType.Text);
        }



        public ResultadoStatement ActualizarEventosPacienteExamenes(string NumeroTubo,string  ExamenHomologado, string ExamenHora,string ExamenFecha,string Nit, string ExamenRegExa, string fechaEvento, string horaEvento)
        {

            string query = $@"UPDATE eventos_paciente_exam SET fecha_event = '{fechaEvento}', hora_event = '{horaEvento}', 
                                                  observ_event = 'ADICIONO RESULTADO POR {equipo}', usr_codigo = 'INTERFAZ' 
                                                  WHERE paciente_cod = '{NumeroTubo.Trim()}' 
                                                  AND hora = '{ExamenHora}' AND fecha = '{ExamenFecha}' AND tipo_even_cod = '020' 
                                                  AND nit = '{Nit}' AND examen = '{ExamenHomologado}' AND reg_exa = {ExamenRegExa}";


            return _conexion.RunStatement(query, null, CommandType.Text);

        }

        public ResultadoQuery pacienteDatosEvento(string vCodPaciente, string strExamen)
        {
            strQuery = "";
            strQuery = $@"SELECT reg_exa,fecha,hora FROM paciente_examenes 
                                                                 WHERE paciente_cod = '{vCodPaciente}' 
                                                                   AND fecha >= current_date - {diasatras} 
                                                                   AND examen =  '{strExamen}'";


            return _conexion.RunQuery(strQuery, null, CommandType.Text);
        }
    }
}
