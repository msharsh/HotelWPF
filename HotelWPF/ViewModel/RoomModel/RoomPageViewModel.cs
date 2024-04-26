using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelWPF.ViewModel.RoomModel
{
    public class RoomPageViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentRoomViewModel => navigationStore.CurrentViewModel;
        public RoomPageViewModel(HotelDataAccess hotel)
        {
            navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new RoomInfoPageViewModel(navigationStore, hotel);
            navigationStore.CurrentViewModelChanged += OnCurrentRoomViewModelChanged;
        }

        private void OnCurrentRoomViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentRoomViewModel));
        }
    }
}
