using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
//using YandexLinguistics.NET;

namespace WpfApp1
{
    // TODO: добавьте следующее к определению SomeType, чтобы видеть этот визуализатор при отладке экземпляров SomeType:
    // 
    //  [DebuggerVisualizer(typeof(Translator))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// Визуализатор для SomeType.  
    /// </summary>
    public class Translator : DialogDebuggerVisualizer
    {
        /*YandexLinguistics.NET.Translator tr;*/

        private string appID = "6CE9C85A41571C050C379F60DA173D286384E0F2";

        private List<Language> liste = new List<Language>();

        public Translator()
        {
            this.PopulateLanguageList();
        }


        public List<Language> DillerListesi
        {
            get
            {
                return (liste.OrderBy(l => l.dil).ToList());
            }
        }

        private void PopulateLanguageList()
        {
            liste.Add(new Language { dil = "Арабский", dil_kod = "ar" });
            liste.Add(new Language { dil = "Чешский", dil_kod = "cs" });
            liste.Add(new Language { dil = "Датский", dil_kod = "da" });
            liste.Add(new Language { dil = "Немецкий", dil_kod = "de" });
            liste.Add(new Language { dil = "Английский", dil_kod = "en" });
            liste.Add(new Language { dil = "Эстонский", dil_kod = "et" });
            liste.Add(new Language { dil = "Финский", dil_kod = "fi" });
            liste.Add(new Language { dil = "Голландский", dil_kod = "nl" });
            liste.Add(new Language { dil = "Греческий", dil_kod = "el" });
            liste.Add(new Language { dil = "Иврит", dil_kod = "he" });
            liste.Add(new Language { dil = "Гаитянский Креольский", dil_kod = "ht" });
            liste.Add(new Language { dil = "Хинди", dil_kod = "hi" });
            liste.Add(new Language { dil = "Венгерский", dil_kod = "hu" });
            liste.Add(new Language { dil = "Индонезийский", dil_kod = "id" });
            liste.Add(new Language { dil = "Итальянский", dil_kod = "it" });
            liste.Add(new Language { dil = "Японский", dil_kod = "ja" });
            liste.Add(new Language { dil = "Корейский", dil_kod = "ko" });
            liste.Add(new Language { dil = "Литовский", dil_kod = "lt" });
            liste.Add(new Language { dil = "Латышский", dil_kod = "lv" });
            liste.Add(new Language { dil = "Норвежский", dil_kod = "no" });
            liste.Add(new Language { dil = "Польский", dil_kod = "pl" });
            liste.Add(new Language { dil = "Португальский", dil_kod = "pt" });
            liste.Add(new Language { dil = "Румынский", dil_kod = "ro" });
            liste.Add(new Language { dil = "Испанский", dil_kod = "es" });
            liste.Add(new Language { dil = "Русский", dil_kod = "ru" });
            liste.Add(new Language { dil = "Словацкий", dil_kod = "sk" });
            liste.Add(new Language { dil = "Словенский", dil_kod = "sl" });
            liste.Add(new Language { dil = "Шведский", dil_kod = "sv" });
            liste.Add(new Language { dil = "Тайский", dil_kod = "th" });
            liste.Add(new Language { dil = "Турецкий", dil_kod = "tr" });
            liste.Add(new Language { dil = "Украинский", dil_kod = "uk" });
            liste.Add(new Language { dil = "Вьетнамский", dil_kod = "vi" });
            liste.Add(new Language { dil = "Упрощенный Китайский", dil_kod = "zh-CHS" });
            liste.Add(new Language { dil = "Традиционный Китайский", dil_kod = "zh-CHT" });
        }

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");

            // TODO: получите объект, для которого нужно отобразить визуализатор.
            //       Выполните приведение результата objectProvider.GetObject() 
            //       к типу визуализируемого объекта.
            object data = (object)objectProvider.GetObject();

            // TODO: отобразите свое представление объекта.
            //       Замените displayForm на свой объект Form или Control.
            using (Form displayForm = new Form())
            {
                displayForm.Text = data.ToString();
                windowService.ShowDialog(displayForm);
            }
        }

        // TODO: добавьте следующее к своему коду тестирования для тестирования визуализатора:
        // 
        //    Translator.TestShowVisualizer(new SomeType());
        // 
        /// <summary>
        /// Тестирует визуализатор, разместив его вне отладчика.
        /// </summary>
        /// <param name="objectToVisualize">Объект для отображения в визуализаторе.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(Translator));
            visualizerHost.ShowVisualizer();
        }

        public string GetTranslatedText(string textToTranslate, string fromLang, string toLang)
        {
            string ceviri;

            if (fromLang != toLang)
            {
                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?appId=" +
                            appID + "&text=" + textToTranslate + "&from=" + fromLang + "&to=" + toLang;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                try
                {
                    WebResponse response = request.GetResponse();
                    Stream strm = response.GetResponseStream();
                    StreamReader sr = new StreamReader(strm);
                    ceviri = sr.ReadToEnd();

                    response.Close();
                    sr.Close();
                }
                catch (WebException)
                {
                    System.Windows.MessageBox.Show("Нет подключения к интернету.",
                                "Translator", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return (string.Empty);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Невозможно перевести на аналогичный язык", "Translator",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return (string.Empty);
            }


            return (XElement.Parse(ceviri).Value);
        }
    }
}
