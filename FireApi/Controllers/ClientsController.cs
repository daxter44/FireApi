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

namespace FireApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ClientsController(
            IClientService clientService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _clientService = clientService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
                
        [AllowAnonymous]
        [HttpPost("createClient")]
        public async Task<IActionResult> Create([FromBody]CreateClientModel model)
        {
            // map model to entity
            var client = _mapper.Map<Client>(model);
            var user = _mapper.Map<User>(model.registerModel);
            client.User = user;

            try
            {
                // create user
                await _clientService.Create(client, model.registerModel.Password).ConfigureAwait(false);
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
        public async Task<ActionResult<Client>> DeleteClient(UpdateClientModel model)
        {
            try
            {
                await _clientService.Delete(model.ClientId).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]UpdateClientModel model)
        {
            // map model to entity and set id
            var client = _mapper.Map<Client>(model);

            try
            {
                // update user 
                await _clientService.Update(client, model.Password).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPost("getById")]
        public async Task<IActionResult> GetById([FromBody]ClientModel postModel)
        {

            var client = await _clientService.GetById(postModel.ClientId).ConfigureAwait(false);
            var model = _mapper.Map<ClientModel>(client);
            return Ok(model);
        }
       
        [HttpPost("addDevice")]
        public async Task<ActionResult<Device>> AddDeviceItem([FromBody]AddDeviceModel model)
        {
            // map model to entity
            var device = _mapper.Map<Device>(model);

            try
            {
                // create device
                await _clientService.AddDevice(model.UserId, device).ConfigureAwait(false);
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
