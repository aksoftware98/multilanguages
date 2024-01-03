using AKSoftware.Localization.MultiLanguages;
using System.Reflection;
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
using System.Text.RegularExpressions;

namespace WpfSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // getting language files list from embedded resources
            var assembly = Assembly.GetExecutingAssembly();
            var languageFiles =
                assembly.GetManifestResourceNames()
                .Where(t => Regex.IsMatch(t, @"\.([A-Za-z]{2}-[A-Za-z]{2}).ya?ml$"))
                .ToDictionary(
                    t =>
                    {
                        // just for proof-of-concept, there might be a better solution for this
                        var m = Regex.Match(t, @"\.([A-Za-z]{2}-[A-Za-z]{2}).ya?ml$");
                        return m.Groups[1].Value;
                    },
                    t => t
                    );
            YamlDotNet.Serialization.Deserializer deserializer = new();

            foreach (var languageFile in languageFiles)
            {
                using var stream = assembly.GetManifestResourceStream(languageFile.Value);
                if (stream is null) continue;

                byte[] s = new byte[stream.Length];
                stream.Read(s);
                string st = Encoding.UTF8.GetString(s);

                var obj = deserializer.Deserialize<Dictionary<object, object>>(st);
                var names = ((Dictionary<object, object>)obj["languageName"]);
                LanguageSelector.Items.Add(
                    new ComboBoxItem()
                    {
                        Tag = languageFile.Key,
                        Content = $"{names["english"]} / {names["native"]}"
                    }
                    );

            }            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            var selected = combobox?.SelectedItem as ComboBoxItem;
            string languageCode = selected?.Tag as string ?? "ko";

            App.LanguageContainer.SetLanguage(new(languageCode));/*
            LanguageContainer = App.DummyLanguageContainer;
            LanguageContainer = App.LanguageContainer;*/
        }

        public ILanguageContainerService LanguageContainer
        {
            get => (ILanguageContainerService)GetValue(LanguageContainerProperty);
            set => SetValue(LanguageContainerProperty, value);
        }

        public static readonly DependencyProperty LanguageContainerProperty
            = DependencyProperty.RegisterReadOnly(
                nameof(LanguageContainer),
                typeof(ILanguageContainerService),
                typeof(MainWindow),
                new FrameworkPropertyMetadata(
                    App.LanguageContainer,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    null
                    )
                ).DependencyProperty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(App.LanguageContainer["desc:3"]);
        }
    }
}
