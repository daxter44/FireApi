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
    public interface IFirmService
    {
        Task<IEnumerable<Firm>> GetAll();
        Task<Firm> GetById(Guid id);
        Task<Firm> Create(Firm firm);
        Task<Task> Update(Firm firm, string password);
        Task<Task> Delete(Guid id);
        Task<Client> AddClient(Guid firmId, Client client);
        Task<IEnumerable<Client>> GetClients(Guid firmId);
        Task<IEnumerable<Device>> GetDevices(Guid firmId);
    }
    public class FirmService : IFirmService
    {
        DataContext _context;
        public FirmService(DataContext context)
        {
            _context = context;
        }
       
        public async Task<Firm> Create(Firm firm)
        {
            if (firm.User != null)
            {
                firm.User.Role = Role.Firm;
                await AddUser(firm.User).ConfigureAwait(true);
            }
            if (firm.Address != null)
            {
                await AddAddress(firm.Address).ConfigureAwait(false);
            }
            _context.Firm.Add(firm);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return firm;
        }

        public async Task<Task> Update(Firm firmParam, string password = null)
        {
            var firm = await _context.Firm.Include(a => a.Address).Include(a => a.User).FirstOrDefaultAsync(i => i.FirmId == firmParam.FirmId).ConfigureAwait(false);

            if (firm == null)
                throw new AppException("Client not found");

            if (firm.User == null)
                throw new AppException("User not found");

            if (firm.Address == null)
                throw new AppException("Address not found");

            // update username if it has changed


            // update user properties if provided
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(firmParam.Name))
                firm.Name = firmParam.Name;

            if (!string.IsNullOrWhiteSpace(firmParam.User.EMail))
                firm.User.EMail = firmParam.User.EMail;

            if (!string.IsNullOrWhiteSpace(firmParam.Address.City))
                firm.Address.City = firmParam.Address.City;

            if (!string.IsNullOrWhiteSpace(firmParam.Address.Street))
                firm.Address.Street = firmParam.Address.Street;

            if (!string.IsNullOrWhiteSpace(firmParam.Address.ZipCode))
                firm.Address.ZipCode = firmParam.Address.ZipCode;

            if (!string.IsNullOrWhiteSpace(firmParam.Address.HouseNumber))
                firm.Address.HouseNumber = firmParam.Address.HouseNumber;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                firm.User.PasswordHash = passwordHash;
                firm.User.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(firm.User);
            _context.Firm.Update(firm);
            _context.Address.Update(firm.Address);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public  async Task<Task> Delete(Guid id)
        {
            var Firm = await _context.Firm.FindAsync(id);
            if (Firm != null)
            {
                _context.Firm.Remove(Firm);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Firm>> GetAll()
        {
            return await _context.Firm.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Firm> GetById(Guid id)
        {
            return await _context.Firm.Include(a => a.Address).Include(a => a.User).FirstOrDefaultAsync(i => i.FirmId == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Client>> GetClients(Guid FirmId)
        {
            var clients = _context.Client
               .Include(a => a.Address).Include(a => a.User).Where(a => a.FirmId == FirmId);
            return clients;
        }
        public async Task<Client> AddClient(Guid FirmId, Client client)
        {
            client.FirmId = FirmId;
            if (client.User != null)
            {
                client.User.Role = Role.Client;
                AddUser(client.User);
            }
            _context.Client.Add(client);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return client;
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
            }
        }
        public async Task<IEnumerable<Device>> GetDevices(Guid firmId)
        {
            List<Device> devicesList = new List<Device>();
            var FirmList = _context.Firm
              .Include(a => a.Clients).Where(a => a.FirmId == firmId).FirstOrDefault();
            foreach(Client i in FirmList.Clients)
            {
               var client = await _context.Client
               .Include(a => a.Devices).Where(a => a.ClientId == i.ClientId).FirstOrDefaultAsync().ConfigureAwait(false);
                devicesList.AddRange(client.Devices);
            }
            return devicesList;
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
