using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Core;
using Microsoft.Win32;
using Core;

namespace View.Pages
{
    public partial class DownloadPage : Page
    {
        private MemeRepository Repo { get; set; }

        private Meme Meme { get; set; }

        public DownloadPage(MemeRepository repo)
        {
            InitializeComponent();
            Repo = repo;
            Meme = new Meme();
            DataContext = Meme;
        }


        private bool HasCorrectFormat( string filepath )
        {
            return filepath.EndsWith(".png")
                || filepath.EndsWith(".jpg")
                || filepath.EndsWith(".jpeg")
                || filepath.EndsWith(".bmp");
        }
        private void Image_MouseDown( object sender, MouseButtonEventArgs e )
        {
            var fileOpenDialog = new OpenFileDialog();
            if (fileOpenDialog.ShowDialog() == false)
                return;
            if (!HasCorrectFormat(fileOpenDialog.FileName))
            {
                MessageBox.Show("Поддерживаемые форматы изображений:\n.png, .jpeg, .jpg, .bmp");
                return;
            }
            Meme.PathToFile = fileOpenDialog.FileName;
            ( sender as Grid ).Background = new SolidColorBrush(Colors.Transparent);
            MainImage.Source = new BitmapImage(new Uri(Meme.PathToFile, UriKind.Absolute));
        }

        private void SaveButton_MouseDown( object sender, MouseButtonEventArgs e )
        {
            if (string.IsNullOrWhiteSpace(Meme.PathToFile))
            {
                MessageBox.Show("Добавьте изображение");
                return;
            }

            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                MessageBox.Show("Добавьте имя");
                return;
            }

            Meme.Name = Title.Text;
            Meme.Tags = TagsBlock.Text;
            Repo.Add(Meme);
            Repo.SaveChanges();
            ResetPage();
            MessageBox.Show("Мем успешно добавлен");
        }

        private void ResetPage()
        {
            Title.Text = string.Empty;
            TagsBlock.Text = string.Empty;
            MainImage.Source = new BitmapImage();
            Meme = new Meme();
        }
    }
}