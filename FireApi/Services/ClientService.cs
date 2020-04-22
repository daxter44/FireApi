using FireApi.Database;
using FireApi.Database.Entity;
using FireApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetById(Guid id);
        Task<Client> Create(Client client);
        Task<Task> Update(Client client);
        Task<Task> Delete(Guid id);
        Task<Device> AddDevice(Guid clientId, Device device);
        Task<IEnumerable<Device>> GetDevices(Guid clientId);
    }
    public class ClientService : IClientService
    {
        DataContext _context;
        public ClientService(DataContext context)
        {
            _context = context;
        }
       
        public async Task<Client> Create(Client client)
        {
            if (client.User != null)
            {
                client.User.Role = Role.Client;
                await AddUser(client.User).ConfigureAwait(false);
            }
            if (client.Address != null)
            {
                await AddAddress(client.Address).ConfigureAwait(false);
            }
            _context.Client.Add(client);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return client;
        }
        public async Task<Task> Update(Client clientParam)
        {
            var client = await _context.Client.Include(a => a.Address).Include(a => a.User).FirstOrDefaultAsync(i => i.ClientId == clientParam.ClientId);

      
            if (client == null)
                throw new AppException("Client not found");

            if (client.User == null)
                throw new AppException("User not found");

            if (client.Address == null)
                throw new AppException("Address not found");

            // update username if it has changed


            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(clientParam.FirstName))
                client.FirstName = clientParam.FirstName;

            if (!string.IsNullOrWhiteSpace(clientParam.LastName))
                client.LastName = clientParam.LastName;

            if (!string.IsNullOrWhiteSpace(clientParam.User.EMail))
                client.User.EMail = clientParam.User.EMail;

            if (!string.IsNullOrWhiteSpace(clientParam.Address.City))
                client.Address.City = clientParam.Address.City;

            if (!string.IsNullOrWhiteSpace(clientParam.Address.Street))
                client.Address.Street = clientParam.Address.Street;

            if (!string.IsNullOrWhiteSpace(clientParam.Address.ZipCode))
                client.Address.ZipCode = clientParam.Address.ZipCode;

            if (!string.IsNullOrWhiteSpace(clientParam.Address.HouseNumber))
                client.Address.HouseNumber = clientParam.Address.HouseNumber;
            // update password if provided


            _context.Users.Update(client.User);
            _context.Client.Update(client);
            _context.Address.Update(client.Address);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public  async Task<Task> Delete(Guid id)
        {
            var client = await _context.Client.FindAsync(id);
            var user = await _context.Users.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Client.ToListAsync().ConfigureAwait(false);
        }
        public async Task<Client> GetById(Guid id)
        {
            return await _context.Client.Include(a => a.Address).Include(a => a.User).FirstOrDefaultAsync(i => i.ClientId == id).ConfigureAwait(false);
        }
        public async Task<IEnumerable<Device>> GetDevices(Guid clientId)
        {
            var client = await _context.Client
               .Include(a => a.Devices).Where(a => a.ClientId == clientId).FirstOrDefaultAsync().ConfigureAwait(false);
            return client.Devices;
        }
        public async Task<Device> AddDevice(Guid clientId, Device device)
        {
            var client = _context.Client.Find(clientId);
            _context.Device.Add(device);
            client.Devices.Add(device);
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return device;
        }
        private async Task<Task> AddUser(User user)
        {
            UserService userService = new UserService(_context);
            try
            {
                await userService.Create(user).ConfigureAwait(false);
                return Task.CompletedTask;
            }
            catch (AppException ex)
            {
                throw new AppException("User service:" + ex.Message);
            }
        }
        private async Task<Task> AddAddress(Address address)
        {
            try
            {
                _context.Address.Add(address);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Task.CompletedTask;
            }
            catch (AppException ex)
            {
                throw new AppException("Add address error:" + ex.Message);
            }}
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
