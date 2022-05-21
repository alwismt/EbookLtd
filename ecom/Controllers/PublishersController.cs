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
    public class PublishersController : Controller
    {
        private readonly IPublishersService _service;
        public PublishersController(IPublishersService service)
        {
            _service = service;
        }
        [Route("publishers", Name = "publisher")]
        public async Task <IActionResult> Index()
        {
            var data = await _service.GetAll();
            return View("index", data);
        }
        [Route("publisher/create", Name = "NewPublisher")]
        public IActionResult Create()
        {
            return View("create");
        }


        [HttpPost("publisher/create")]
        public async Task <IActionResult> save([Bind("Name,Bio")]Publisher publisher){
            if(!ModelState.IsValid){
                return View("create",publisher);
            }
            else{
                await _service.Add(publisher);
                return RedirectToAction(nameof(Index));
            }
            
        }
        [Route("publisher/edit/{id}", Name = "PublisherEdit")]
        public async Task<IActionResult> Edit(int id)
        {
            var edit = await _service.GetById(id);
            if(edit == null) return View("NotFound");
            return View("edit", edit);
        } 

        [HttpPost("publisher/update/{id}", Name = "PublisherUpdate")]
        public async Task<IActionResult> update(int id,string oldImage,[Bind("Id,Name,Bio")]Publisher publisher)
        {
            if(!ModelState.IsValid){
                return View("edit",publisher);
            }
            if(id == publisher.Id){
                await _service.Update(id, publisher);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            
        }

        [Route("publisher/delete/{id}", Name = "PublisherDelete")]
        public async Task <IActionResult> delete(int id)
        {
            var del = await _service.GetById(id);
            if(del == null) return View("NotFound");
            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}