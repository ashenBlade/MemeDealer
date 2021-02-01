using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MemeRepo = Core.DatabaseTools;
using Meme = Core.Image;

namespace View
{
    /// <summary>
    /// Interaction logic for AllMemesPage.xaml
    /// </summary>
    public partial class AllMemesPage : Page
    {
        private ObservableCollection<Meme> Memes { get; set; }
        private Meme SelectedMeme { get; set; }

        public AllMemesPage()
        {
            InitializeComponent();
            InitializeMemes();
            AllMemesList.ItemsSource = Memes;
        }

        private void InitializeMemes()
        {
            Memes = new ObservableCollection<Meme>();
            Memes.Add(new Meme() { Name = "Meme 1", PathToFile = "../Assets/hashtag.png" });
            Memes.Add(new Meme() { Name = "Meme 2", PathToFile = "../Assets/search.png" });
            Memes.Add(new Meme() { Name = "Meme 3", PathToFile = "../Assets/menu.png" });
        }

        private void AllMemesList_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = new Meme();
            var image = ((((sender as Border).Child as Grid).Children[0] as Border).Child as Image).DataContext as Core.Image;
            SelectedMeme = image;
        }
    }
}
