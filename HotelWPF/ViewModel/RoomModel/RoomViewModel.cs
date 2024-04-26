using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.ViewModel.RoomModel
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly Room room;

        public int Id => room.Id;

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

        public RoomType RoomType => room.RoomType;
        public float Rate => room.GetRate();
        public string Status => room.Status;

        public RoomViewModel(Room room)
        {
            this.room = room;

            roomNumber = room.RoomNumber;
            floor = room.Floor.ToString();
        }
    }
}
