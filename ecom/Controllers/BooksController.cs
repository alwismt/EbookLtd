using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Data;
using Microsoft.EntityFrameworkCore;
using ecom.Data.Services;
using ecom.Models;
using Microsoft.AspNetCore.Authorization;

namespace ecom.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly IBooksService _service;
        public BooksController(IBooksService service)
        {
            _service = service;
        }
        [Route("books", Name = "books")]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAll(n => n.Publisher);
            //var data = await _service.GetAll();
            return View(data);
        }

        [Route("book/create", Name = "Createbooks")]
        public async Task<IActionResult> Create()
        {
            var bookDopdownsData = await _service.GetNewBookDropdownsValues();
            ViewBag.Publisher = new SelectList(bookDopdownsData.Publishers, "Id", "Name");
            ViewBag.Writers = new SelectList(bookDopdownsData.Writters, "Id", "Name");
            return View();
        }

        [HttpPost("book/add")]
        public async Task <IActionResult> AddBook(NewBookVM book, [FromForm]IFormFile Image)
        {
            //Check model state
            if(!ModelState.IsValid){
                var bookDopdownsData = await _service.GetNewBookDropdownsValues();
                ViewBag.Publisher = new SelectList(bookDopdownsData.Publishers, "Id", "Name");
                ViewBag.Writers = new SelectList(bookDopdownsData.Writters, "Id", "Name");
                return View("Create",book);
            }
            //Upload the image
            if (Image != null && Image.Length > 0){
                var fileName = Path.GetRandomFileName()+Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books", fileName);
                book.Image = "/images/books/"+fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileSrteam);
                }
                await _service.AddNewBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            else{
                var bookDopdownsData = await _service.GetNewBookDropdownsValues();
                ViewBag.Publisher = new SelectList(bookDopdownsData.Publishers, "Id", "Name");
                ViewBag.Writers = new SelectList(bookDopdownsData.Writters, "Id", "Name");
                ViewBag.ErrorMessage = "Book Image is Required!";
                return View("Create",book);
            }

            
            //return RedirectToAction(nameof(Index));
        }

        [HttpPost("bookurl/slugcheck/{slug}")]
        public async Task <IActionResult> SlugCheck(string slug){
            var data = await _service.SlugCheck(slug);
            if(data == null)
            {
                return Json(new { success = true, slug = slug });
            }
            else{
                return Json(new { success = false, slug = slug });
            }
            
            
        }

        [Route("book/edit/{slug}")]
        public async Task<IActionResult> Edit(string slug)
        {
            var book = await _service.GetBookBySlugAsync(slug);
            if(book == null) return View("NotFound");

            var response = new NewBookVM()
            {
                Id = book.Id,
                Name = book.Name,
                Slug = book.Slug,
                Description = book.Description,
                Price = book.Price,
                Image = book.Image,
                PublishDate = book.PublishDate,
                BookCategory = book.BookCategory,
                PublisherId = book.PublisherId,
                WritterIds = book.Writter_Books.Select(n => n.WritterId).ToList(),
            };
            var bookDopdownsData = await _service.GetNewBookDropdownsValues();
            ViewBag.Publisher = new SelectList(bookDopdownsData.Publishers, "Id", "Name");
            ViewBag.Writers = new SelectList(bookDopdownsData.Writters, "Id", "Name");
            return View("edit", response);
        }


        [HttpPost("book/add/{id}")]
        public async Task <IActionResult> UpdateBook(int id,string oldImage, NewBookVM book, [FromForm]IFormFile Image)
        {
            if(id != book.Id) return View("NotFound");
            if(!ModelState.IsValid){
                
                var bookDopdownsData = await _service.GetNewBookDropdownsValues();
                ViewBag.Publisher = new SelectList(bookDopdownsData.Publishers, "Id", "Name");
                ViewBag.Writers = new SelectList(bookDopdownsData.Writters, "Id", "Name");
                return View("Create",book);
            }

            if (Image != null && Image.Length > 0){
                var fileName = Path.GetRandomFileName()+Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books", fileName);
                book.Image = "/images/books/"+fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileSrteam);
                }
                await _service.UpdateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            else{
                book.Image = oldImage;
                await _service.UpdateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            
            //return RedirectToAction(nameof(Index));
        }

        [Route("/book/delete/{id}")]
        public async Task <IActionResult> DeleteBook(int id)
        {
            var book = await _service.GetBookByIdAsync(id);
            if(book == null)return View("NotFound");
            await _service.DeleteBookAsync(book);
            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }



    }
}