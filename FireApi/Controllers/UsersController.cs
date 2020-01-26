using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FireApi.Entity;
using FireApi.Helpers;
using FireApi.Models;
using FireApi.Models.Device;
using FireApi.Models.Users;
using FireApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static FireApi.Services.UserService;

namespace FireApi.Controllers
{

    [Authorize(Roles = Role.Admin)]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

       
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);


            try
            {
                // create user
                await _userService.Create(user, model.Password).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll().ConfigureAwait(false);
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("getById")]
        public async Task<IActionResult> GetById([FromBody]UserModel postModel)
        {

            var user = await _userService.GetById(postModel.Id).ConfigureAwait(false);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);

            try
            {
                // update user 
                await _userService.Update(user, model.Password).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("delete")]
        public async Task<IActionResult> Delete([FromBody]UpdateModel model)
        {
            try
            {
                await _userService.Delete(model.Id).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("addDevice")]
        public async Task<ActionResult<Device>> AddDeviceItem([FromBody]AddDeviceModel model)
        {
            // map model to entity
            var device = _mapper.Map<Device>(model);

            try
            {
                // create device
                await _userService.AddDevice(model.UserId, device).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("myDevices")]
        public async Task<IActionResult> GetDevicesByUserId()
        {
            // only allow users show myDevices
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();

            try
            {
                var devices = await _userService.GetDevices(currentUserId).ConfigureAwait(false);
                var modelToReturn = _mapper.Map<IList<DeviceModel>>(devices);
                return Ok(modelToReturn);               
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}