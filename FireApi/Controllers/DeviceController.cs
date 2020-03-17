using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FireApi.Services;
using AutoMapper;
using FireApi.Helpers;
using Microsoft.Extensions.Options;
using FireApi.Entity;
using FireApi.Models.Device;

namespace FireApi.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private IDeviceService _deviceService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public DeviceController(
             IDeviceService userService,
             IMapper mapper,
             IOptions<AppSettings> appSettings)
        {
            _deviceService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        // GET: api/Device

        [Authorize(Roles = Role.Firm + ", " + Role.Client)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDevices()
        {
            // 
            var devices = await _deviceService.GetAll().ConfigureAwait(false);
            var model = _mapper.Map<IList<DeviceModel>>(devices);
            return Ok(model);
        }

        // GET: api/Device/5

        [Authorize(Roles = Role.Firm + ", " + Role.Client)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDeviceItem(Guid id)
        {             
            var deviceItem = await _deviceService.GetById(id).ConfigureAwait(false);
            if (deviceItem == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<DeviceModel>(deviceItem);
            return Ok(model);
        }

        // PUT: api/Device/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceItem(Guid id, [FromBody]UpdateDeviceModel model)
        {
            var device = _mapper.Map<Device>(model);
            if (id != device.ID)
            {
                device.ID = id;
            }
           
            try
            {
                // update user 
                await _deviceService.Update(device).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Device
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        [HttpPost]
        public async Task<ActionResult<Device>> AddDeviceItem( [FromBody]AddDeviceModel model)
        {
            // map model to entity
            var device = _mapper.Map<Device>(model);          

            try
            {
                // create device
                await _deviceService.AddDevice(model.Id, device).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
               
        // DELETE: api/Device/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Device>> DeleteDeviceItem(Guid id)
        {
            try
            {
                // create device
                await _deviceService.Delete(id).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

       

    }
}
