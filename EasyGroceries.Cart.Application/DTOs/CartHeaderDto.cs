using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.DTOs
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public int UserId { get; set; }
        public bool LoyaltyMembershipOpted { get; set; }
        public double CartTotal { get; set; }
        public CustomerDto CustomerInfo { get; set; }
    }
}
