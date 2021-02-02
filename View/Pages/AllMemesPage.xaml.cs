using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Image = System.Windows.Controls.Image;
using Meme = Core.Image;
using MemeRepo = Core.DatabaseTools;
namespace View.Pages
{
    /// <summary>
    /// Interaction logic for AllMemesPage.xaml
    /// </summary>
    public partial class AllMemesPage : Page
    {
        public ObservableCollection<Meme> Memes { get; set; }
        public Meme SelectedMeme { get; set; }
        private MemeRepo Repo { get; set; }
        private Frame MainFrame { get; set; }

        public AllMemesPage(MemeRepo repo, Frame mainFrame)
        {
            InitializeComponent();
            // InitializeMemes();
            Repo = repo;
            Memes = new ObservableCollection<Meme>();
            foreach (var meme in Repo.FindByTags(""))
                Memes.Add(meme);
            AllMemesList.ItemsSource = Memes;
            MainFrame = mainFrame;
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
            var image = ((((sender as Border).Child as Grid).Children[0] as Border).Child as Image).DataContext as Core.Image;
            MainFrame.Navigate(new MemeInfoPage(image, MainFrame));
        }
    }
}
