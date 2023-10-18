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
        public string Name { get; set; }
        public string ApartmentName { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }
}
