using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Payment
    {
        public int Id { get; }
        public int ReservationId { get; }
        public float AmountPaid { get; }
        public DateOnly PaymentDate { get; }
        public string PaymentMethod { get; }
        public Payment(int id, int reservationId, float amountPaid, DateOnly paymentDate, string paymentMethod)
        {
            Id = id;
            ReservationId = reservationId;
            AmountPaid = amountPaid;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
        }
    }
}
