using Microsoft.Win32;
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
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class UploadDialog : Window
    {
        MainWindow.PhotoDesc res;
        public UploadDialog()
        {
            InitializeComponent();
            res = new MainWindow.PhotoDesc();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            res.title = title.Text;
            res.file = file.Text;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        public MainWindow.PhotoDesc Answer
        {
            get { return res; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                res.file = op.FileName;
                file.Text = op.FileName;
            }
        }
    }
}
