using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EssentialTools.Models;
using Ninject;

namespace EssentialTools.Controllers
{
    public class HomeController : Controller
    {
        //add a class constructor that accepts
        //an implementation of the IValueCalculator interface,
        private IValueCalculator calc;
        public HomeController(IValueCalculator calcParam, IValueCalculator calc2)
        {
            calc = calcParam;
        }
        
        private Product[] products = {
            

            new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
            new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
            new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
            new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
        };

        // GET: Home
        public ActionResult Index()
        {
            ////đầu tiên là chuẩn bị Ninject để sử dụng.
            ////Để làm điều này, tôi tạo một thể hiện của Ninject Kernel,
            ////là đối tượng chịu trách nhiệm giải quyết các phụ thuộc
            ////và tạo các đối tượng mới. Khi tôi cần một đối tượng,
            ////tôi sẽ sử dụng Kernel thay vì từ khóa new.

            ////creating a new instance of the StandardKernel class
            //IKernel ninjectKernel = new StandardKernel();

            ////Thiết lập interface muốn làm việc với tham số kiểu cho phương thức Bind
            ////Và gọi phương thức To trên kết quả return
            ////Thiết lập Inlementation class muốn khởi tạo bằng tham số kiểu cho ư
            ////phương thức To
            ////Câu lệnh này nói với Ninject rằng các phụ thuộc trên
            ////IValueCalculator interface nên được giải quyết bằng cách
            ////tạo một cá thể của lớp LinqValueCalculator
            //ninjectKernel.Bind<IValueCalculator>().To<LinqValueCalculator>();

            ////Sử dụng Ninject để tạo một đối tượng thông qua phương thức Get
            ////Tham số kiểu của phương thức Get nói với Ninject biết rằng 
            ////interface nào mà tôi quan tâm. 
            ////Phương thức Get này trả về kiểu triển khai (implementation type)
            //// mà tôi đã chỉ định với phương thức To ở bên trên.
            //IValueCalculator calc = ninjectKernel.Get<IValueCalculator>();

            //Những dòng comment bên trên đây đã được xử lý bằng:
            //Setting up MVC Dependency Injection
            //Đã phá vỡ được sự phụ thuộc của lớp LinqValueCalculator và lớp
            //HomeController 
            ShoppingCart cart = new ShoppingCart(calc) { Products = products };
            decimal totalValue = cart.CalculateProductTotal();

            return View(totalValue);
        }
    }
}