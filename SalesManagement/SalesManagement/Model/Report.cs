namespace SalesManagement.Model
{
    public class Report
    {
        public  long ReportID { get; set; } 
        public long  PersonID { get; set; }
        public long ProductID { get; set; }
        public long AllocationID { get; set; }
        public int SoldQuanity { get; set; }
        public DateTime DateTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime  CreatedDate { get; set;}
        public DateTime ModifiedDate { get; set;}
    }
}
