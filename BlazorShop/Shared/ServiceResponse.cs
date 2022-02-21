using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class ServiceResponse<T>
    {
        //To handle all calls and make controller thin
        #region Properties
        public T? Data { get; set; } //The list of products
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        #endregion
    }
}
