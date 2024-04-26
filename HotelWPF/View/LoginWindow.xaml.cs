using HotelWPF.DataAccess;
using HotelWPF.View;
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

namespace HotelWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            string connection = "data source=DESKTOP-VFNA9KJ;initial catalog=Hotel;User Id=" + username + ";Password=" + password + ";";
            if (LoginDataAccess.TryLogin(connection))
            {
                Window window;
                switch (LoginDataAccess.GetRole(connection))
                {
                    case UserRole.Manager:
                        window = new MainWindow(username, password);
                        break;
                    case UserRole.Receptionist:
                        window = new ReceptionistMainWindow(username, password);
                        break;
                    case UserRole.Accountant:
                        window = new AccountantMainWindow(username, password);
                        break;
                    default:
                        window = new MainWindow(username, password);
                        break;
                }
                window.Show();
                Close();
            }
        }
    }
}
