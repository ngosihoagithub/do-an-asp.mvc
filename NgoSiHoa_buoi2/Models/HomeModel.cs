using NgoSiHoa_buoi2.Contex;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NgoSiHoa_buoi2.Models
{
    public class HomeModel
    {
        public List<Product> ListProducts { get; set; }
        public List<Category> ListCategory { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

    }
}