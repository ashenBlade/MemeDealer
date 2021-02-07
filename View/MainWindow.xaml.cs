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
using Core;
using View.Pages;


namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemeRepository Repo { get; set; }
        private AllMemesPage AllMemesPage { get; set; }
        private EditorPage EditorPage { get; set; }
        private DownloadPage DownloadPage { get; set; }
        private Page CurrentPage { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Contains all images
            Repo = new MemeRepository();

            // Initialize main pages
            EditorPage = new EditorPage(Repo);
            DownloadPage = new DownloadPage(Repo);
            AllMemesPage = new AllMemesPage(Repo, MainFrame, EditorPage);


            // Start page
            CurrentPage = AllMemesPage;
            MainFrame.Navigate(CurrentPage);
        }


        private void AllMemesButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = AllMemesPage;
            AllMemesPage.ShowImagesWithTags("");
            MainFrame.Navigate(CurrentPage);
        }

        // Update all memes with every new letter inserted
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AllMemesPage.ShowImagesWithTags(SearchBar.Text);
            CurrentPage = AllMemesPage;
            MainFrame.Navigate(CurrentPage);
        }

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = EditorPage;
            MainFrame.Navigate(CurrentPage);
        }

        private void DownloadButton_OnClick( object sender, RoutedEventArgs e )
        {
            CurrentPage = DownloadPage;
            MainFrame.Navigate(CurrentPage);
        }
    }
}