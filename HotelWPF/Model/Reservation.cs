using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Reservation
    {
        public int Id { get; }
        public Room Room { get; }
        public string GuestName { get; }
        public string PhoneNumber { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
        public string PaymentStatus { get; }
        public List<Payment> Payments { get; }
        public List<Service> ReservedServices { get; }
        public List<Discount> AppliedDiscounts { get; }
        public Reservation(int id, Room room, string guestName, string phoneNumber, DateOnly checkInDate, DateOnly checkOutDate, string paymentStatus, List<Payment> payments, List<Service> reservedServices, List<Discount> appliedDiscounts)
        {
            Id = id;
            Room = room;
            GuestName = guestName;
            PhoneNumber = phoneNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            PaymentStatus = paymentStatus;
            Payments = payments;
            ReservedServices = reservedServices;
            AppliedDiscounts = appliedDiscounts;
        }
        public static bool Validate(Room room, string guestName, string phoneNumber, DateOnly checkIn, DateOnly checkOut)
        {
            return room != null && !string.IsNullOrEmpty(guestName) && !string.IsNullOrEmpty(phoneNumber) && checkIn <= checkOut;
        }
    }
}
