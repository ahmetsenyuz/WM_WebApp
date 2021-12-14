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
        //[Required,StringLength(15)] böyle olursa default olarak hatayı kendi yazdırır ancak ingilizce yazıyor. Alttaki gibi yazarsak hata mesajı olarak bizim yazdığımız mesajı verir
        [Required(ErrorMessage ="Kategori Adı alanı boş bırakılamaz.")]
        [StringLength(15,ErrorMessage ="Kategori adı en fazla 15 karakter olabilir.")]
        [Display(Name ="Kategori Adı")]
        public string CategoryName { get; set; }
        [Display(Name ="Açıklama")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ProductCount { get; set; }
    }
}
