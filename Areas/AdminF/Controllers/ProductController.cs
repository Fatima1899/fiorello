using fiorello.DAL;
using fiorello.Models;
using fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fiorello.Areas.AdminF.Controllers
{
    [Area("AdminF")]
    public class ProductController : Controller
    {
        private AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int take=5,int pageSize=1)
        {
            List<Product> products = _context.products
                .Include(p => p.Category)
                .Skip((pageSize-1)*take)
                .Take(take)
                .ToList();

            Pagination<ProductVM> pagination = new Pagination<ProductVM>(
                ReturnPageCount(take),
                pageSize,
                MapProductToProductVM(products)
                ) ;

            return View(pagination);
        }

        private List<ProductVM> MapProductToProductVM(List<Product> products)
        {
            List<ProductVM> productVMs = products.Select(p => new ProductVM
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl=p.ImageUrl,
                Count=p.Count,
                Price=p.Price,
                CategoryName=p.Category.Name
            }).ToList();
            return productVMs;
        }

        private int ReturnPageCount(int take)
        {
            int productCount = _context.products.Count();

            return (int)Math.Ceiling(((decimal)productCount / take));
        }
    }
}
