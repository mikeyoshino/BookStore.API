using BookStore_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Contract
{
    public interface IAuthenticationRepository
    {

        public Task<bool> Register(RegistrationModel user);
    }
}
