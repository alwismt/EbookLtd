using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Base;
using ecom.Data.ViewModels;

namespace ecom.Data.Services
{
    public interface IBooksService:IEntityBaseRepository<Book>
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> GetBookBySlugAsync(string slug);
        Task<Book> SlugCheck(string slug);

        Task <IEnumerable<Book>> NewBooks();
        Task <IEnumerable<Book>> HarryPotter();
        Task <IEnumerable<Book>> GetAllDescAsync();

        Task <NewBookDropdownsVM> GetNewBookDropdownsValues();

        Task AddNewBookAsync(NewBookVM data);
        Task UpdateBookAsync(NewBookVM data);

        IEnumerable<BookCategoryCountData> GetBookCount();

        Task DeleteBookAsync(Book data);


    }
}