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
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Practice.xaml
    /// </summary>
    public partial class Practice : Window
    {

        List<List<string>> CurrentDictionary = null;
        Random rand = new Random(DateTime.Now.Millisecond * DateTime.Now.Second); // Чтоб точно рандом

        #region Для марафона
        int mrthSec = 15; // секунд осталось
        int mrthCount = 10; // всего вопросов  в марафоне
        int mrthCountTrue = 0; // всего верных ответов в марафоне
        int mrthIndex = 0; // текущий вопрос в марафоне 
        bool mrthCurrentVariant; // текущий задуманный ответ

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(); // ТАЙМЕР

        #endregion

        public Practice()
        {
            InitializeComponent();
            ti1.Height = 0;
            ti2.Height = 0;
            ti3.Height = 0;
            ti4.Height = 0;
            ti5.Height = 0;


            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lMarathonTimer.Content = $"{mrthSec--} сек. (слово {mrthIndex}/{mrthCount})";
            if (mrthSec < 0)
            {
                MarathonNextWord();
            }
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 1;
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 2;
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 3;

            lMarathonTimer.Visibility = btnMarathonFalse.Visibility = btnMarathonTrue.Visibility = Visibility.Visible;

            //обнуление счётчиков марафона
            mrthCountTrue = 0; // всего верных ответов в марафоне
            mrthIndex = 0; // текущий вопрос в марафоне

            if (System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\").Length < 1)
            {
                MessageBox.Show("Отсутствуют словари.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadDictionary(System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\")[0]);

            MarathonNextWord();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void LoadDictionary(string dicPath)
        {
            CurrentDictionary = new List<List<string>>();

            XDocument xdoc = new XDocument();
            XElement list = XElement.Load(dicPath);
            var words = list.Elements();
            foreach (var item in words)
            {
                try
                {
                    CurrentDictionary.Add(new List<string>() {
                        item.Elements().ToList()[0].Value,
                        item.Elements().ToList()[1].Value
                    });
                }
                catch { }
            }
        }

        private void MarathonNextWord()
        {
            if (CurrentDictionary.Count < 2)
            {
                MessageBox.Show("В словаре меньше двух слов.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (++mrthIndex > mrthCount) // если конец
            {
                timer.Stop();
                lMarathonTimer.Visibility = btnMarathonFalse.Visibility = btnMarathonTrue.Visibility = Visibility.Hidden;

                lMarathon.Content = $"Верных ответов {mrthCountTrue} из {mrthCount}";
            }
            else // иначе продолжаем
            {
                mrthSec = 15;
                timer.Start();
                Timer_Tick(null, EventArgs.Empty);
                mrthCurrentVariant = rand.Next(2) == 1; // 50/50 верный или неверный вариант мы задумали

                int ind = rand.Next(CurrentDictionary.Count);
                string word = CurrentDictionary[ind][0]; // рандомное слово
                string translate = "";

                if (mrthCurrentVariant) // если ждем ответ "верно"
                {
                    translate = CurrentDictionary[ind][1];
                }
                else// если ждем ответ "не верно"
                {
                    int anotherInd = ind;
                    while (anotherInd == ind) // если ВДРУГ рандом кинет нас снова на этот же индекс, то слова запросим новый, чтобы не вводить юзера в  заблуждение
                        anotherInd = rand.Next(CurrentDictionary.Count);
                    translate = CurrentDictionary[anotherInd][1];
                }

                lMarathon.Content = word + " — " + translate;
            }
        }

        private void CheckMarathonWord(bool variant)
        {
            if (variant == mrthCurrentVariant)
                mrthCountTrue++;
            MarathonNextWord();
        }

        private void BtnMarathonTrue_Click(object sender, RoutedEventArgs e)
        {
            CheckMarathonWord(true);
        }

        private void BtnMarathonFalse_Click(object sender, RoutedEventArgs e)
        {
            CheckMarathonWord(false);
        }

    }
}

