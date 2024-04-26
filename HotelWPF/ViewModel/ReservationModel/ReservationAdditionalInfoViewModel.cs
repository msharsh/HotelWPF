using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelWPF.ViewModel.ReservationModel
{
    public class ReservationAdditionalInfoViewModel : ViewModelBase
    {
        private readonly HotelDataAccess hotel;
        private Room selectedRoom;


        private string guestName;
        public string GuestName
        {
            get => guestName;
            set
            {
                guestName = value;
                OnPropertyChanged(nameof(GuestName));
            }
        }
        private string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private DateTime checkInDate;
        private DateTime checkOutDate;
        //Items
        private ObservableCollection<string> discountList = new ObservableCollection<string>();
        public ObservableCollection<string> DiscountList
        {
            get => discountList;
            set
            {
                discountList = value;
                OnPropertyChanged(nameof(DiscountList));
            }
        }
        private ObservableCollection<string> serviceList = new ObservableCollection<string>();
        public ObservableCollection<string> ServiceList
        {
            get => serviceList;
            set
            {
                serviceList = value;
                OnPropertyChanged(nameof(ServiceList));
            }
        }
        // ComboBox
        private ObservableCollection<string> availableDiscountList = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableDiscountList
        {
            get => availableDiscountList;
            set
            {
                availableDiscountList = value;
                OnPropertyChanged(nameof(AvailableDiscountList));
            }
        }
        private ObservableCollection<string> availableServiceList = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableServiceList
        {
            get => availableServiceList;
            set
            {
                availableServiceList = value;
                OnPropertyChanged(nameof(AvailableServiceList));
            }
        }
        private string selectedDiscount;
        public string SelectedDiscount
        {
            get => selectedDiscount;
            set
            {
                selectedDiscount = value;
                OnPropertyChanged(nameof(SelectedDiscount));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        private string selectedService;
        public string SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
                CommandManager.InvalidateRequerySuggested();
            }
        }


        public ICommand AddServiceCommand { get; }
        public ICommand AddDiscountCommand { get; }
        public ICommand DeleteDiscountCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand ReservationInfoNavigate { get; }
        public ICommand SubmitReservationCommand { get; }

        

        public ReservationAdditionalInfoViewModel(NavigationStore navigationStore, HotelDataAccess hotel, ReservationChooseRoomViewModel vm)
        {
            this.hotel = hotel;
            selectedRoom = vm.SelectedRoom;
            checkInDate = vm.DateFrom;
            checkOutDate = vm.DateTo;
            hotel.GetDiscounts().Select(e => e.Name).ToList().ForEach(AvailableDiscountList.Add);
            hotel.GetServices().Select(e => e.Name).ToList().ForEach(AvailableServiceList.Add);

            AddDiscountCommand = new RelayCommand(AddDiscount, CanAddDiscount);
            DeleteDiscountCommand = new RelayCommand(DeleteDiscount);
            AddServiceCommand = new RelayCommand(AddService, CanAddService);
            DeleteServiceCommand = new RelayCommand(DeleteService);
            ReservationInfoNavigate = new NavigateCommand(navigationStore, () => new ReservationInfoPageViewModel(navigationStore, hotel));
            SubmitReservationCommand = new RelayCommand(SubmitReservation, CanSubmitReservation);
        }

        private void AddDiscount(object parameter)
        {
            DiscountList.Add(SelectedDiscount);
            AvailableDiscountList.Remove(SelectedDiscount);
        }

        private bool CanAddDiscount(object parameter)
        {
            return SelectedDiscount != null;
        }

        private void DeleteDiscount(object parameter)
        {
            var deleted = parameter as string;
            AvailableDiscountList.Add(deleted);
            DiscountList.Remove(deleted);
        }

        private void AddService(object parameter)
        {
            ServiceList.Add(SelectedService);
            AvailableServiceList.Remove(SelectedService);
        }

        private bool CanAddService(object parameter)
        {
            return SelectedService != null;
        }

        private void DeleteService(object parameter)
        {
            var deleted = parameter as string;
            AvailableServiceList.Add(deleted);
            ServiceList.Remove(deleted);
        }

        private void SubmitReservation(object parameter)
        {
            var selectedDiscounts = hotel.GetDiscounts().Where(e => DiscountList.Contains(e.Name)).ToList();
            var selectedServices = hotel.GetServices().Where(e => ServiceList.Contains(e.Name)).ToList();
            if (hotel.AddReservation(new Reservation(
                0, selectedRoom,
                GuestName,
                PhoneNumber,
                DateOnly.FromDateTime(checkInDate),
                DateOnly.FromDateTime(checkOutDate),
                "Not Paid",
                new List<Payment>(),
                selectedServices,
                selectedDiscounts
                )))
            {
                ReservationInfoNavigate.Execute(null);
            }
        }

        private bool CanSubmitReservation(object parameter)
        {
            return Reservation.Validate(selectedRoom, GuestName, PhoneNumber, DateOnly.FromDateTime(checkInDate), DateOnly.FromDateTime(checkOutDate));
        }
    }
}
