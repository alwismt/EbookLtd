using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models
{
    public class Writter_Book
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int WritterId { get; set; }
        public Writter Writter { get; set; }

    }
}


