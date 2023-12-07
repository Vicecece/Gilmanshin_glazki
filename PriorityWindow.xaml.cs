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

namespace Gilmanshin_glazki
{
    /// <summary>
    /// Логика взаимодействия для PriorityWindow.xaml
    /// </summary>
    public partial class PriorityWindow : Window
    {
        private Agent _currentAgent = new Agent();
        public PriorityWindow(int p)
        {
            InitializeComponent();
            Priority.Text = p.ToString();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Priority.Text != string.Empty)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Введите приоритет");
            }
        }
    }
}
