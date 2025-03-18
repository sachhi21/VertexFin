using Microsoft.AspNetCore.Mvc;
using System;
using VertexFin.Domain.DTOModels;
using VertexFin.Domain.Models;
namespace UserService.src.Application.IUserService
{
    public interface IUserService
    {
        Task<User> GetUserDetails(Guid id);
        Task<string> InsertNewCustomer(UserDto user);
    }
}