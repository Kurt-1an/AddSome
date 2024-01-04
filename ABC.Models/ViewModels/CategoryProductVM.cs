using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models.ViewModels
{
	public class CategoryProductVM
	{
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> BestSellingProducts { get; set; }
    }

}
