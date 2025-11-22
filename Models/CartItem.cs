namespace EcommerceApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int? UserId { get; set; } // Nullable for guest carts if we persist them, though we might use Session for guests.
        // Actually, for simplicity, let's assume this is for persistent carts or we just use Session for now.
        // The plan mentioned CartItem model, so I'll add it.

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        
        public string SessionId { get; set; } = string.Empty; // To link to session if not logged in
    }
}
