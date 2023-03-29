namespace SalesManagement.Model
{
    public class TopAchievers
    {
        public long PersonID { get; set; }
        public long ProductID { get; set; }
        public string PersonName { get; set; }

        public int SoldQuantity { get; set; }
        public DateTime  Date { get; set; }
    }
}
