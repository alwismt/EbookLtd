using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Data;
using ecom.Models;
using ecom.Data.Services;
using Microsoft.AspNetCore.Authorization;

namespace ecom.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class WrittersController : Controller
    {
        //private readonly AppDbContext _context;
        private readonly IWritersService _service;
        public WrittersController(IWritersService service)
        {
            _service = service;
        }
        
        [Route("writers", Name = "writers")]
        public async Task <IActionResult> Index()
        {
            var data = await _service.GetAll();
            return View(data);
        }
        [Route("writer/create", Name = "NewWriter")]
        public IActionResult Create()
        {
            return View("create");
        }

        [HttpPost("writer/save")]
        public async Task <IActionResult> save([Bind("Name,Image,Bio")]Writter writter, [FromForm]IFormFile image)
        {
            if(!ModelState.IsValid){
                return View("create",writter);
            }
            if (image != null && image.Length > 0){
                var fileName = Path.GetRandomFileName()+Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/actors", fileName);
                writter.Image = "/images/actors/"+fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileSrteam);
                }
                await _service.Add(writter);
                return RedirectToAction(nameof(Index));
            }
            else{
                ViewBag.ErrorMessage = "Profile Image is Required!";
                return View("create",writter);
            }
        }

        

        [Route("writer/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {
            var edit = await _service.GetById(id);
            if(edit == null) return View("NotFound");
            return View("edit", edit);
        }

        [HttpPost("writerupdate/{id}", Name = "update")]
        public async Task<IActionResult> update(int id,string oldImage,[Bind("Id,Name,Image,Bio")]Writter writter, [FromForm]IFormFile image)
        {
            if(!ModelState.IsValid){
                return View("edit",writter);
            }
            if(id == writter.Id){
                if (image != null && image.Length > 0){
                    var fileName = Path.GetRandomFileName()+Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/actors", fileName);
                    writter.Image = "/images/actors/"+fileName;
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileSrteam);
                    }
                    
                    await _service.Update(id, writter);
                    return RedirectToAction(nameof(Index));
                }
                else{
                    writter.Image = oldImage;
                    await _service.Update(id, writter);
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
            
        }
        [HttpGet("writer/delete/{id}", Name = "delete")]
        public async Task <IActionResult> delete(int id)
        {
            //var edit = await _service.GetById(id);
             await _service.Delete(id);
             return RedirectToAction(nameof(Index));
        }
    }
}