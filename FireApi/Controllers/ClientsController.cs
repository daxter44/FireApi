using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FireApi.Data;
using FireApi.Entity;
using FireApi.Services;
using AutoMapper;
using FireApi.Helpers;
using Microsoft.Extensions.Options;
using FireApi.Models.Client;
using Microsoft.AspNetCore.Authorization;
using FireApi.Models.Device;
using FireApi.Models.Firm;
using FireApi.Models.Users;

namespace FireApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ClientsController(
            IUserService userService,
            IClientService clientService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _clientService = clientService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
                
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateClientModel model)
        {
            // map model to entity
            var client = _mapper.Map<Client>(model);
            var user = _mapper.Map<User>(model.registerModel);
            client.User = user;

            try
            {
                // create user
                await _clientService.Create(client).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = Role.Firm)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(Guid id)
        {
            try
            {
                await _clientService.Delete(id).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Firm)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]UpdateClientModel model)
        {
            // map model to entity and set id
            var client = _mapper.Map<Client>(model);
            
            if (id != client.ClientId)
            {
                client.ClientId = id;
            }
            try
            {
                // update user 
                await _clientService.Update(client, null).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Firm)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var client = await _clientService.GetById(id).ConfigureAwait(false);
            var user = await _userService.GetById(id).ConfigureAwait(false);
            var modelClient = _mapper.Map<ClientModel>(client);
            var modelUser = _mapper.Map<UserModel>(user);
            modelClient.User = modelUser;
            return Ok(modelClient);
        }

        [Authorize(Roles = Role.Firm)]
        [HttpPost("addDevice")]
        public async Task<ActionResult<Device>> AddDeviceItem([FromBody]AddDeviceModel model)
        {
            // map model to entity
            var device = _mapper.Map<Device>(model);

            try
            {
                // create device
                await _clientService.AddDevice(model.Id, device).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("myDevices")]
        public async Task<IActionResult> GetDevicesByUserId()
        {
            // only allow users show myDevices
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();

            try
            {
                var devices = await _clientService.GetDevices(currentUserId).ConfigureAwait(false);
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
