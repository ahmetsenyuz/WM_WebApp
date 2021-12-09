using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_MVC_Projesi.Models
{
    public partial class Deneme1
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Shipper { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Supplier { get; set; }
        public string SupCity { get; set; }
        public string SupCountry { get; set; }
        public string Salesman { get; set; }
    }
}
