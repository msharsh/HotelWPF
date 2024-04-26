using HotelWPF.DataAccess;
using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelWPF.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        HotelDataAccess hotel;
        private ObservableCollection<Reservation> futureArrivals = new ObservableCollection<Reservation>();
        public ObservableCollection<Reservation> FutureArrivals
        {
            get => futureArrivals;
            set
            {
                futureArrivals = value;
                OnPropertyChanged(nameof(FutureArrivals));
            }
        }
        private ObservableCollection<Reservation> futureDepartures = new ObservableCollection<Reservation>();
        public ObservableCollection<Reservation> FutureDepartures
        {
            get => futureDepartures;
            set
            {
                futureDepartures = value;
                OnPropertyChanged(nameof(FutureDepartures));
            }
        }
        private ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> AvailableRooms
        {
            get => rooms;
            set
            {
                rooms = value;
                OnPropertyChanged(nameof(AvailableRooms));
            }
        }
        public HomeViewModel(HotelDataAccess hotel)
        {
            this.hotel = hotel;
            FindAvailableRooms();
            FindFutureArrivals();
            FindFutureDepartures();
        }

        private void FindAvailableRooms()
        {
            AvailableRooms.Clear();

            hotel.GetAvailableRooms(0, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7))).ForEach(AvailableRooms.Add);
        }

        private void FindFutureArrivals()
        {
            FutureArrivals.Clear();

            hotel.GetFutureArrivals().ForEach(FutureArrivals.Add);
        }

        private void FindFutureDepartures()
        {
            FutureDepartures.Clear();

            hotel.GetFutureDepartures().ForEach(FutureDepartures.Add);
        }
    }
}
