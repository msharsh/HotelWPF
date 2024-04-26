using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Rate
    {
        public int Id { get; }
        public string Name { get; }
        public float Multiplier { get; }

        public Rate (int id, string name, float multiplier)
        {
            Id = id;
            Name = name;
            Multiplier = multiplier;
        }
    }
}
