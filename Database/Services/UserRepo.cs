﻿using API_Complete_ASP.Models;
using API_Complete_ASP.Models.Dtos;

namespace API_Complete_ASP.Database.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly APICompleteContext _context;

        public UserRepo(APICompleteContext context)
        {
            _context = context;
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


    }
}