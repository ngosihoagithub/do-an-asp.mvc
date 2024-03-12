using NgoSiHoa_buoi2.Contex;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NgoSiHoa_buoi2.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}