using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.ViewModels
{
    public class CategoryViewModel
    {
        [Required,StringLength(15)]
        [Display(Name ="Kategori Adı")]
        public string CategoryName { get; set; }
        [Display(Name ="Açıklama")]
        public string Description { get; set; }
    }
}
