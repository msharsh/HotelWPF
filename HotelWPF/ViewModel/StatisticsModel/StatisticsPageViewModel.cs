using HotelWPF.DataAccess;
using HotelWPF.Store;
using HotelWPF.ViewModel.ReservationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.ViewModel.StatisticsModel
{
    public class StatisticsPageViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentStatisticViewModel => navigationStore.CurrentViewModel;
        public StatisticsPageViewModel(HotelDataAccess hotel)
        {
            navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new StatisticsInfoPageViewModel(navigationStore, hotel);
            navigationStore.CurrentViewModelChanged += OnCurrentStatisticViewModelChanged;
        }

        private void OnCurrentStatisticViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentStatisticViewModel));
        }
    }
}
