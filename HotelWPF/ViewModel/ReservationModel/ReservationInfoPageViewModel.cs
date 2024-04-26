using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.QueryOptions;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelWPF.ViewModel.ReservationModel
{
    public class ReservationInfoPageViewModel : ViewModelBase
    {
        private readonly HotelDataAccess hotel;


        // Tab Navigation
        private NavigationStore navigationTabStore;
        public ViewModelBase CurrentTabViewModel => navigationTabStore.CurrentViewModel;


        // Reservations
        private ObservableCollection<Reservation> reservations;
        public ObservableCollection<Reservation> Reservations
        {
            get => reservations;
            set
            {
                reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        private Reservation selectedReservation;
        public Reservation SelectedReservation
        {
            get => selectedReservation;
            set
            {
                selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }


        private int pageNumber = 1;
        private int maxPage = 1000;
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value > 0 && value <= maxPage)
                {
                    pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                }
            }
        }


        // Filter
        private DateTime filterDate = DateTime.Now;
        public DateTime FilterDate
        {
            get => filterDate;
            set
            {
                filterDate = value;
                OnPropertyChanged(nameof(FilterDate));
            }
        }
        private string filterName;
        public string FilterName
        {
            get => filterName;
            set
            {
                filterName = value;
                OnPropertyChanged(nameof(FilterName));
            }
        }
        private bool hasPaid;
        public bool Unpaid
        {
            get => hasPaid;
            set
            {
                hasPaid = value;
                OnPropertyChanged(nameof(Unpaid));
            }
        }
        private bool enableFilterDate = false;
        public bool EnableFilterDate
        {
            get => enableFilterDate;
            set
            {
                enableFilterDate = value;
                OnPropertyChanged(nameof(EnableFilterDate));
            }
        }
        ReservationGetOptions options;


        public ICommand AddReservationCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand SortCommand { get; }

        public ReservationInfoPageViewModel(NavigationStore navigationStore, HotelDataAccess hotel)
        {
            this.hotel = hotel;
            Reservations = new ObservableCollection<Reservation>();
            options = new ReservationGetOptions();
            LoadData();

            navigationTabStore = new NavigationStore();
            navigationTabStore.CurrentViewModel = new ReservationInfoTabViewModel(navigationTabStore, this, hotel);
            navigationTabStore.CurrentViewModelChanged += OnCurrentTabViewModelChanged;


            AddReservationCommand = new NavigateCommand(navigationStore, () => new ReservationChooseRoomViewModel(navigationStore, hotel));
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ApplyFilterCommand = new RelayCommand(FilterReservations);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            SortCommand = new RelayCommand(Sort);
        }

        private void OnCurrentTabViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentTabViewModel));
        }

        public void LoadData()
        {
            var reservations = hotel.GetReservationsWithOptions(PageNumber, 20, options);
            Reservations.Clear();
            reservations.ForEach(Reservations.Add);
        }

        public void DeleteReservation()
        {
            if (SelectedReservation == null) return;

            if (hotel.DeleteReservation(SelectedReservation.Id))
            {
                Reservations.Remove(SelectedReservation);
            }
        }


        private void Sort(object parameter)
        {
            
        }


        private void FilterReservations(object parameter)
        {
            options.ShowOnlyUnpaid = Unpaid;
            options.FilterByDate = EnableFilterDate;
            options.FilterDate = FilterDate;
            options.NameFilter = FilterName;
            options.EnableSort = false;
            options.SortColumnName = string.Empty;

            PageNumber = 1;
            LoadData();
        }

        private void ResetFilter(object parameter)
        {
            options = new ReservationGetOptions();
            LoadData();
        }

        private void NextPage(object parameter)
        {
            PageNumber++;
            LoadData();
        }

        private void PreviousPage(object parameter)
        {
            PageNumber--;
            LoadData();
        }
    }
}
