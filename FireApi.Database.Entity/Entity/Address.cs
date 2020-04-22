using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireApi.Database.Entity
{
    public class Address
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String ZipCode { get; set; }
        public String HouseNumber { get; set; }
    }
}