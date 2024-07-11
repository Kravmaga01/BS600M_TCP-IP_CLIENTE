using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using BC6200.Service;
using BC6200.Config;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace BC6200.views
{
    public partial class CargueResultados : Page
    {
        public CargueResultados()
        {
            InitializeComponent();

            // Subscribe to changes in the message service
            MessageService.Instance.Messages.CollectionChanged += Messages_CollectionChanged;

            // Load existing messages
            foreach (var message in MessageService.Instance.Messages)
            {
                AddMessageToPanel(message);
            }
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Message newMessage in e.NewItems)
                {
                    AddMessageToPanel(newMessage);
                }
            }
        }

        private void AddMessageToPanel(Message message)
        {
            Border messageBorder = new Border
            {
                CornerRadius = new CornerRadius(15),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Background = Brushes.Transparent
            };

            // Crear un Grid para contener la imagen y el texto
            Grid messageGrid = new Grid
            {
                Margin = new Thickness(5)
            };

            // Definir las columnas del Grid
            messageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            messageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Crear la imagen del mensaje
            Image messageImage = new Image
            {
                Width = 25,
                Height = 25,
                Margin = new Thickness(0, 0, 10, 0), // Espacio entre la imagen y el texto
                VerticalAlignment = VerticalAlignment.Center
            };

            string baseUri = "../assets/";

            // Asignar la imagen y el color del borde basado en el estado del mensaje
            switch (message.Estado)
            {
                case EnumEstados.Ok:
                    messageImage.Source = new BitmapImage(new Uri(baseUri + "OkM.png", UriKind.Relative));
                    messageBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4BDAD8"));
                    break;
                case EnumEstados.Info:
                    messageImage.Source = new BitmapImage(new Uri(baseUri + "InterpretandoM.png", UriKind.Relative));
                    messageBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C8AE2"));
                    break;
                case EnumEstados.Process:
                    messageImage.Source = new BitmapImage(new Uri(baseUri + "Process.png", UriKind.Relative));
                    messageBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C8AE2"));
                    break;
                case EnumEstados.Warning:
                    messageImage.Source = new BitmapImage(new Uri(baseUri + "Esperando.png", UriKind.Relative));
                    messageBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDD941"));
                    break;
                case EnumEstados.Error:
                    messageImage.Source = new BitmapImage(new Uri(baseUri + "ErrorM.png", UriKind.Relative));
                    messageBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9777A"));
                    break;
                case EnumEstados.Empty:
                    messageImage.Source = null;
                    break;
                default:
                    break;
            }

            // Crear el texto del mensaje
            TextBlock messageText = new TextBlock
            {
                Text = message.Text,
                Foreground = new SolidColorBrush(Color.FromRgb(62, 62, 62)),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Segoe UI Semibold"),
                VerticalAlignment = VerticalAlignment.Center
            };

            // Agregar la imagen y el texto al Grid
            Grid.SetColumn(messageImage, 0);
            Grid.SetColumn(messageText, 1);
            messageGrid.Children.Add(messageImage);
            messageGrid.Children.Add(messageText);

            // Agregar el Grid dentro del borde
            messageBorder.Child = messageGrid;

            // Agregar el borde al panel de mensajes
            MessagesPanel.Children.Add(messageBorder);

            // Auto-desplazamiento hacia el final
            MessageScrollViewer.ScrollToEnd();
        }



        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            MessageService.Instance.Messages.Clear();
            MessagesPanel.Children.Clear();
        }
    }
}
