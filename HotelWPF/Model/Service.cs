using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Service
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public float Cost { get; }
        public Service(int id, string name, string description, float cost)
        {
            Id = id;
            Name = name;
            Description = description;
            Cost = cost;
        }
    }
}
