using System.Windows.Controls;
using WpfApp3.ViewModel;

namespace WpfApp3.View
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl
    {
      
        public ContentView()
        {


            InitializeComponent();
            ContentViewModel contentViewModel = new();
            DataContext = contentViewModel;

        }

    }
}
