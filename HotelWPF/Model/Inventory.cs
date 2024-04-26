﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Model
{
    public class Inventory
    {
        public int Id {  get; }
        public string Name { get; }
        public string Description { get; }
        public Inventory(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
