using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Base;
using Microsoft.EntityFrameworkCore;
using ecom.Data.ViewModels;


namespace ecom.Data.Services

{
    public class BookCategoryCountData
    {
        public BookCategory Category { get; set; }
        // public string BookgCategory { get; set; }
        public int Count { get; set; }
    }
}
