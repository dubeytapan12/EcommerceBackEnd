namespace EcommerceBackEnd.Models
{
    public class CardWishListResponse
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
