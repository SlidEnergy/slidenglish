﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SlidEnglish.Domain;

namespace SlidEnglish.App
{
    public interface IUsersService
    {
        Task<User> GetById(string userId);
        Task<TokensCortage> Login(string email, string password);
        Task<IdentityResult> Register(User user, string password);
    }
}