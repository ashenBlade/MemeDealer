using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Core;
using Microsoft.Win32;

namespace View.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class EditorPage : Page
    {
        private Meme Meme { get; set; }
        private MemeRepository Repo { get; set; }

        public EditorPage( MemeRepository repo , Meme meme = null)
        {
            InitializeComponent();
            Repo = repo;
            Meme = meme;
            if (Meme != null)
                SetBackgroundImage(Meme);
            SetCanvasBehaviour(MainCanvas);
            SaveButton.PreviewMouseLeftButtonDown += SaveButton_OnMouseLeftButtonDown;
        }

        public void SetBackgroundImage(Meme meme)
        {
            Reset();
            Meme = meme;
            var background = new ImageBrush();
            background.ImageSource = new BitmapImage(new Uri(Meme.PathToFile));
            background.Stretch = Stretch.Uniform;
            MainCanvas.Background = background;
        }

        private void Reset()
        {
            MainCanvas.Background = null;
            MainCanvas.Children.Clear();
            MainCanvas.Strokes.Clear();
        }

        private void SetCanvasBehaviour(InkCanvas canvas)
        {
            canvas.EditingMode = InkCanvasEditingMode.None;
            canvas.MoveEnabled = true;
        }

        private void MainCanvas_OnMouseLeftButtonDown( object sender, MouseEventArgs e )
        {
            var element = e.OriginalSource as UIElement;
            var canvas = sender as InkCanvas;
            if (element == null || canvas == null)
                return;
            var text = new TextBox();
            SetTextBoxStyle(text);
            SetTextBoxBehaviour(text);
            InkCanvas.SetTop(text, e.GetPosition(MainCanvas).Y );
            InkCanvas.SetLeft(text, e.GetPosition(MainCanvas).X);
            MainCanvas.Children.Add(text);
            MainCanvas.UpdateLayout();
            MainCanvas.CommandBindings.Clear();
        }

        private void SetTextBoxStyle(TextBox text)
        {
            text.Visibility = Visibility.Visible;
            text.FontSize = 30;
            text.BorderBrush = new SolidColorBrush(Colors.Transparent);
            text.Background = new SolidColorBrush(Colors.Transparent);
            text.TextAlignment = TextAlignment.Center;
            text.TextWrapping = TextWrapping.WrapWithOverflow;
            text.Text = "Текст";
        }

        private void SetTextBoxBehaviour(TextBox text)
        {
            text.MouseRightButtonDown += (sender, args) =>
            {
                MainCanvas.Children.Remove(text);
                MainCanvas.UpdateLayout();
            };
            text.KeyDown += (sender, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    Keyboard.ClearFocus();
                }
            };
        }

        private void DownloadButton_MouseDown( object sender, MouseButtonEventArgs e )
        {
        }

        private void MainCanvas_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as InkCanvas;
            var element = e.OriginalSource as UIElement;
            if (canvas == null || element == null)
                return;
            if (canvas.Children.Contains(element))
                canvas.Children.Remove(element);
        }

        private void SaveButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // MainCanvas.UpdateLayout();
            MainCanvas.Focus();
            var rtb = new RenderTargetBitmap((int) MainCanvas.ActualWidth, (int) MainCanvas.ActualHeight, 92, 92,
                                             PixelFormats.Default);
            rtb.Render(MainCanvas);
            // var cropped = new CroppedBitmap(rtb, new Int32Rect(50, 50,
            // (int) MainCanvas.RenderSize.Width,
            // (int) MainCanvas.RenderSize.Height));
            var pathToSave = GetPathToSave();
            var encoder = GetEncoder(new FileInfo(pathToSave).Extension);
            // Add image
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            SaveImage(pathToSave, encoder);
            MessageBox.Show("Мем создан успешно!");
        }

        private BitmapEncoder GetEncoder(string extension)
        {
            return extension switch
                   {
                       ".png"  => new PngBitmapEncoder(),
                       ".jpg"  => new JpegBitmapEncoder(),
                       ".bmp"  => new BmpBitmapEncoder(),
                       ".tiff" => new TiffBitmapEncoder(),
                       _       => throw new ArgumentException($"{extension} is not valid extension")
                   };
        }

        private void SaveImage(string path, BitmapEncoder encoder)
        {
            using var fs = File.OpenWrite(path);
            encoder.Save(fs);
        }

        private string GetPathToSave()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Png|*.png|Jpg|*.jpg|Jpeg|*.jpeg|Bmp|*.bmp|Tiff|.tiff";
            saveFileDialog.Title = "Выберите файл для сохранения";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

    }
}