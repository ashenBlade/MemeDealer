using System.Windows.Controls;
using System.Windows.Input;
using Core;

namespace View.Pages
{
    public partial class MemeInfoPage : Page
    {
        private MemeRepository Repo { get; set; }
        private EditorPage EditorPage { get; set; }
        private Meme Meme { get; set; }
        private Frame MainFrame { get; set; }
        public MemeInfoPage(Meme meme, Frame frame, EditorPage editorPage)
        {
            InitializeComponent();
            Meme = meme;
            MainFrame = frame;
            EditorPage = editorPage;
            DataContext = Meme;
        }

        private void ReturnButton_MouseDown( object sender, MouseButtonEventArgs e )
        {
            MainFrame.GoBack();
        }

        private void EditorButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EditorPage.SetBackgroundImage(Meme);
            MainFrame.Navigate(EditorPage);
        }
    }
}