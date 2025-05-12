using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp3.View
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public string InputChoice { get; set; }
        public bool Success { get; set; }

        public EditWindow(string text)
        {
            InputChoice = text;
            InitializeComponent();
            InputChoiceTextBox.Text = text;
        }


        private void ConfirmChoiceChange_Click(object sender, RoutedEventArgs e)
        {
            Success = true;
            InputChoice = InputChoiceTextBox.Text.Trim();
            Close();
        }

        private void CancelChoiceChange_Click(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }

        private void InputChoiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmChoiceChange.IsEnabled = InputChoiceTextBox.Text.Length != 0;

        }
    }
}
