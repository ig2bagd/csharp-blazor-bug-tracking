using System;

namespace BugTrackerUI.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal OrderTotal { get; set; }
    }
}