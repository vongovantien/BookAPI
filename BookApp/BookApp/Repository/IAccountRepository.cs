using BookApp.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUp(SignUpModel signUpModel);
        Task<string> Login(SignInModel signInModel);
    }
}
