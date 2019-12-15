using FireApi.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireApi.Models.Users;
using FireApi.Models.Device;
using FireApi.Entity;

namespace FireApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<Device, DeviceModel>();
            CreateMap<AddDeviceModel, Device>();
            CreateMap<UpdateDeviceModel, Device>();
        }
    }
}
