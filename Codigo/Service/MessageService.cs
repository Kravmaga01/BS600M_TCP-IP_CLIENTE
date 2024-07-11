using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC6200.Service
{
    public class MessageService
    {
        private static readonly Lazy<MessageService> instance = new Lazy<MessageService>(() => new MessageService());

        public static MessageService Instance => instance.Value;

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        private MessageService() { }

        public void AddMessage(string msg, EnumEstados estado)
        {
            string fechaActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Messages.Add(new Message { Text = $"{fechaActual}: {msg}", Estado = estado });
        }
    }

    public class Message
    {
        public string Text { get; set; }
        public EnumEstados Estado { get; set; }
    }

    public enum EnumEstados
    {
        Ok,
        Info,
        Process,
        Warning,
        Error,
        Empty
    }
}
