using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Room
    {
        public int Id { get; }
        public string RoomNumber { get; }
        public int Floor { get; }
        public RoomType RoomType { get; }
        public string Status { get; }
        public float Rate => GetRate();
        public Room(int id, string roomNumber, int floor, RoomType roomType, string status)
        {
            Id = id;
            RoomNumber = roomNumber;
            Floor = floor;
            RoomType = roomType;
            Status = status;
        }
        public float GetRate()
        {
            return RoomType.StandartRate * RoomType.Rate.Multiplier;
        }
        public static bool Validate(string roomNumber, string floor, RoomType? roomType)
        {
            int _floor;
            if (int.TryParse(floor, out _floor))
            {
                return !string.IsNullOrWhiteSpace(roomNumber) &&
                    _floor > 0 &&
                    roomType != null;
            }
            return false;
        }
    }
}
