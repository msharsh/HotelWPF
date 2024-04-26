using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Store;
using HotelWPF.ViewModel.ReservationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelWPF.ViewModel
{
    public class ReceptionistMainViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateReservationCommand { get; }


        public ReceptionistMainViewModel(NavigationStore navigationStore, string username, string password)
        {
            HotelDataAccess hotelDataAccess = new HotelDataAccess("data source=DESKTOP-VFNA9KJ;initial catalog=Hotel;User Id=" + username + ";Password=" + password + ";");
            NavigateHomeCommand = new NavigateCommand(navigationStore, () => new HomeViewModel(hotelDataAccess));
            NavigateReservationCommand = new NavigateCommand(navigationStore, () => new ReservationPageViewModel(hotelDataAccess));


            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            NavigateHomeCommand.Execute(null);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
