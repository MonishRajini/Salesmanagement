using System.Globalization;

namespace SalesManagement.Model
{
    public class SalesPerson
    {
        public long PersonID { get; set; }
        public string PersonName { get; set; }
        public string city { get;  set; }
        public string state { get; set; }
        public long PhoneNumber { get; set; }
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
