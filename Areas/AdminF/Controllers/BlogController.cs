using fiorello.DAL;
using fiorello.Extentions;
using fiorello.Helpers;
using fiorello.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fiorello.Areas.AdminF.Controllers
{
        [Area("AdminF")]
    public class BlogController : Controller
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env=env;
        }
        public IActionResult Index()
        {
            List<Blog> blogs = _context.blogs.ToList();
            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();
            }
            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "accept only image");
                return View();
            }
            if (blog.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mg yuxari ola bilmez");
                return View();
            }
            string fileName = await blog.Photo.SaveImage(_env, "img");
            Blog newblog = new Blog();
            newblog.ImageUrl = fileName;
            await _context.blogs.AddAsync(newblog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Blog dbBlog = await _context.blogs.FindAsync(id);
            if (dbBlog == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbBlog.ImageUrl);
            _context.blogs.Remove(dbBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
