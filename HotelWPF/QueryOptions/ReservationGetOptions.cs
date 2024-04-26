using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.QueryOptions
{
    public class ReservationGetOptions
    {
        public bool ShowOnlyUnpaid { get; set; }
        public bool FilterByDate { get; set; }
        public DateTime FilterDate { get; set; }
        public string? NameFilter { get; set; }
        public bool EnableSort { get; set; }
        public string? SortColumnName { get; set; }
        public ReservationGetOptions()
        {
            ShowOnlyUnpaid = false;
            FilterByDate = false;
            FilterDate = DateTime.Now;
            NameFilter = string.Empty;
            EnableSort = false;
            SortColumnName = "ReservationId";
        }
    }
}
