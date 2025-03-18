using System;
using Microsoft.AspNetCore.Mvc;
using PortRec.Common.Library.Models;
using UserService.src.Application.IUserService;
using VertexFin.Domain.DTOModels;
using VertexFin.Domain.Models;

namespace UserService.src.API.Controllers
{
    [Route("vertexfin/api/[controller]")]
    [ApiController]
    public class UserServiceController : ControllerBase
    {
        private readonly  IUserService _userService;

        public UserServiceController(IUserService userService) { 
        
            _userService = userService;
        }

        [HttpGet(nameof(GetUserdetails))]
        public async Task<IActionResult> GetUserdetails(Guid Id)
        {
            APIResponse<User> response;
            try
            {
                var obj = await _userService.GetUserDetails(Id);
                response = new APIResponse<User>(true, obj);

            }
            catch (Exception ex)
            {
                response = new APIResponse<User>(false, new string[] { ex.Message });

            }
            return Ok(response);
        }


        [HttpPost(nameof(InsertNewUser))]
        public async Task<IActionResult> InsertNewUser(UserDto user)
        {
            APIResponse<string> response;
            try
            {
                var obj = await _userService.InsertNewCustomer(user);
                response = new APIResponse<string>(true, obj);

            }
            catch (Exception ex)
            {
                response = new APIResponse<string>(false, new string[] { ex.Message });

            }
            return Ok(response);
        }

        [HttpGet(nameof(LoginUser))]
        public async Task<IActionResult> LoginUser(UserDto user)
        {
            APIResponse<User> response;
            try
            {
                var obj = await _userService.LoginUser(user);
                response = new APIResponse<User>(true, obj);

            }
            catch (Exception ex)
            {
                response = new APIResponse<User>(false, new string[] { ex.Message });

            }
            return Ok(response);
        }
    }
}