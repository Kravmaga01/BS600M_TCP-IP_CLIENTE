using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BC6200.UserControls
{
    public partial class UCInput : UserControl
    {
        // DependencyProperty para LabelText
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(UCInput));

        // DependencyProperty para ErrorText
        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(UCInput));

        // DependencyProperty para LabelForeground
        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(UCInput), new PropertyMetadata(Brushes.Black));


        public static readonly DependencyProperty InputTextProperty =
        DependencyProperty.Register("InputText", typeof(string), typeof(UCInput), new PropertyMetadata(string.Empty, OnInputTextChanged));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public Brush LabelForeground
        {
            get { return (Brush)GetValue(LabelForegroundProperty); }
            set { SetValue(LabelForegroundProperty, value); }
        }

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        private static void OnInputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UCInput;
            if (control != null && control.InputTextBox != null)
            {
                control.InputTextBox.Text = e.NewValue as string;
            }
        }

        // Event handler para cuando cambia el texto en el TextBox
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.InputText = InputTextBox.Text;
        }

        public UCInput()
        {
            InitializeComponent();
            // Establecer el color inicial del Label y del borde
            LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
            BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
            BorderInputTextBox.BorderThickness = new Thickness(2);

            // Suscribirse a los eventos de LostFocus, GotFocus y TextChanged
            InputTextBox.LostFocus += CityTextBox_LostFocus;
            InputTextBox.GotFocus += InputTextBox_GotFocus;
            InputTextBox.TextChanged += CityTextBox_TextChanged;
        }

        private void CityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                LabelForeground = Brushes.Red;
                BorderInputTextBox.BorderBrush = Brushes.Red;
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                LabelForeground = Brushes.Red;
                BorderInputTextBox.BorderBrush = Brushes.Red;
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void CityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                LabelForeground = Brushes.Red;
                BorderInputTextBox.BorderBrush = Brushes.Red;
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4051FC"));
            LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4051FC"));
            BorderInputTextBox.BorderThickness = new Thickness(2);
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                LabelForeground = Brushes.Red;
                BorderInputTextBox.BorderBrush = Brushes.Red;
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                LabelForeground = Brushes.Red;
                BorderInputTextBox.BorderBrush = Brushes.Red;
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderThickness = new Thickness(2);
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void InputTextBox_Initialized(object sender, EventArgs e)
        {
            
               
           
                LabelForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B4B8B"));
                BorderInputTextBox.BorderThickness = new Thickness(2);
           
        }
    }
}
