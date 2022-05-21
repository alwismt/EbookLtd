using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ecom.Data.Base;
using ecom.Data;

namespace ecom.Models
{
    public class Address:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order {get; set;}

        public string FullName { get; set; }
        public string ShipAddress { get; set; }
        
        public string Apartment { get; set; }
        public string District { get; set; }
        public string Postcode { get; set; }
        //public string PhoneNumber { get; set; }

    }
}