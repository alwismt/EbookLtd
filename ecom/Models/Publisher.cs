using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ecom.Data.Base;

namespace ecom.Models
{
    public class Publisher:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Publisher's Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Publisher's Description is Required!")]
        public string Bio { get; set; }
        
        //Relationships
        public List<Book> Books { get; set; }
    }
}


