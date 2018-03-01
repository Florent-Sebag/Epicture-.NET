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

namespace DotNet_epicture_2017
{
    /// <summary>
    /// Logique d'interaction pour ConnectDialog.xaml
    /// </summary>
    public partial class ConnectDialog : Window
    {
        public ConnectDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        public string Answer
        {
            get { return result.Text; }
        }
    }
}
