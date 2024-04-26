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

namespace HotelWPF.ViewModel.RoomModel
{
    public class RoomAddPageViewModel : ViewModelBase
    {
        private readonly HotelDataAccess hotel;

        public ICommand CancelCommand { get; }
        public ICommand AddRoomCommand { get; }

        private string roomNumber;
        public string RoomNumber
        {
            get => roomNumber;
            set
            {
                roomNumber = value;
                OnPropertyChanged(nameof(RoomNumber));
            }
        }

        private string floor;
        public string Floor
        {
            get => floor;
            set
            {
                floor = value;
                OnPropertyChanged(nameof(Floor));
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


        public RoomAddPageViewModel(NavigationStore navigationStore, HotelDataAccess hotel)
        {
            this.hotel = hotel;

            CancelCommand = new NavigateCommand(navigationStore, () => new RoomInfoPageViewModel(navigationStore, hotel));
            AddRoomCommand = new AddRoomCommand(this, hotel, navigationStore, () => new RoomInfoPageViewModel(navigationStore, hotel));

            roomTypes = new ObservableCollection<string>(hotel.GetRoomTypes().Select(e => e.Name));
        }
    }
}
