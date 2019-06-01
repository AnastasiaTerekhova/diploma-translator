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
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для dictionary.xaml
    /// </summary>
    public partial class Dictionary : Window
    {
        public List<List<string>> words = new List<List<string>>();
        public static string CurrentDirectory { get; set; }

        public Dictionary()
        {
            InitializeComponent();
            //foreach (var file in System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\"))
            //ComboBox.Items.Add(file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 5));
            //if (ComboBox.Items.Count > 0)
            //ComboBox.SelectedIndex = 0;

            foreach (var file in System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\"))
                
                ComboBox.Items.Add(file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 5));
            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        class Answer 
        {
            public string Word { get; set; } 
            public string Translate { get; set; } 
        }
        

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnAddWord_Click(object sender, RoutedEventArgs e)
        {
            words.Add(new List<string>() { tbWord.Text, tbTranslate.Text });
            lbWords.Items.Add($"{tbWord.Text} — {tbTranslate.Text}");
            tbWord.Text = "";
            tbTranslate.Text = "";
        }

        private void BtnAddLang_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = new XDocument();
            XElement list = new XElement("list");
            int index = 0;
            foreach (var item in words)
            {
                XElement word = new XElement("word");
                XAttribute idAttr = new XAttribute("id", index);
                XElement text = new XElement("text", item[0]);
                XElement translationElem = new XElement("translation", item[1]);
                word.Add(idAttr);
                word.Add(text);
                word.Add(translationElem);
                list.Add(word);
                index++;
            }
            xdoc.Add(list);
            xdoc.Save($"D:\\дипломм\\WpfApp1\\WpfApp1\\Dictionaries\\{tbLangName.Text}.xml");
            //Environment.CurrentDirectory + $"\\..\\..\\Dictionaries\\{tbLangName.Text}.xml";

            words = new List<List<string>>();
            lbWords.Items.Clear();
            ComboBox.Items.Add(tbLangName.Text);
            tbLangName.Text = "";
            
        }

    }
}
