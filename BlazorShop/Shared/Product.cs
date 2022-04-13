using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class Product
    {
        //Add the class in _Imports.razor
        //Add global using BlazorShop.Shared; to program.cs for the controller
        #region Properties
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty; //To avoid null reference exeption
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        //[Column(TypeName= "decimal(18,2)")] //define the digits for the model
        //public decimal Price { get; set; }//price
        public Category? Category { get; set; } //Link to category
        public int CategoryId { get; set; } //FKey
        public bool Featured { get; set; } = false; //for the feature functionality
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>(); //Different varriant of the same object, paperback, ebook etc
        public bool Visible { get; set; } = true; 
        public bool Deleted { get; set; } = false;
        [NotMapped] //Not a coulumn in the Database
        public bool Editing { get; set; } = false;//Not in the database
        [NotMapped]
        public bool IsNew { get; set; } = false;//Not in the database

        #endregion
    }
}
