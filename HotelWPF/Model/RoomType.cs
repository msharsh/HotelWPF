using HotelWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class RoomType
    {
        public int Id { get; }
        public string Name { get; }
        public int Capacity { get; }
        public string Description { get; }
        public float StandartRate { get; }
        public Rate Rate { get; }
        public Dictionary<Inventory, int> Inventory { get; }
        public RoomType(int id, string name, int capacity, string description, float standartRate, Rate rate, Dictionary<Inventory, int> inventory)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
            Description = description;
            StandartRate = standartRate;
            Rate = rate;
            Inventory = inventory;
        }
    }
}
