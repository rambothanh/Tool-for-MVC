using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{
    public class ShoppingCart
    {
        //Tạo trường kiểu Linq...
        private LinqValueCalculator cals;
        
        //Hàm khởi tạo
        public ShoppingCart(LinqValueCalculator calcParam)
        {
            cals = calcParam;
        }

        //Thuộc tính kiểu IEnumerable<Product>
        public IEnumerable<Product> Products { get; set; }

        //Method tính tổng
        public decimal CalculateProductTotal()
        {
            return cals.ValueProducts(Products);
        }

    }
}