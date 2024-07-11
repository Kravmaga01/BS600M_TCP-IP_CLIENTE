using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BC6200.SocketClient
{
    class SocketCliente
    {
        private Stream mensajesEnviarRecibir; // Para enviar y recibir datos del servidor
        private string ipServidor; // Dirección IP
        private int puertoServidor; // Puerto de escucha

        private TcpClient clienteTCP;
        private Thread hiloMensajeServidor; // Hilo para escuchar mensajes del servidor
        private volatile bool isRunning = true; // Bandera para controlar la ejecución del hilo

        public event Action ConexionTerminada;
        public event Action<string> DatosRecibidos;

        // Dirección IP del servidor al que nos conectaremos
        public string IP
        {
            get { return ipServidor; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La dirección IP no puede estar vacía.");
                }
                ipServidor = value;
            }
        }

        // Puerto por el que realizar la conexión al servidor
        public int Puerto
        {
            get { return puertoServidor; }
            set
            {
                if (value <= 0 || value > 65535)
                {
                    throw new ArgumentOutOfRangeException("El puerto debe estar entre 1 y 65535.");
                }
                puertoServidor = value;
            }
        }

        // Procedimiento para realizar la conexión con el servidor
        public void Conectar()
        {
            if (clienteTCP != null && clienteTCP.Connected)
            {
                throw new InvalidOperationException("Ya existe una conexión activa.");
            }

            clienteTCP = new TcpClient();

            try
            {
                // Conectar con el servidor
                clienteTCP.Connect(IP, Puerto);
                mensajesEnviarRecibir = clienteTCP.GetStream();

                // Crear hilo para establecer escucha de posibles mensajes
                // enviados por el servidor al cliente
                hiloMensajeServidor = new Thread(LeerSocket)
                {
                    IsBackground = true
                };

                isRunning = true; // Establecer la bandera de ejecución del hilo
                hiloMensajeServidor.Start();
            }
            catch (SocketException ex)
            {
                throw new InvalidOperationException("No se pudo conectar al servidor.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al intentar conectar.", ex);
            }
        }

        // Procedimiento para cerrar la conexión con el servidor
        public void Desconectar()
        {
            if (clienteTCP == null || !clienteTCP.Connected)
            {
                throw new InvalidOperationException("No hay una conexión activa para cerrar.");
            }

            try
            {
                // Establecer la bandera para detener la ejecución del hilo
                isRunning = false;

                // Desconectar del servidor
                clienteTCP.Close();

                
              
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al intentar desconectar.", ex);
            }
            finally
            {
                ConexionTerminada?.Invoke(); // Generar evento de conexión terminada
            }
        }

        // Enviar mensaje al servidor
        public void EnviarDatos(string Datos)
        {
            if (string.IsNullOrWhiteSpace(Datos))
            {
                throw new ArgumentException("Los datos a enviar no pueden estar vacíos.");
            }

            byte[] BufferDeEscritura = Encoding.ASCII.GetBytes(Datos);

            if (mensajesEnviarRecibir != null)
            {
                try
                {
                    mensajesEnviarRecibir.Write(BufferDeEscritura, 0, BufferDeEscritura.Length);
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException("Error al enviar datos al servidor.", ex);
                }
            }
            else
            {
                throw new InvalidOperationException("No se puede enviar datos, la conexión no está establecida.");
            }
        }

        private void LeerSocket()
        {
            byte[] BufferDeLectura = new byte[100];

            while (isRunning)
            {
                try
                {
                    // Esperar a que llegue algún mensaje
                    int bytesLeidos = mensajesEnviarRecibir.Read(BufferDeLectura, 0, BufferDeLectura.Length);

                    if (bytesLeidos > 0)
                    {
                        // Generar evento DatosRecibidos cuando se reciban datos desde el servidor
                        DatosRecibidos?.Invoke(Encoding.ASCII.GetString(BufferDeLectura, 0, bytesLeidos));
                    }
                }
                catch (IOException)
                {
                    break; // Salir del bucle si hay una excepción de E/S
                }
                catch (ObjectDisposedException)
                {
                    break; // Salir del bucle si el objeto está desechado
                }
                catch (Exception ex)
                {
                    // Otras excepciones pueden ser manejadas aquí
                    Console.WriteLine($"Error desconocido: {ex.Message}");
                    break;
                }
            }

            // Finalizar conexión y generar evento ConexionTerminada
            ConexionTerminada?.Invoke();
        }
    }
}
