using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using HotelWPF.View.RoomControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelWPF.ViewModel.RoomModel
{
    public class RoomInfoPageViewModel : ViewModelBase
    {
        HotelDataAccess hotel;


        private ObservableCollection<RoomViewModel> rooms;
        public ObservableCollection<RoomViewModel> Rooms
        {
            get => rooms;
            set
            {
                rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }


        private RoomViewModel? _selectedRoom = null;
        public RoomViewModel? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                
                if (value != null)
                {
                    CurrentTab = new RoomInfoTab();
                    SetUIList();
                    EditedRoom = new RoomViewModel(new Room(SelectedRoom.Id, SelectedRoom.RoomNumber, int.Parse(SelectedRoom.Floor), SelectedRoom.RoomType, SelectedRoom.Status));
                    SelectedRoomType = SelectedRoom.RoomType.Name;
                    SelectedRoomMaintenance = SelectedRoom.Status == "Under Maintenance";
                }
                InfoTabVisibilty = value == null ? Visibility.Collapsed : Visibility.Visible;
                NullInfoTabVisibilty = value != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private string inventoryList;
        public string InventoryList
        {
            get => inventoryList;
            set
            {
                inventoryList = value;
                OnPropertyChanged(nameof(InventoryList));
            }
        }

        private RoomViewModel? editedRoom = null;
        public RoomViewModel EditedRoom
        {
            get => editedRoom;
            set
            {
                editedRoom = value;
                OnPropertyChanged(nameof(EditedRoom));
                editedRoom.PropertyChanged += EditedRoom_PropertyChanged; ;
            }
        }
        private bool selectedRoomMaintenance;
        public bool SelectedRoomMaintenance
        {
            get => selectedRoomMaintenance;
            set
            {
                selectedRoomMaintenance = value;
                OnPropertyChanged(nameof(SelectedRoomMaintenance));
                if (CurrentTab is RoomInfoTab)
                    SelectedCurrentRoomMaintenance = value;
            }
        }
        public bool SelectedCurrentRoomMaintenance { get; private set; }

        private void EditedRoom_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(EditedRoom));
        }


        // Right Tab
        private Visibility _infoTabVisibilty = Visibility.Collapsed;
        public Visibility InfoTabVisibilty
        {
            get => _infoTabVisibilty;
            set
            {
                _infoTabVisibilty = value;
                OnPropertyChanged(nameof(InfoTabVisibilty));
            }
        }
        private Visibility _nullInfoTabVisibilty = Visibility.Visible;
        public Visibility NullInfoTabVisibilty
        {
            get => _nullInfoTabVisibilty;
            set
            {
                _nullInfoTabVisibilty = value;
                OnPropertyChanged(nameof(NullInfoTabVisibilty));
            }
        }

        private UserControl _currentTab = new RoomInfoTab();
        public UserControl CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged(nameof(CurrentTab));
            }
        }

        // Filters
        private bool isAvailable;
        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        private int filterFloor = 0;
        public int FilterFloor
        {
            get => filterFloor;
            set
            {
                filterFloor = value;
                OnPropertyChanged(nameof(FilterFloor));
            }
        }

        private int filterCapacity = 0;
        public int FilterCapacity
        {
            get => filterCapacity;
            set
            {
                filterCapacity = value;
                OnPropertyChanged(nameof(FilterCapacity));
            }
        }


        public IEnumerable<string> RoomTypes => roomTypes;
        private readonly ObservableCollection<string> roomTypes;
        private string selectedRoomType;
        public string SelectedRoomType
        {
            get => selectedRoomType;
            set
            {
                selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
            }
        }


        public ICommand NavigateAddRoomCommand { get; }
        public ICommand EnableEditCommand { get; }
        public ICommand SubmitEditCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        public ICommand EnableInfoCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand SortCommand { get; }


        public RoomInfoPageViewModel(NavigationStore navigationStore, HotelDataAccess hotel)
        {
            rooms = new ObservableCollection<RoomViewModel>();
            this.hotel = hotel;
            LoadData();
            roomTypes = new ObservableCollection<string>(hotel.GetRoomTypes().Select(e => e.Name));

            NavigateAddRoomCommand = new NavigateCommand(navigationStore, () => new RoomAddPageViewModel(navigationStore, hotel));
            EnableEditCommand = new RelayCommand(ShowEditTab);
            SubmitEditCommand = new EditRoomCommand(this, hotel, ShowInfoTab);
            EnableInfoCommand = new RelayCommand(ShowInfoTab);
            DeleteRoomCommand = new RelayCommand(DeleteRoom);
            ApplyFilterCommand = new RelayCommand(FilterRooms);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            SortCommand = new RelayCommand(Sort);
        }

        public void LoadData()
        {
            Rooms.Clear();
            hotel.GetRooms().ForEach(e => rooms.Add(new RoomViewModel(e)));
        }


        private void SetUIList()
        {
            if (SelectedRoom.RoomType.Inventory.Count == 0)
                InventoryList = "None";
            else
            {
                string str = "";
                foreach (var pair in SelectedRoom.RoomType.Inventory)
                {
                    if (pair.Value == 1)
                    {
                        str += pair.Key.Name + ", ";
                    }
                    else
                    {
                        str += pair.Key.Name + " x" + pair.Value + ", ";
                    }
                }
                str = str.Substring(0, str.Length - 2);
                InventoryList = str;
            }
        }


        public void ShowInfoTab(object parameter)
        {
            CurrentTab = new RoomInfoTab();
        }

        public void ShowEditTab(object parameter)
        {
            CurrentTab = new RoomEditTab();
        }

        public void DeleteRoom(object parameter)
        {
            if (hotel.DeleteRoom(SelectedRoom.Id))
            {
                Rooms.Remove(SelectedRoom);
            }
        }

        private void FilterRooms(object parameter)
        {
            LoadData();
            List<RoomViewModel> filterRooms = Rooms.ToList();
            if (FilterFloor != 0)
                filterRooms = filterRooms.Where(e => int.Parse(e.Floor).Equals(FilterFloor)).ToList();
            if (FilterCapacity != 0)
                filterRooms = filterRooms.Where(e => e.RoomType.Capacity >= FilterCapacity).ToList();

            if (IsAvailable)
            {
                var available = hotel.GetAvailableRooms(0, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));
                var tempList = new List<RoomViewModel>();
                foreach (var room in filterRooms)
                {
                    if (available.Where(e => e.Id == room.Id).Any())
                    {
                        tempList.Add(room);
                    }
                }
                filterRooms = tempList;
            }

            Rooms.Clear();
            foreach (var room in filterRooms)
            {
                Rooms.Add(room);
            }
        }

        private void ResetFilter(object parameter)
        {
            LoadData();
        }

        private void Sort(object parameter)
        {
            string? column = parameter as string;
            List<RoomViewModel> sorted = new List<RoomViewModel>();
            switch (column)
            {
                case "Room Number":
                    sorted = Rooms.OrderBy(e => e.RoomNumber).ToList();
                    break;
                case "Floor":
                    sorted = Rooms.OrderBy(e => e.Floor).ToList();
                    break;
                case "Status":
                    sorted = Rooms.OrderBy(e => e.Status).ToList();
                    break;
                case "Room Type":
                    sorted = Rooms.OrderBy(e => e.RoomType.Name).ToList();
                    break;
                case "Capacity":
                    sorted = Rooms.OrderBy(e => e.RoomType.Capacity).ToList();
                    break;

            }

            Rooms.Clear();
            sorted.ForEach(Rooms.Add);
        }
    }
}
