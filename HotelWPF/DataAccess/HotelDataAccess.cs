using HotelWPF.Model;
using HotelWPF.QueryOptions;
using HotelWPF.ViewModel.RoomModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class HotelDataAccess
    {
        private readonly string connectionString;
        private readonly RoomDataAccess roomDataAccess;
        private readonly RoomTypeDataAccess roomTypeDataAccess;
        private readonly ReservationDataAccess reservationDataAccess;
        private readonly DiscountDataAccess discountDataAccess;
        private readonly ServiceDataAccess serviceDataAccess;
        private readonly PaymentDataAccess paymentDataAccess;
        private readonly StaffDataAccess staffDataAccess;
        private readonly ExpenseDataAccess expenseDataAccess;
        public HotelDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
            roomDataAccess = new RoomDataAccess(connectionString);
            roomTypeDataAccess = new RoomTypeDataAccess(connectionString);
            reservationDataAccess = new ReservationDataAccess(connectionString);
            discountDataAccess = new DiscountDataAccess(connectionString);
            serviceDataAccess = new ServiceDataAccess(connectionString);
            paymentDataAccess = new PaymentDataAccess(connectionString);
            staffDataAccess = new StaffDataAccess(connectionString);
            expenseDataAccess = new ExpenseDataAccess(connectionString);
        }

        #region Rooms
        public List<Room> GetRooms()
        {
            return roomDataAccess.GetRooms();
        }

        public bool AddRoom(Room room)
        {
            return roomDataAccess.AddRoom(room);
        }

        public bool UpdateRoom(Room room)
        {
            return roomDataAccess.UpdateRoom(room);
        }

        public bool DeleteRoom(int id)
        {
            return roomDataAccess.DeleteRoom(id);
        }

        public List<Room> GetAvailableRooms(int exclude, DateOnly from, DateOnly to)
        {
            return roomDataAccess.GetAvailableRooms(exclude, from.ToDateTime(TimeOnly.MinValue), to.ToDateTime(TimeOnly.MinValue));
        }
        #endregion


        #region Reservations
        public List<Reservation> GetReservations(int page, int perPage)
        {
            return reservationDataAccess.GetReservations(page, perPage);
        }

        public List<Reservation> GetReservationsWithOptions(int page, int perPage, ReservationGetOptions options)
        {
            return reservationDataAccess.GetReservationsWithOptions(page, perPage, options);
        }

        public List<Reservation> GetFutureArrivals()
        {
            return reservationDataAccess.GetFutureArrivals();
        }

        public List<Reservation> GetFutureDepartures()
        {
            return reservationDataAccess.GetFutureDepartures();
        }

        public float GetReservationCost(int id)
        {
            return reservationDataAccess.GetReservationCost(id);
        }

        public List<Reservation> GetUnpaidReservations()
        {
            return reservationDataAccess.GetUnpaidReservations();
        }

        public bool AddReservation(Reservation reservation)
        {
            return reservationDataAccess.AddReservation(reservation);
        }

        public bool UpdateReservation(Reservation reservation)
        {
            return reservationDataAccess.UpdateReservation(reservation);
        }

        public bool DeleteReservation(int id)
        {
            return reservationDataAccess.DeleteReservation(id);
        }
        #endregion

        public List<RoomType> GetRoomTypes()
        {
            return roomTypeDataAccess.GetRoomTypes();
        }

        public int FindRoomTypeCountByName(string name)
        {
            return roomTypeDataAccess.FindRoomTypeCountByName(name);
        }

        public List<Discount> GetDiscounts()
        {
            return discountDataAccess.GetDiscounts();
        }

        public List<Service> GetServices()
        {
            return serviceDataAccess.GetServices();
        }

        public int FindServiceCountByName(string name)
        {
            return serviceDataAccess.FindServiceCountByName(name);
        }

        public List<Payment> GetPaymentsByPeriod(DateTime from, DateTime to)
        {
            return paymentDataAccess.GetPaymentsByPeriod(from, to);
        }

        public List<Payment> GetPaymentsByReservation(int id)
        {
            return paymentDataAccess.GetPaymentsByReservationId(id);
        }

        public void ExportPaymentData(DateTime from, DateTime to)
        {
            paymentDataAccess.ExportPaymentData(from, to);
        }

        public bool AddPayment(Payment payment)
        {
            return paymentDataAccess.AddPayment(payment);
        }

        public float GetTotalSalary()
        {
            return staffDataAccess.GetSalarySum();
        }

        public List<Expense> GetExpenses(DateTime from, DateTime to)
        {
            return expenseDataAccess.GetExpensesForPeriod(from, to);
        }
    }
}
