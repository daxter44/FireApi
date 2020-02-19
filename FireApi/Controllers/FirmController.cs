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

namespace FireApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FirmController : ControllerBase
    {
        private IFirmService _firmService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FirmController(
            IFirmService firmService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _firmService = firmService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize(Roles = Role.Admin)]
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateFirmModel model)
        {
            // map model to entity
            var firm = _mapper.Map<Firm>(model);
            var user = _mapper.Map<User>(model.registerModel);
            firm.User = user;

            try
            {
                // create user
                await _firmService.Create(firm).ConfigureAwait(false);
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
        public async Task<ActionResult<Client>> DeleteClient(UpdateFirmModel model)
        {
            try
            {
                await _firmService.Delete(model.FirmId).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]UpdateFirmModel model)
        {
            // map model to entity and set id
            var firm = _mapper.Map<Firm>(model);

            try
            {
                // update user 
                await _firmService.Update(firm, model.Password).ConfigureAwait(false);
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
        public async Task<IActionResult> GetById([FromBody]FirmModel postModel)
        {

            var firm = await _firmService.GetById(postModel.FirmId).ConfigureAwait(false);
            var model = _mapper.Map<FirmModel>(firm);
            return Ok(model);
        }

        [Authorize(Roles = Role.Firm)]
        [HttpPost("addClient")]
        public async Task<ActionResult<Client>> AddClientItem([FromBody]AddClientModel model)
        {
            // map model to entity
            var client = _mapper.Map<Client>(model);
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();
            try
            {
                // create device
                await _firmService.AddClient(currentUserId, client).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Firm)]
        [HttpGet("myClients")]
        public async Task<IActionResult> GetClientsByUserId()
        {
            // only allow users show myDevices
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();

            try
            {
                var clients = await _firmService.GetClient(currentUserId).ConfigureAwait(false);
                var modelToReturn = _mapper.Map<IList<ClientModel>>(clients);
                return Ok(modelToReturn);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Firm)]
        [HttpGet("devices")]
        public async Task<IActionResult> GetDevicesByFirmId()
        {
            // only allow users show myDevices
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();

            try
            {
                var devices = await _firmService.GetDevices(currentUserId).ConfigureAwait(false);
                var modelToReturn = _mapper.Map<IList<DeviceModel>>(devices);
                //zwraca info o urządzeniach oraz o klientach, a następnie znowu o urządzeniach tych klientów, należy okroić aby zwracał podstawowe info o urządzeniach i podst o klientach.
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
