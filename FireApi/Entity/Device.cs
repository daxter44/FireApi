using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Entity
{
    public class Device
    {
        public Guid ID { get; set; }

        public int UserId { get; set; }
        public String Name { get; set; }
        public int Temperature { get; set; }

    }
}
