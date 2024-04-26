using HotelWPF.Store;
using HotelWPF.ViewModel;
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
using System.Windows.Shapes;

namespace HotelWPF.View
{
    /// <summary>
    /// Interaction logic for AccountantMainWindow.xaml
    /// </summary>
    public partial class AccountantMainWindow : Window
    {
        private readonly NavigationStore navigationStore;
        public AccountantMainWindow(string username, string password)
        {
            InitializeComponent();
            navigationStore = new NavigationStore();
            DataContext = new AccountantMainViewModel(navigationStore, username, password);
        }
    }
}
