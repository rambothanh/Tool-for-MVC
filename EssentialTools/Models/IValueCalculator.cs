using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{

    public interface IValueCalculator
    {
        //Phương thức này là trừu tượng và public
        decimal ValueProducts(IEnumerable<Product> products);
    }
}