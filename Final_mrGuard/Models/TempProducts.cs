using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final_mrGuard.Models
{
    public class TempProducts
    {
        [DataType(DataType.Text)]
        [Display(Name = "Product Name")]
        public string Product_Name { get; set; }
       
    }
}