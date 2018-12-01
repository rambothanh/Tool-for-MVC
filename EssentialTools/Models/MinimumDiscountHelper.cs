using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{
    public class MinimumDiscountHelper: IDiscountHelper
    {
        //Muc tiêu:
        //Nếu total >100 giảm 10%
        //Nếu 10 < total =<100 giảm 5$
        //Nếu total <10 không giảm 
        //ArgumentOutOfRangeException sẽ ném lỗi khi total <0
        public decimal ApplyDiscount(decimal totalParam)
        {
            if (totalParam < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if(totalParam > 100)
            {
                return totalParam * 0.9M;
            }
            else if (totalParam >= 10 && totalParam <=100)
            {
                return totalParam - 5M;
            }
            else
            {
                return totalParam;
            }

            
        }
    }
}