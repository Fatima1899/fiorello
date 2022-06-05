using fiorello.Models;
using fiorello.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace fiorello.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager ;

        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task <IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser newUser = new AppUser()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email=registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                    return View(registerVM);
            }

            await _signInManager.SignInAsync(newUser, true);

            return RedirectToAction("index","home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task <IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser dbUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (dbUser == null)
            {
                ModelState.AddModelError("", "email or password wrong");
                return View(loginVM);
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(dbUser,loginVM.Password,loginVM.RememberMe,true);




            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "your account is lockout");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "email or password wrong");
                return View(loginVM);
            }
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
