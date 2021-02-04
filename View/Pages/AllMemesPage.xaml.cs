using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Image = System.Windows.Controls.Image;
using Core;

namespace View.Pages
{
    /// <summary>
    /// Interaction logic for AllMemesPage.xaml
    /// </summary>
    public partial class AllMemesPage : Page
    {
        public ObservableCollection<Meme> Memes { get; set; }
        public Meme SelectedMeme { get; set; }
        private EditorPage EditorPage { get; set; }
        private MemeRepository Repo { get; set; }
        private Frame MainFrame { get; set; }

        public AllMemesPage(MemeRepository repo, Frame mainFrame, EditorPage editorPage)
        {
            InitializeComponent();
            // InitializeMemes();
            Repo = repo;
            Memes = new ObservableCollection<Meme>();
            foreach (var meme in Repo.GetAllMemes())
                Memes.Add(meme);
            AllMemesList.ItemsSource = Memes;
            MainFrame = mainFrame;
            EditorPage = editorPage;
        }

        public void ShowImagesWithTags( string tags )
        {
            Memes.Clear();
            foreach (var meme in Repo.FindByTags(tags))
            {
                Memes.Add(meme);
            }
        }


        private void AllMemesList_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = ((((sender as Border).Child as Grid).Children[0] as Border).Child as Image).DataContext as Meme;
            MainFrame.Navigate(new MemeInfoPage(image, MainFrame, EditorPage, Repo));
        }
    }
}
