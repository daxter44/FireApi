using FireApi.Data;
using FireApi.Entity;
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
        Task<Task> Update(Client client, string password);
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
            UserService userService = new UserService(_context);
            try
            {
                client.User.Role = Role.Client;
                client.User = await userService.Create(client.User).ConfigureAwait(false);
            }catch (AppException ex)
            {
               throw new AppException("User service:" + ex.Message);
            }           

            _context.Client.Add(client);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return client;
        }

        public async Task<Task> Update(Client clientParam, string password = null)
        {
            var client = await _context.Client.FindAsync(clientParam.ClientId);

            var user = await _context.Users.FindAsync(clientParam.ClientId);

            if (client == null)
                throw new AppException("Client not found");

            // update username if it has changed
           

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(clientParam.FirstName))
                client.FirstName = clientParam.FirstName;

            if (!string.IsNullOrWhiteSpace(clientParam.LastName))
                client.LastName = clientParam.LastName;

            if (!string.IsNullOrWhiteSpace(clientParam.User.EMail))
                user.EMail = clientParam.User.EMail;
            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                client.User.PasswordHash = passwordHash;
                client.User.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.Client.Update(client);
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
            return await _context.Client.FindAsync(id).ConfigureAwait(false);
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
            _context.DeviceItems.Add(device);
            client.Devices.Add(device);
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return device;
        }
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
