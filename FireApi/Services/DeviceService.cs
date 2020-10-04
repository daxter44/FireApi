using FireApi.Database;
using FireApi.Database.Entity;
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
        Task<Device> AddDevice(Guid userId, Device device);
        Task<IEnumerable<Device>> GetAll();
        Task<Device> GetById(Guid id);
        Task<Task> Update(Device device);
        Task<Task>  Delete(Guid id);
        Task<Device> GetDetails(Guid id);
    }
    public class DeviceService : IDeviceService 
    {

        private DataContext _context;

        public DeviceService(DataContext context)
        {
            _context = context;
        }

        public async Task<Device> AddDevice(Guid clientId, Device deviceItem)
        {
            var client = _context.Client.Find(clientId);
            deviceItem.Client = client;
            deviceItem.ID = Guid.Empty;
            client.Devices.Add(deviceItem);
            _context.Entry(client).State = EntityState.Modified;
            _context.Device.Add(deviceItem);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return deviceItem;
        }


        public async Task<Task> Delete(Guid id)
        {
            var device = _context.Device.Find(id);
            if (device != null)
            {
                _context.Device.Remove(device);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Task.CompletedTask;
            } else
            {
                throw new AppException("Device not found");
            }
        }

        public async Task<IEnumerable<Device>> GetAll()
        {
            return await _context.Device.ToListAsync().ConfigureAwait(false);
        }

        public  async Task<Device> GetById(Guid id)
        {
            var deviceItem = await _context.Device.Include(a => a.Status).FirstOrDefaultAsync().ConfigureAwait(false);
            return deviceItem;

        }

        public async Task<Task> Update(Device deviceParam)
        {
            var device = _context.Device.Find(deviceParam.ID);

            if (device == null)
                throw new AppException("Device not found");

            if (!string.IsNullOrWhiteSpace(deviceParam.Name))
                device.Name = deviceParam.Name;

            if (!string.IsNullOrWhiteSpace(deviceParam.Model))
                device.Model = deviceParam.Model;

            if (!string.IsNullOrWhiteSpace(deviceParam.SerialNumber))
                device.SerialNumber = deviceParam.SerialNumber;

                device.InstalationDate = deviceParam.InstalationDate;

            try
            {
                _context.Device.Update(device);
                await  _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new AppException(e.Message);
               
            }
            return Task.CompletedTask;
        }

        public async Task<Device> GetDetails(Guid id)
        {
            var deviceStatus = await _context.Device.Include(a => a.Status).FirstOrDefaultAsync().ConfigureAwait(false);
            return deviceStatus;
        }
        private async Task<bool> DeviceItemExists(Guid id)
        {
            return await _context.Device.AnyAsync(e => e.ID == id).ConfigureAwait(false);
        }
    }
}
