using API_Complete_ASP.Models;

namespace API_Complete_ASP.Database.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly APICompleteContext _context;

        public UserRepo(APICompleteContext context)
        {
            _context = context;
        }

        // USUARIO ---------------------------------------------------------------------------------- //
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.IdUser = _context.SaveChanges();

            return user;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int? id)
        {
            return _context.Users.FirstOrDefault(u => u.IdUser == id);
        }

        public User GetByPass(string password)
        {
            return _context.Users.FirstOrDefault(u => u.Password == password);
        }


        // CONTACTO ---------------------------------------------------------------------------------- //
        public Contact Create(Contact contact)
        {
            _context.Contacts.Add(contact);
            contact.IdContact = _context.SaveChanges();

            return contact;
        }

        // DATOS OPCIONALES ---------------------------------------------------------------------------------- //

        public DataUser Create(DataUser dataUser)
        {
            _context.DataUsers.Add(dataUser);
            dataUser.IdData = _context.SaveChanges();

            return dataUser;
        }
    }
}
