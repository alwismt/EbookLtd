using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Base;
using Microsoft.EntityFrameworkCore;
using ecom.Data.ViewModels;


// using ecom.Data;
// using ecom.Data.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;



namespace ecom.Data.Services
{
    public class BooksService : EntityBaseRepository<Book>, IBooksService
    {
        private readonly AppDbContext _context;
        public BooksService(AppDbContext context) :base(context) 
        {
            _context = context;
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var bookDetails = await _context.Books
                .Include(p => p.Publisher)
                .Include(wb => wb.Writter_Books).ThenInclude(w => w.Writter)
                .FirstOrDefaultAsync(n => n.Id == id);

            return bookDetails;
        }

        public async Task <IEnumerable<Book>> GetAllDescAsync()
        {
            var result = await _context.Books
            //.OrderBy(x => x.Id)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
            return result;
        }

        public async Task<Book> GetBookBySlugAsync(string slug)
        {
            var bookDetails = await _context.Books
                .Include(p => p.Publisher)
                .Include(wb => wb.Writter_Books).ThenInclude(w => w.Writter)
                .FirstOrDefaultAsync(n => n.Slug == slug);
            return bookDetails;
        }

        public async Task <IEnumerable<Book>> NewBooks()
        {
            var result = await _context.Books
            .OrderByDescending(x => x.Id)
            .Take(4)
            .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Book>> HarryPotter()
        {
            var result = await _context.Books
            .OrderBy(x => x.Id)
            .Take(11)
            .ToListAsync();
            return result;
        }

        public async Task <Book> SlugCheck(string slug)
        {
            var result = await _context.Books
                .FirstOrDefaultAsync(n => n.Slug == slug);
            return result;
        }

        public async Task<NewBookDropdownsVM> GetNewBookDropdownsValues()
        {
            var reponse = new NewBookDropdownsVM();
            reponse.Writters = await _context.Writters.OrderBy(n => n.Name).ToListAsync();
            reponse.Publishers = await _context.Publishers.OrderBy(n => n.Name).ToListAsync();
            return reponse;
        }
        //Add new book method
        public async Task AddNewBookAsync(NewBookVM data)
        {
            var newBook = new Book()
            {
                Name = data.Name,
                Slug = data.Slug,
                Description = data.Description,
                Price = data.Price,
                Image = data.Image,
                PublishDate = data.PublishDate,
                BookCategory = data.BookCategory,
                PublisherId = data.PublisherId
                
            };
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            //Add Book Writers
            foreach(var writerId in data.WritterIds)
            {
                var newWriterBook = new Writter_Book()
                {
                    BookId = newBook.Id,
                    WritterId = writerId
                };
                await _context.Writter_Books.AddAsync(newWriterBook);
            }
            await _context.SaveChangesAsync();
            //throw new Exception();
        }

        public async Task UpdateBookAsync(NewBookVM data)
        {
            var dbBook = await _context.Books.FirstOrDefaultAsync(n => n.Id == data.Id);
            if(dbBook != null)
            {
                dbBook.Name = data.Name;
                dbBook.Slug = data.Slug;
                dbBook.Description = data.Description;
                dbBook.Price = data.Price;
                dbBook.Image = data.Image;
                dbBook.PublishDate = data.PublishDate;
                dbBook.BookCategory = data.BookCategory;
                dbBook.PublisherId = data.PublisherId;
                await _context.SaveChangesAsync();
            }
            //Remove existing writers
            var existingWriterDb = _context.Writter_Books.Where(n => n.BookId == data.Id).ToList();
            _context.Writter_Books.RemoveRange(existingWriterDb);
            await _context.SaveChangesAsync();

            //Add Book Writers
            foreach(var writerId in data.WritterIds)
            {
                var newWriterBook = new Writter_Book()
                {
                    BookId = data.Id,
                    WritterId = writerId
                };
                await _context.Writter_Books.AddAsync(newWriterBook);
            }
            await _context.SaveChangesAsync();
        }

        public IEnumerable<BookCategoryCountData> GetBookCount()
        {
            //int dd = 2;
            var count = _context.Books
                    .GroupBy(book => book.BookCategory)                 
                    .Select(group => new BookCategoryCountData() { 
                        Category = group.Key, 
                        Count = group.Count() 
                    }).AsEnumerable();
            return count;
            
        }

        public async Task DeleteBookAsync(Book data)
        {
            //Remove existing writers
            var existingWriterDb = _context.Writter_Books.Where(n => n.BookId == data.Id).ToList();
            _context.Writter_Books.RemoveRange(existingWriterDb);
            await _context.SaveChangesAsync();
        }

    }
}

