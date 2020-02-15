using FireApi.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireApi.Models.Users;
using FireApi.Models.Device;
using FireApi.Entity;
using FireApi.Models.Client;
using FireApi.Models.Firm;

namespace FireApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //user
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            //device
            CreateMap<Device, DeviceModel>();
            CreateMap<AddDeviceModel, Device>();
            CreateMap<UpdateDeviceModel, Device>();
            //client
            CreateMap<CreateClientModel, Client>();
            CreateMap<UpdateClientModel, Client>();
            CreateMap<ClientModel, Client>();
            CreateMap<Client, ClientModel>();
            //firm
            CreateMap<CreateFirmModel, Firm>();
            CreateMap<UpdateFirmModel, Firm>();
            CreateMap<FirmModel, Firm>();
        }
    }
}
