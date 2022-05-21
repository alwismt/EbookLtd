using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book {get; set;}

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order {get; set;}

    }
}


