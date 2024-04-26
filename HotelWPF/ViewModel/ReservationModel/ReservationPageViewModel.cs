using HotelWPF.DataAccess;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.ViewModel.ReservationModel
{
    public class ReservationPageViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentReservationViewModel => navigationStore.CurrentViewModel;
        public ReservationPageViewModel(HotelDataAccess hotel)
        {
            navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new ReservationInfoPageViewModel(navigationStore, hotel);
            navigationStore.CurrentViewModelChanged += OnCurrentReservationViewModelChanged;
        }

        private void OnCurrentReservationViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentReservationViewModel));
        }
    }
}
