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

namespace Memory_game_T2.Views
{
    /// <summary>
    /// Interaction logic for CustomGameModeDialog.xaml
    /// </summary>
    public partial class CustomGameModeDialog : Window
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public CustomGameModeDialog()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsTextBox.Text, out int rows) && int.TryParse(ColumnsTextBox.Text, out int cols))
            {
                Rows = rows;
                Columns = cols;
                if (rows * cols % 2 != 0)
                {
                    MessageBox.Show("The number of cards must be even!");
                    return;
                }
                if (rows < 2 || rows > 6 || cols < 2 || cols > 6)
                {
                    MessageBox.Show("Rows and columns must be between 2 and 6.");
                    return;
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter numbers.");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
