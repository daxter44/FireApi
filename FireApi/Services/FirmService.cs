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
    public interface IFirmService
    {
        Task<IEnumerable<Firm>> GetAll();
        Task<Firm> GetById(Guid id);
        Task<Firm> Create(Firm firm);
        Task<Task> Update(Firm firm, string password);
        Task<Task> Delete(Guid id);
        Task<Client> AddClient(Guid firmId, Client client);
        Task<IEnumerable<Client>> GetClient(Guid firmId);
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
            UserService userService = new UserService(_context);
            try
            {
                firm.User.Role = Role.Firm;
                firm.User = await userService.Create(firm.User).ConfigureAwait(false);
            }catch (AppException ex)
            {
               throw new AppException("User service:" + ex.Message);
            }           

            _context.Firm.Add(firm);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return firm;
        }

        public async Task<Task> Update(Firm firmParam, string password = null)
        {
            var firm = await _context.Firm.FindAsync(firmParam.FirmId);

            if (firm == null)
                throw new AppException("Firm not found");

            // update username if it has changed
           

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(firmParam.Name))
                firm.Name = firmParam.Name;

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
            return await _context.Firm.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Client>> GetClient(Guid FirmId)
        {
            var Firm = _context.Firm
               .Include(a => a.Clients).Where(a => a.FirmId == FirmId).FirstOrDefault();
            return Firm.Clients;
        }
        public async Task<Client> AddClient(Guid FirmId, Client client)
        {
            UserService userService = new UserService(_context);
            try
            {
                client.FirmId = FirmId;
                client.User.Role = Role.Client;
                client.User = await userService.Create(client.User).ConfigureAwait(false);
            }
            catch (AppException ex)
            {
                throw new AppException("User service:" + ex.Message);
            }
            _context.Client.Add(client);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return client;
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
