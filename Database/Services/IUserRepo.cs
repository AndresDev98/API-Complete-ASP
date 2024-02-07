using API_Complete_ASP.Models;

namespace API_Complete_ASP.Database.Services
{
    public interface IUserRepo
    {
        User Create(User user);
        User GetByEmail(string email); 
        User GetById(int? id);

        Contact Create(Contact contact);
    }
}
