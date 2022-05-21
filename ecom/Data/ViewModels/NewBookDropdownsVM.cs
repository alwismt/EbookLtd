using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;

namespace ecom.Data.ViewModels

{
    public class NewBookDropdownsVM
    {
        public NewBookDropdownsVM()
        {
            Publishers = new List<Publisher>();
            Writters = new List<Writter>();

        }
        public List <Publisher> Publishers { get; set; }
        public List <Writter> Writters { get; set; }
    }
}