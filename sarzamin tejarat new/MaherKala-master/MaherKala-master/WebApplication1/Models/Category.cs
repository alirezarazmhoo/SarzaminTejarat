using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "نام  دسته بنذی را وارد نمایید")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "تصویر را وارد نمایید")]
        //[MaxLength(1000)]
        public string Image { get; set; }

        [MaxLength(1000)]
        public string Thumbnail { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category>Children{ get; set; }

        public virtual Category Parent{ get; set; }
        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}