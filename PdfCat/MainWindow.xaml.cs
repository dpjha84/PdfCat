using Microsoft.Win32;
using PDFiumSharp;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PdfCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Process(string filePath)
        {
            sp.Children.Clear();
            using (var doc = new PdfDocument(filePath))
            {
                foreach (var page in doc.Pages)
                {
                    using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, true))
                    {
                        var border = new Border();
                        border.BorderBrush = new BrushConverter().ConvertFromString("DarkGray") as SolidColorBrush;
                        border.BorderThickness = new Thickness(1);
                        border.Margin = new Thickness(5);
                        var image = new Image();
                        page.Render(bitmap);
                        image.Source = new BmpBitmapDecoder(bitmap.AsBmpStream(), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
                        border.Child = image;
                        sp.Children.Add(border);
                    }
                }
            }
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Pdf files (*.pdf)|*.pdf";
            openFileDialog.InitialDirectory = "E:\\Test";
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = openFileDialog.FileNames[0];
                Process(textBox.Text);
            }
        }
    }
}
