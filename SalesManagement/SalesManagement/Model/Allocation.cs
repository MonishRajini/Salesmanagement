namespace SalesManagement.Model
{
    public class Allocation
    {
        public long AllocationID { get; set; }
        public long PersonID { get; set; }
        public long ProductID { get; set; }

        public int Quantity { get; set; }
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
