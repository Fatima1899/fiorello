using fiorello.Models;
using System.Collections.Generic;

namespace fiorello.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> sliders { get; set; }
        public PageIntro pageIntro { get; set; }

        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
