using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Store;
using HotelWPF.ViewModel.ReservationModel;
using HotelWPF.ViewModel.RoomModel;
using HotelWPF.ViewModel.StatisticsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateRoomCommand { get; }
        public ICommand NavigateReservationCommand { get; }
        public ICommand NavigateStatisticsCommand { get; }


        public MainViewModel(NavigationStore navigationStore, string username, string password)
        {
            HotelDataAccess hotelDataAccess = new HotelDataAccess("data source=DESKTOP-VFNA9KJ;initial catalog=Hotel;User Id=" + username + ";Password=" +  password + ";");
            NavigateHomeCommand = new NavigateCommand(navigationStore, () => new HomeViewModel(hotelDataAccess));
            NavigateRoomCommand = new NavigateCommand(navigationStore, () => new RoomPageViewModel(hotelDataAccess));
            NavigateReservationCommand = new NavigateCommand(navigationStore, () => new ReservationPageViewModel(hotelDataAccess));
            NavigateStatisticsCommand = new NavigateCommand(navigationStore, () => new StatisticsPageViewModel(hotelDataAccess));


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
