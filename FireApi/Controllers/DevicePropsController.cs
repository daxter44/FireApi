using System;
using System.Threading.Tasks;
using FireApi.Database.Entity;
using FireApi.Database.Repository;
using FireApi.Helpers;
using FireApi.Models.Device;
using FireApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FireApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DevicePropsController : ControllerBase
    {
        private IDevicePropService _deviceService;

        public DevicePropsController(IDevicePropService deviceService)
        {
            _deviceService = deviceService;
        }

        //[HttpPost("addProps")]
        //public async Task<IActionResult> AddProps()
        //{
        //    //var device = new DeviceSettings()
        //    //{
        //    //    DeviceId = Guid.NewGuid(),
        //    //    Type = "furnance"
        //    //};
        //    var testDevice = DeviceProp.FromJson(System.IO.File.ReadAllText("data.json"));
        //    try
        //    {
        //        await _settingsRepository.InsertOneAsync(testDevice).ConfigureAwait(false);
        //        return Ok();
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        [HttpGet("getProps/{deviceId}")]
        public async Task<IActionResult> GetPropsData(Guid deviceId)
        {
            var deviceProps = await _deviceService.GetLastPropAwait(deviceId).ConfigureAwait(false);
            return Ok(Serialize.ToJson(deviceProps));
        }
        [HttpGet("ChangeProp")]
        public async Task<IActionResult> ChangeProp(Device device)
        {
            return Ok();
        }
    }
}
