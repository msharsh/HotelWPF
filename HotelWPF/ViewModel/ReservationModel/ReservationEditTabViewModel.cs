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
    public class ReservationEditTabViewModel : ViewModelBase
    {
        private ReservationInfoPageViewModel parentVM;
        private readonly HotelDataAccess hotel;

        private Reservation selected;
        public Reservation Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }


        // Edit Fields
        private string selectedRoom;
        private string currentRoom;
        public string SelectedRoom
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }
        private ObservableCollection<string> availableRooms = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableRooms
        {
            get => availableRooms;
            set
            {
                availableRooms = value;
                OnPropertyChanged(nameof(AvailableRooms));
            }
        }
        private DateTime checkInDate = DateTime.Now;
        public DateTime CheckInDate
        {
            get => checkInDate;
            set
            {
                checkInDate = value;
                OnPropertyChanged(nameof(CheckInDate));
                FindAvailableRooms();
            }
        }
        private DateTime checkOutDate = DateTime.Now.AddDays(7);
        public DateTime CheckOutDate
        {
            get => checkOutDate;
            set
            {
                checkOutDate = value;
                OnPropertyChanged(nameof(CheckOutDate));
                FindAvailableRooms();
            }
        }
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


        // Discount And Service
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

        public ICommand EnableInfoCommand { get; }
        public ICommand SubmitEditCommand { get; }


        public ICommand AddServiceCommand { get; }
        public ICommand AddDiscountCommand { get; }
        public ICommand DeleteDiscountCommand { get; }
        public ICommand DeleteServiceCommand { get; }

        public ReservationEditTabViewModel(NavigationStore navigationStore, ReservationInfoPageViewModel parentVM, HotelDataAccess hotel)
        {
            this.parentVM = parentVM;
            this.hotel = hotel;
            parentVM.PropertyChanged += OnParentViewModelPropertyChanged;
            Selected = parentVM.SelectedReservation;
            currentRoom = Selected.Room.RoomNumber;

            CheckInDate = Selected.CheckInDate.ToDateTime(TimeOnly.MinValue);
            CheckOutDate = Selected.CheckOutDate.ToDateTime(TimeOnly.MinValue);
            GuestName = Selected.GuestName;
            PhoneNumber = Selected.PhoneNumber;

            FindAvailableRooms();

            // Populate Discount and Service 
            hotel.GetDiscounts().Select(e => e.Name).ToList().ForEach(AvailableDiscountList.Add);
            hotel.GetServices().Select(e => e.Name).ToList().ForEach(AvailableServiceList.Add);
            var discounts = Selected.AppliedDiscounts.Select(e => e.Name).ToList();
            discounts.ForEach(DiscountList.Add);
            discounts.ForEach(e => AvailableDiscountList.Remove(e));
            var services = Selected.ReservedServices.Select(e => e.Name).ToList();
            services.ForEach(ServiceList.Add);
            services.ForEach(e => AvailableServiceList.Remove(e));

            // Commands
            EnableInfoCommand = new NavigateCommand(navigationStore, () => new ReservationInfoTabViewModel(navigationStore, parentVM, hotel));
            SubmitEditCommand = new RelayCommand(SubmitEdit, CanSubmitEdit);
            AddDiscountCommand = new RelayCommand(AddDiscount, CanAddDiscount);
            DeleteDiscountCommand = new RelayCommand(DeleteDiscount);
            AddServiceCommand = new RelayCommand(AddService, CanAddService);
            DeleteServiceCommand = new RelayCommand(DeleteService);
        }

        private void OnParentViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(parentVM.SelectedReservation))
            {
                if (parentVM.SelectedReservation != null)
                    EnableInfoCommand.Execute(null);
            }
        }

        private void FindAvailableRooms()
        {
            SelectedRoom = currentRoom;
            AvailableRooms.Clear();
            var rooms = hotel.GetAvailableRooms(Selected.Id, DateOnly.FromDateTime(CheckInDate), DateOnly.FromDateTime(CheckOutDate));
            rooms.ForEach(e => AvailableRooms.Add(e.RoomNumber));

        }

        private void SubmitEdit(object parameter)
        {
            Room room = hotel.GetRooms().FirstOrDefault(e => e.RoomNumber == SelectedRoom);
            var services = hotel.GetServices().Where(e => ServiceList.Contains(e.Name)).ToList();
            var discounts = hotel.GetDiscounts().Where(e => DiscountList.Contains(e.Name)).ToList();

            if (hotel.UpdateReservation(new Reservation(
                Selected.Id,
                room, GuestName, PhoneNumber,
                DateOnly.FromDateTime(CheckInDate), DateOnly.FromDateTime(CheckOutDate),
                "Paid", new List<Payment>(),
                services, discounts
            )))
            {
                parentVM.LoadData();
                EnableInfoCommand.Execute(null);
            }
        }

        private bool CanSubmitEdit(object parameter)
        {
            if (SelectedRoom == string.Empty) return false;

            Room room = hotel.GetRooms().FirstOrDefault(e => e.RoomNumber == SelectedRoom);
            return Reservation.Validate(room, GuestName, PhoneNumber, DateOnly.FromDateTime(CheckInDate), DateOnly.FromDateTime(CheckOutDate));
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
    }
}
