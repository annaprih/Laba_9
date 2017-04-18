using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using ComboBox = System.Windows.Controls.ComboBox;
using Cursor = System.Windows.Forms.DataVisualization.Charting.Cursor;
using DataFormats = System.Windows.DataFormats;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Laba9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int amountOfWindows = 0;
        public MainWindow()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (System.Threading.Thread.CurrentThread.CurrentUICulture.Name)
            {
                case "ru-RU":
                    dict.Source = new Uri("ru_RU.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("en_UA.xaml", UriKind.Relative);
                    break;
            }
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);

            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.ScrollE;
            this.Icon = new BitmapImage(new Uri(@"icon.ico", UriKind.Relative));
            amountOfWindows++;
            this.Title = $"Window{amountOfWindows}";
            SizeSlider.ValueChanged += SizSlider;
            Size.ValueChanged += SizSlider;
            Style.SelectionChanged += Style_Click;
            StyleComboBox.SelectionChanged += Style_Click;
            this.Closed += (sender, args) => { amountOfWindows--; };
            Style.ItemsSource = Fonts.SystemFontFamilies;
            StyleComboBox.ItemsSource = Fonts.SystemFontFamilies;


        }

        private void SizSlider(object sender, RoutedEventArgs e)
        {

            SizeTextBlock.Text = Convert.ToInt32(((Slider)sender).Value).ToString();
            SizeText.Text = Convert.ToInt32(((Slider)sender).Value).ToString();
            TextSelection textSelection = richTextBox.Selection;
            textSelection.ApplyPropertyValue(FontSizeProperty, ((Slider)sender).Value);
        }

        private void New_Click(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Close_Click(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RichText Files (*.Rtf)|*.Rtf";
            if (openFileDialog.ShowDialog() == true)
            {
                this.Title = openFileDialog.FileName;
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    textRange.Load(fileStream, DataFormats.Rtf);
                }
            }

        }

        private void Save_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "RichText Files (*.Rtf)|*.Rtf";
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Title = saveFileDialog.FileName;
                using (FileStream fileStream = File.Create(saveFileDialog.FileName))
                {
                    TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    textRange.Save(fileStream, DataFormats.Rtf);
                }
            }

        }

        private void Style_Click(object sender, RoutedEventArgs e)
        {

            TextSelection textSelection = richTextBox.Selection;
            textSelection.ApplyPropertyValue(FontFamilyProperty, ((ComboBox)sender).SelectedItem);

        }

        private void AmountSymbols(object sender, EventArgs e)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            AmountOfSymbols.Content = textRange.Text.Length;
        }

        private void Font_Click(object sender, EventArgs e)
        {
            TextSelection textSelection = richTextBox.Selection;

            FontDialog font = new FontDialog();
            font.ShowEffects = false;

            if (font.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                double fontSize = font.Font.Size;
                textSelection.ApplyPropertyValue(FontFamilyProperty, new FontFamily(font.Font.Name));
                textSelection.ApplyPropertyValue(FontSizeProperty, fontSize);

                if (font.Font.Italic)
                {
                    textSelection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                }
                else
                {
                    textSelection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
                }
                if (font.Font.Bold)
                {
                    textSelection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                }
                else
                {
                    textSelection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
                }
            }
        }

        private void Color_Click(object sender, EventArgs e)
        {
            TextSelection textSelection = richTextBox.Selection;
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.Color = System.Drawing.Color.Black;
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Brush br = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                textSelection.ApplyPropertyValue(TextElement.ForegroundProperty, br);
            }

        }


    }
}
