using fiorello.DAL;
using fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fiorello.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private AppDbContext _context;
        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            int totalCount = 0;
            if (Request.Cookies["basket"] != null)
            {
                List<BasketProduct> basketProducts = JsonConvert.DeserializeObject<List<BasketProduct>>(Request.Cookies["basket"]);
                foreach (var item in basketProducts)
                {
                    totalCount += item.Count;
                }
            }
            ViewBag.BasketLength = totalCount;
            Bio bio = _context.bios.FirstOrDefault();
            return View(await Task.FromResult(bio));
        }

    }
}
