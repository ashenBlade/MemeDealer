using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Core;
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;
using MemeRepo = Core.DatabaseTools;
using Meme = Core.Image;
namespace View.Pages
{
    public partial class DownloadPage : Page
    {
        private MemeRepo Repo { get; set; }

        private Meme Meme { get; set; }

        public DownloadPage(MemeRepo repo)
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
            if (string.IsNullOrEmpty(Meme.PathToFile))
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
            Meme.PathToFile = ( MainImage.Source as BitmapImage).UriSource.AbsoluteUri;
            Repo.Add(Meme);
            MessageBox.Show("Мем успешно добавлен");
            ResetPage();
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