using fiorello.DAL;
using fiorello.Models;
using Microsoft.AspNetCore.Identity;
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

        private UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
            if (User.Identity.IsAuthenticated)
            {
                AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName = currentUser.FullName;
            }
            return View(await Task.FromResult(bio));
        }

    }
}
