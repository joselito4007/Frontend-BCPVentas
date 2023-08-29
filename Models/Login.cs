namespace Frontend_BCPVentas.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Pwd { get; set; }
        public class Access
        {
            public string token { get; set; }
        }
    }
}
