using System.Windows.Controls;
using System.Windows.Input;
using Meme = Core.Image;

namespace View.Pages
{
    public partial class MemeInfoPage : Page
    {
        private Meme Meme { get; set; }
        private Frame MainFrame { get; set; }
        public MemeInfoPage(Meme meme, Frame frame)
        {
            InitializeComponent();
            Meme = meme;
            DataContext = Meme;
            MainFrame = frame;
        }

        private void ReturnButton_MouseDown( object sender, MouseButtonEventArgs e )
        {
            MainFrame.GoBack();
        }

        private void MemeTitle_MouseDown( object sender, MouseButtonEventArgs e )
        {
            ( sender as TextBlock ).IsEnabled = true;
        }
    }
}