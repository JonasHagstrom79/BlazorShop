using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class ProductType
    {

        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        [NotMapped] //Not a coulumn in the Database
        public bool Editing { get; set; } = false;//Not in the database
        [NotMapped]
        public bool IsNew { get; set; } = false;//Not in the database

    }
}
