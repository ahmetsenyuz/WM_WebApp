using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.Models
{
    //entityframework: data anotation ve fluent api araştır.
    public class Urun
    {
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
    }
    public static class UrunManager
    {
        public static List<Urun> GetUrunler()
        {
            var ulist = new List<Urun>();
            ulist.Add(new Urun()
            {
                Ad = "Kitap",
                Fiyat = 15
            });
            ulist.Add(new Urun()
            {
                Ad = "Defter",
                Fiyat = 7
            });
            ulist.Add(new Urun()
            {
                Ad = "Kalem",
                Fiyat = 5
            });
            return ulist;
        }
    }
}
