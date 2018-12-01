using System;
using System.Linq;
using EssentialTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products = {
            new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
            new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
            new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
            new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
        };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            // arrange
            //Bước đầu tiên là cho Moq biết mình muốn làm việc với loại
            //đối tượng nào. Moq dựa rất nhiều vào các tham số kiểu, và
            //ta có thể thấy điều này theo cách mà ta nói với Moq rằng
            //ta muốn tạo ra một thực thi giả lập IDiscountHelper

            //chúng ta tạo ra một đối tượng Mock <IDiscountHelper>
            //để nói cho Moq biết type mà Moq sẽ xử lý 
            //Đây là interface IDiscountHelper cho unit test của
            //ta, nhưng nó có thể là bất kỳ loại nào mà bạn muốn 
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();

            //Chúng cần xác định cách hoạt động của đối tượng vừa tạo.
            //Đây là trọng tâm của quá trình mô phỏng và nó cho phép chúng ta
            //đảm bảo rằng ta thiết lập một hành vi cơ sở trong đối tượng giả,
            //mà ta có thể sử dụng để kiểm tra chức năng của đối tượng đích của
            //ta trong Unit test.
            //Lớp It định nghĩa một số phương thức được sử dụng với các tham số
            //kiểu generic. Trong trường hợp này, ta đã gọi phương thức IsAny
            //bằng cách sử dụng decimal làm kiểu generic. Điều này để Moq 
            //áp dụng các hành vi ta định nghĩa bất cứ khi nào ta gọi phương thức
            //ApplyDiscount với bất kỳ giá trị thập phân nào.
            //Phương thức Returns cho phép ta chỉ định kết quả mà Moq sẽ trả về
            //khi ta gọi phương thức giả của mình. Tôi chỉ định loại kết quả bằng
            //cách sử dụng tham số kiểu và chỉ định kết quả bằng cách sử dụng
            //biểu thức lambda.  
            //Returns<decimal> nói với Moq rằng ta sẽ trả về một giá trị thập phân.
            //Đối với biểu thức lambda, Moq chuyển cho ta một giá trị của kiểu
            //nhận được trong phương thức ApplyDiscount. Ta tạo ra một phương thức
            //pass-through trong bên dưới, trong đó ta trả về giá trị được truyền vào
            //phương thức ApplyDiscount giả lập mà không thực hiện bất kỳ thao
            //tác nào trên nó.
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);

            //Bước cuối cùng là sử dụng đối tượng giả trong thử nghiệm,
            // bằng cách đọc giá trị của thuộc tính Object của đối tượng
            // Mock <IDiscountHelper>:
            var target = new LinqValueCalculator(mock.Object);
            // act
            var result = target.ValueProducts(products);
            // assert
            Assert.AreEqual(products.Sum(e => e.Price), result);

            //Tóm tắt ví dụ: thuộc tính Object trả về một triển khai của
            //interface IDiscountHelper trong đó phương thức ApplyDiscount
            //trả về giá trị của tham số thập phân mà nó được truyền.
            //Điều này giúp ta dễ dàng để thực hiện kiểm tra đơn vị
            //bởi vì ta có thể tổng hợp giá của các đối tượng
            //sản phẩm thử nghiệm của chính mình và kiểm tra xem
            //ta có nhận được cùng một giá trị trở lại từ đối tượng
            //LinqValueCalculator hay không.




        }

        
        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            // arrange
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0)))
                .Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
                .Returns<decimal>(total => (total * 0.9M));
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100,
                Range.Inclusive))).Returns<decimal>(total => total - 5);
            var target = new LinqValueCalculator(mock.Object);

            // act
            decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));
            // assert
            Assert.AreEqual(5, FiveDollarDiscount, "$5 Fail");
            Assert.AreEqual(5, TenDollarDiscount, "$10 Fail");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 Fail");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 Fail");
            target.ValueProducts(createProduct(0));

        }
    }
}
