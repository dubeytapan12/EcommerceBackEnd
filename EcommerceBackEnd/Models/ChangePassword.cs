namespace EcommerceBackEnd.Models
{
    public class ChangePassword
    {
        public string email { get; set; }
        public string currentPassword { get; set; }
        public string NewtPassword { get; set; }
    }
}
