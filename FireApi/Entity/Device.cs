using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Entity
{
    public class Device
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public String Name { get; set; }
        public int Temperature { get; set; }
        [Required]
        public virtual User user { get; set; }

    }
}
