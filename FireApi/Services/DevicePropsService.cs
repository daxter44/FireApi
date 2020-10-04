using FireApi.Database;
using FireApi.Database.Entity;
using FireApi.Database.Entity.Entity;
using FireApi.Database.Repository;
using FireApi.Helpers;
using FireApi.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Services
{
    public interface IDevicePropService
    {
        Task<DeviceProp> GetLastPropAwait(Guid deviceId);
    }
    public class DevicePropService : IDevicePropService 
    {

        private readonly IMongoRepository<DeviceProp> _settingsRepository;

        public DataContext _dbContext;

        public DevicePropService(IMongoRepository<DeviceProp> settingsRepository, DataContext context)
        {
            _settingsRepository = settingsRepository;
            _dbContext = context;
        }

        public async Task<DeviceProp> GetLastPropAwait(Guid deviceId)
        {
            DeviceDoc deviceDoc = await GetDeviceDocByDeviceId(deviceId).ConfigureAwait(false);
            ObjectId id = ObjectId.Parse(deviceDoc.DocumentId);
            DeviceProp deviceProp = await _settingsRepository.FindOneAsync(
                filter => filter.DocumentId == id
            ).ConfigureAwait(false); 
            return deviceProp;
          
        }

        private async Task<DeviceDoc> GetDeviceDocByDeviceId(Guid deviceId) =>
           await _dbContext.DeviceDocs.OrderByDescending(c => c.DocCreationTime).FirstOrDefaultAsync(x => x.DeviceID == deviceId).ConfigureAwait(false);

    }
}
