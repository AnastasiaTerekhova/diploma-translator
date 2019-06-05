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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            // загрузка булевых значений
            cbAutoload.IsChecked = Properties.Settings.Default.Autoload;
            cbKeyboardLock.IsChecked = Properties.Settings.Default.KeyboardLock;
            cbMouseLock.IsChecked = Properties.Settings.Default.MouseLock;

            // загрузка числовых значений
            tbWordsInTest.Text = Properties.Settings.Default.TestMinWords.ToString();
            tbMinutesForTest.Text = Properties.Settings.Default.TestMinMinutes.ToString();
            tbInterval.Text = Properties.Settings.Default.TestInterval.ToString();

            // загрузка списка словарей и отметка не отключённых
            foreach (var file in System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\"))
            {
                string nameNewDic = file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 5);
                lbDictionaties.Items.Add(new CheckBox()
                {
                    Content = nameNewDic,
                    IsChecked = Properties.Settings.Default.DisabledDictionaries.Split('|').ToList().IndexOf(nameNewDic) < 0
                });
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void tb_KeyPress(object sender, KeyEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Key.ToString()[e.Key.ToString().Length - 1])); //  извращение, но только так вышло
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // сохранение булевых значений
            Properties.Settings.Default.Autoload = cbAutoload.IsChecked.Value;
            Properties.Settings.Default.KeyboardLock = cbKeyboardLock.IsChecked.Value;
            Properties.Settings.Default.MouseLock = cbMouseLock.IsChecked.Value;

            // сохранение числовых значений
            int words = 5, minutes = 5, interval = 30; // значения по умолчанию, если пользователь всё же смог ввести что-то не то (например, пустую строку)
            int.TryParse(tbWordsInTest.Text, out words);
            int.TryParse(tbMinutesForTest.Text, out minutes);
            int.TryParse(tbInterval.Text, out interval);
            Properties.Settings.Default.TestMinWords = words;
            Properties.Settings.Default.TestMinMinutes = minutes;
            Properties.Settings.Default.TestInterval = interval;

            // сохранение списка отключённых словарей 
            string disabledDictionaries = "";
            foreach (var item in lbDictionaties.Items)
            {
                var cb = item as CheckBox;
                if (!cb.IsChecked.Value)
                    disabledDictionaries += "|" + cb.Content.ToString();
            }
            Properties.Settings.Default.DisabledDictionaries = disabledDictionaries.Trim('|');

            Properties.Settings.Default.Save(); // сохранение по факту
        }

        private void BtnDelSelDic_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить словарь? Это действие невозможно отменить, желаете продолжить?",
                "Внимание!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var file in System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\"))
                {
                    if ((lbDictionaties.SelectedItem as CheckBox).Content.ToString() == file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 5))
                    {
                        lbDictionaties.Items.Remove(lbDictionaties.SelectedItem);
                        System.IO.File.Delete(file);
                        break;
                    }
                }
            }
        }
    }
}
