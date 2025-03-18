using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedRepository.IRepository;
using System;
using VertexFin.Common.Library.Helper;
using VertexFin.Domain.DTOModels;
using VertexFin.Domain.Models;

namespace UserService.src.Application.UserService
{
    public class UserServices : IUserService.IUserService
    {
        private readonly IRepository _repository;

        public UserServices(IRepository repository) {
            _repository = repository;
        }
        public async  Task<User>  GetUserDetails(Guid id)
        {
           return await _repository.Get<Guid, User>(id);

            
        }

        public async Task<string> InsertNewCustomer(UserDto userDto)
        {
            bool usernameExists = await _repository.ExistsAsync(u => u.Username == userDto.UserName);
            //bool emailExists = await _repository.ExistsAsync(u => u.Email == userDto.);

            if (usernameExists)
                return "Username is already taken.";

            //if (emailExists)
            //    return "Email is already registered.";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.UserName,
                
                PasswordHash = PasswordHasher.HashPassword(userDto.Password),
            };

            int result = await _repository.Insert<User>(user);
            return "Username is created  "; // Return the user if save was successful, otherwise null
        }
    }
}