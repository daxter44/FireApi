using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Entity
{
    public class Client
    {
        [ForeignKey(nameof(User))]
        public Guid ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? FirmId { get; set; }
        public User User { get; set; }
        public ICollection<Device> Devices { get; set; }

        public Client()
        {
            Devices = new HashSet<Device>();
        }

    }
}
