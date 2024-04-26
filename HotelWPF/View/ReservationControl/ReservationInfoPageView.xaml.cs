using HotelWPF.ViewModel.RoomModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelWPF.View.ReservationControl
{
    /// <summary>
    /// Interaction logic for ReservationInfoPageView.xaml
    /// </summary>
    public partial class ReservationInfoPageView : UserControl
    {
        public ReservationInfoPageView()
        {
            InitializeComponent();
        }

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader? headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                string? header = headerClicked.Column.Header as string;
                if (header != null)
                {
                    (DataContext as RoomInfoPageViewModel)?.SortCommand.Execute(header);
                }
            }
        }
    }
}
