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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Xml;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Practice.xaml
    /// </summary>
    public partial class Practice : Window
    {
        public Practice()
        {
            InitializeComponent();
            ti1.Height = 0;
            ti2.Height = 0;
            ti3.Height = 0;
            ti4.Height = 0;
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //выбрать нужную вкладку по индексу, начиная с нуля
            tc.SelectedIndex = 0;
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 1;
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 2;
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 3;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
