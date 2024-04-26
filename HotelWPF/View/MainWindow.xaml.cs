using System.Text;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.ViewModel;
using HotelWPF.View;
using HotelWPF.Store;

namespace HotelWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationStore navigationStore;
        public MainWindow(string username, string password)
        {
            InitializeComponent();
            navigationStore = new NavigationStore();
            DataContext = new MainViewModel(navigationStore, username, password);
        }
    }
}