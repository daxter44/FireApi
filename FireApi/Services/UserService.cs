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
   
        public interface IUserService
        {
            Task<User> Authenticate(string username, string password);
            Task<IEnumerable<User>> GetAll();
            Task<User> GetById(Guid id);
            Task<User> Create(User user, string password);
            Task<Task> Update(User user, string password = null);
            Task<Task> Delete(Guid id);
            Task<Device> AddDevice(Guid userId, Device device);
            Task<IEnumerable<Device>> GetDevices(Guid userid);
        }

        public class UserService : IUserService
        {
            private DataContext _context;

            public UserService(DataContext context)
            {
                _context = context;
            }

            public async Task<User> Authenticate(string username, string password)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return null;

                var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username).ConfigureAwait(false);

                // check if username exists
                if (user == null)
                    return null;

                // check if password is correct
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                // authentication successful
                return user;
            }

            public async Task<IEnumerable<User>> GetAll()
            {
                return await _context.Users.ToListAsync().ConfigureAwait(false);
            }

            public async Task<User> GetById(Guid id)
            {
                return await _context.Users.FindAsync(id).ConfigureAwait(false);
            }

            public async Task<User> Create(User user, string password)
            {
                // validation
                if (string.IsNullOrWhiteSpace(password))
                    throw new AppException("Password is required");
                var anyExist = await _context.Users.AnyAsync(x => x.Username == user.Username).ConfigureAwait(false);
                if (anyExist)
                    throw new AppException("Username \"" + user.Username + "\" is already taken");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return user;
            }

            public async Task<Task> Update(User userParam, string password = null)
            {
                var user = await _context.Users.FindAsync(userParam.Id);

                if (user == null)
                    throw new AppException("User not found");

                // update username if it has changed
                if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
                {
                    // throw error if the new username is already taken
                    if (_context.Users.Any(x => x.Username == userParam.Username))
                        throw new AppException("Username " + userParam.Username + " is already taken");

                    user.Username = userParam.Username;
                }

                // update user properties if provided
                if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                    user.FirstName = userParam.FirstName;

                if (!string.IsNullOrWhiteSpace(userParam.LastName))
                    user.LastName = userParam.LastName;

                // update password if provided
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                _context.Users.Update(user);
               await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
            }

            public async Task<Task> Delete(Guid id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                   await _context.SaveChangesAsync().ConfigureAwait(false);
                }
            return Task.CompletedTask;
            }
            public async Task<Device> AddDevice(Guid userid, Device deviceItem)
            {
                var user = _context.Users.Find(userid);
                _context.DeviceItems.Add(deviceItem);
                user.Devices.Add(deviceItem);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return deviceItem;
            }
            public async Task<IEnumerable<Device>> GetDevices(Guid userid)
        {
            // var user = await _context.Users.FindAsync(userid);
            var user = _context.Users
                .Include(a => a.Devices).Where(a => a.Id == userid).FirstOrDefault();
            return user.Devices;
        }

        // private helper methods

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

            private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
                if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
                if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

                using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i]) return false;
                    }
                }

                return true;
            }
        }
    
}
