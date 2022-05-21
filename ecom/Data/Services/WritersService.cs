//using ecom.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Base;


namespace ecom.Data.Services
{
    public class WritersService : EntityBaseRepository<Writter>, IWritersService
    {
        
        public WritersService(AppDbContext context) : base(context) { }
        
        

    }
}