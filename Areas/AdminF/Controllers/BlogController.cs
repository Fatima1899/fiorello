using fiorello.DAL;
using fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace fiorello.Areas.AdminF.Controllers
{
        [Area("AdminF")]
    public class BlogController : Controller
    {
        private AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Blog> blogs = _context.blogs.ToList();
            return View(blogs);
        }
    }
}
