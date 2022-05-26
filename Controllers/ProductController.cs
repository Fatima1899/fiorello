using fiorello.DAL;
using fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace fiorello.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product>products = _context.products.Include(p => p.Category).OrderByDescending(p=>p.Id).Take(8).ToList();
            return View(products);
        }
        public IActionResult LoadMore(int skip)
        {
            List<Product> products = _context.products.Include(p => p.Category).Skip(skip).Take(8).ToList();
            return Json(new{
                name="kamal"
            });
        }
    }
}
