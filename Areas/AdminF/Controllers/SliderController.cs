using fiorello.DAL;
using fiorello.Extentions;
using fiorello.Helpers;
using fiorello.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace fiorello.Areas.AdminF.Controllers
{
        [Area("AdminF")]
    public class SliderController : Controller
    {
        private IWebHostEnvironment _env;
        private AppDbContext _context;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "accept only image");
                return View();
            }
            if (slider.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mg yuxari ola bilmez");
                return View();
            }
            string fileName = await slider.Photo.SaveImage(_env, "img");
            Slider newSlider = new Slider();
            newSlider.ImageUrl = fileName;
            await _context.sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider dbSlider = await _context.sliders.FindAsync(id);
            if (dbSlider == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbSlider.ImageUrl);
            _context.sliders.Remove(dbSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
