using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerUI.Data
{
   [Table("Employees")]
   public class Employee
   {
      public int EmployeeID { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Title { get; set; }
   }
}
