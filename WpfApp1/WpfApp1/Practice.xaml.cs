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
    public partial class Practice : Window
    {

        class Answer
        {
            public string word { get; set; }
            public string translate { get; set; }
            public string answer { get; set; }
            public string result { get; set; }
        }
        Dictionary dict = new Dictionary();
        List<Answer> answers = new List<Answer>();

        List<List<string>> CurrentDictionary = null;
        Random rand = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

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
            if (System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Dictionaries\\").Length < 1)
            {
                MessageBox.Show("Отсутствуют словари.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

            LoadDictionary(Environment.CurrentDirectory + $"\\..\\..\\Dictionaries\\{dict.ComboBox.SelectedItem}.xml");

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
                    while (anotherInd == ind)
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

        int n = -1;
        private void NextTestWord() //вывод следующего слова или результата
        {
            if (++n >= CurrentDictionary.Count) // если следующий элемент отсутствует, то переходим к результатам.
                tc.SelectedIndex = 4;
            else // иначе следующее слово
            {
                lWord.Content = CurrentDictionary[n][0];
                tbTestTranslate.Text = "";
            }
        }
        private void tbTestTranslate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                answers.Add(new Answer()
                {
                    answer = tbTestTranslate.Text,
                    word = CurrentDictionary[n][0],
                    translate = CurrentDictionary[n][1],
                    result = CurrentDictionary[n][1].ToLower() == tbTestTranslate.Text.ToLower() ? "Верно" : "Неверно"
                });
                NextTestWord();
            }
        }

        private void tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tc.SelectedIndex)
            {
                case 1:
                    LoadDictionary(Environment.CurrentDirectory + $"\\..\\..\\Dictionaries\\{dict.ComboBox.SelectedItem}.xml");
                    n = -1;
                    answers.Clear();
                    Random r = new Random();
                    for (int i = 0; i < CurrentDictionary.Count; i++)
                    {
                        int randIndex = r.Next(CurrentDictionary.Count);
                        var word = CurrentDictionary[i];
                        CurrentDictionary[i] = CurrentDictionary[randIndex];
                        CurrentDictionary[randIndex] = word;
                    }

                    NextTestWord();
                    break;

                case 4: // итоги
                    lwAnswers.ItemsSource = null;
                    lwAnswers.ItemsSource = answers;
                    break;
            }
        }
    }
}

