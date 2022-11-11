using System;

namespace BugTrackerUI.Data
{
   public class SalesByYear
   {
      public DateTime ShippedDate { get; set; }
      public int OrderID { get; set; }
      public decimal Subtotal { get; set; }
      //public int Year { get; set; }              // DATENAME ( datepart , date ): This function returns a character string representing the specified datepart of the specified date.
      public string Year { get; set; }
   }
}
