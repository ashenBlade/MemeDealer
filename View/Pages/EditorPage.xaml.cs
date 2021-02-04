using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Core;

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
            MainCanvas.Background = new ImageBrush(new BitmapImage(new Uri(Meme.PathToFile)));
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
            text.DragEnter += (sender, args) =>
            {
                InkCanvas.SetTop(text, args.GetPosition(MainCanvas).Y);
                InkCanvas.SetLeft(text, args.GetPosition(MainCanvas).X);
                MainCanvas.UpdateLayout();
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
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using var fs = File.OpenWrite("logo.png");
            encoder.Save(fs);
            MessageBox.Show("Мем создан успешно!");
        }

    }
}