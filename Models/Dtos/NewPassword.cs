namespace API_Complete_ASP.Models.Dtos
{
    public class NewPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int? Id { get; set; }
    }
}
