using System;
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
        }

        public void SetBackgroundImage(Meme meme)
        {
            Meme = meme;
            MainCanvas.Background = new ImageBrush(new BitmapImage(new Uri(Meme.PathToFile)));
        }

        private void MainCanvas_MouseLeftButtonDown( object sender, MouseEventArgs e )
        {
            var element = e.OriginalSource as UIElement;
            var canvas = sender as Canvas;
            if (element == null || canvas == null)
                return;
            var text = new TextBox { Text = "Hello, world",
                                       FontSize = 30,
                                       Foreground = new SolidColorBrush(Colors.Black),
                                       Background = new SolidColorBrush(Colors.Transparent)
                                   };
            Canvas.SetTop(text, Mouse.GetPosition(MainCanvas).Y);
            Canvas.SetLeft(text, Mouse.GetPosition(MainCanvas).X);
            MainCanvas.Children.Add(text);
        }

        private void DownloadButton_MouseDown( object sender, MouseButtonEventArgs e )
        {
        }

        private void MainCanvas_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!( sender is Canvas canvas )
             || !( e.OriginalSource is TextBox element ))
                return;
            if (canvas.Children.Contains(element))
                canvas.Children.Remove(element);
        }
    }
}