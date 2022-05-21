using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ecom.Data;
using ecom.Data.Base;

namespace ecom.Models
{
    public class NewBookVM
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Slug is required")]
        public string Slug { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        public string Image { get; set; }
        
        [Required(ErrorMessage = "PublishDate is required")]
        public string PublishDate { get; set; }

        [Required(ErrorMessage = "Book category is required")]
        public BookCategory BookCategory { get; set; }

        //Relationships
        [Required(ErrorMessage = "Book writer(s) is required")]
        public List<int> WritterIds { get; set; }

        [Required(ErrorMessage = "Book publisher is required")]
        public int PublisherId { get; set; }

    }
}


