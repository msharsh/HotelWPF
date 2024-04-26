using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Expense
    {
        public float Value { get; }
        public DateTime Date { get; }
        public Expense(float value, DateTime date)
        {
            Value = value;
            Date = date;
        }
    }
}
