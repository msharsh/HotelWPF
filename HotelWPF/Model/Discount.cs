using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Discount
    {
        public int Id { get; }
        public string Name { get; }
        public float Value { get; }
        public string Description { get; }
        public Discount(int id, string name, float value, string description)
        {
            Id = id;
            Name = name;
            Value = value;
            Description = description;
        }
    }
}
