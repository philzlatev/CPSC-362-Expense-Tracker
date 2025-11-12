namespace TrackStack.Models
{
    public class Expenses
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }

        public string Description { get; set; }

        public Expenses()
        {
            
        }
    }
}
