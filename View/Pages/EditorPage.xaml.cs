using System.Windows.Controls;
using MemeRepo = Core.DatabaseTools;


namespace View.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class EditorPage : Page
    {
        private MemeRepo Repo { get; set; }
        public EditorPage(MemeRepo repo)
        {
            InitializeComponent();
            Repo = repo;
        }
    }
}
