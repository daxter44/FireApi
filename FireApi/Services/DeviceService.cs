using FireApi.Data;
using FireApi.Entity;
using FireApi.Helpers;
using FireApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Services
{
    public interface IDeviceService
    {
        Task<Device> AddDevice(int userId, Device device);
        Task<IEnumerable<Device>> GetAll();
        Task<Device> GetById(Guid id);
        Task<Task> Update(Device device);
        Task<Task>  Delete(Guid id);
    }
    public class DeviceService : IDeviceService 
    {

        private DataContext _context;

        public DeviceService(DataContext context)
        {
            _context = context;
        }

        public async Task<Device> AddDevice(int userid, Device deviceItem)
        {
            var user = _context.Users.Find(userid);
            user.Devices.Add(deviceItem);
            _context.Entry(user).State = EntityState.Modified;
            _context.DeviceItems.Add(deviceItem);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return deviceItem;
        }


        public async Task<Task> Delete(Guid id)
        {
            var device = _context.DeviceItems.Find(id);
            if (device != null)
            {
                _context.DeviceItems.Remove(device);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Task.CompletedTask;
            } else
            {
                throw new AppException("Device not found");
            }
        }

        public async Task<IEnumerable<Device>> GetAll()
        {
            return await _context.DeviceItems.ToListAsync().ConfigureAwait(false);
        }

        public  async Task<Device> GetById(Guid id)
        {
            var deviceItem = await _context.DeviceItems.FindAsync(id);
            return deviceItem;

        }

        public async Task<Task> Update(Device deviceParam)
        {
            var device = _context.DeviceItems.Find(deviceParam.ID);

            if (device == null)
                throw new AppException("Device not found");

            if (!string.IsNullOrWhiteSpace(deviceParam.Name))
                device.Name = deviceParam.Name;

                device.Temperature = deviceParam.Temperature;

            try
            {
                _context.DeviceItems.Update(device);
                await  _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new AppException(e.Message);
               
            }
            return Task.CompletedTask;
        }

        private async Task<bool> DeviceItemExists(Guid id)
        {
            return await _context.DeviceItems.AnyAsync(e => e.ID == id).ConfigureAwait(false);
        }
    }
}
