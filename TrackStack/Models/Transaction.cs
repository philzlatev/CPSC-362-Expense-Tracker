using System;

namespace TrackStack.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Merchant { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
