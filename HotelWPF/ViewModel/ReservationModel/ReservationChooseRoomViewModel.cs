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
    public class ReservationChooseRoomViewModel : ViewModelBase
    {
        private HotelDataAccess hotel;

        private ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            set
            {
                rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        private Room selectedRoom;
        public Room SelectedRoom
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private DateTime dateFrom = DateTime.Now;
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
                FindAvailableRooms();
            }
        }
        private DateTime dateTo = DateTime.Now.AddDays(7);
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                dateTo = value;
                OnPropertyChanged(nameof(DateTo));
                FindAvailableRooms();
            }
        }


        public ICommand AdditionalInfoNavigate { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand GoNextCommand { get; }
        public ICommand GoBackCommand { get; }
        public ReservationChooseRoomViewModel(NavigationStore navigationStore, HotelDataAccess hotel)
        {
            this.hotel = hotel;
            FindAvailableRooms();

            AdditionalInfoNavigate = new NavigateCommand(navigationStore, () => new ReservationAdditionalInfoViewModel(navigationStore, hotel, this));
            SelectItemCommand = new RelayCommand(SelectItem);
            GoNextCommand = new RelayCommand(GoNext, CanGoNext);
            GoBackCommand = new NavigateCommand(navigationStore, () => new ReservationInfoPageViewModel(navigationStore, hotel));
        }

        private void FindAvailableRooms()
        {
            Rooms.Clear();
            if (DateFrom > DateTo)
                return;

            hotel.GetAvailableRooms(0, DateOnly.FromDateTime(DateFrom), DateOnly.FromDateTime(DateTo)).ForEach(Rooms.Add);
        }

        private void SelectItem(object parameter)
        {
            var LastClickedItem = parameter as Room;
            SelectedRoom = LastClickedItem;
        }

        private void GoNext(object parameter)
        {
            AdditionalInfoNavigate.Execute(null);
        }

        private bool CanGoNext(object parameter)
        {
            return SelectedRoom != null;
        }
    }
}
