using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class ProductVariant
    {
        //composide primary key
        [JsonIgnore] //To breake the circular reference between product(list in a list in a list etc)
        public Product? Product { get; set; } //Must be nullable
        public int ProductId { get; set; }
        public ProductType? ProductType { get; set; } //Must be nullable
        public int ProductTypeId { get; set; }
        //set the price here instead
        [Column(TypeName="decimal(18,2)")]
        public decimal Price { get; set; }
        //add the original prize
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped] //Not a coulumn in the Database
        public bool Editing { get; set; } = false;//Not in the database
        [NotMapped]
        public bool IsNew { get; set; } = false;//Not in the database
    }
}
