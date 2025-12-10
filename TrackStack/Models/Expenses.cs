using Microsoft.AspNetCore.Identity;

namespace TrackStack.Models
{
    public class Expenses
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = string.Empty;

        // IMPORTANT: IdentityUser.Id is string, so UserId must be string
        public string? UserId { get; set; }

        // Navigation to Identity user (not your custom TrackStack.Models.User)
        public IdentityUser? User { get; set; }
    }
}