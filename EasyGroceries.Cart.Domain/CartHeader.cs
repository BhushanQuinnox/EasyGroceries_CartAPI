using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Domain
{
    public class CartHeader
    {
        public int CartHeaderId { get; set; }

        public int UserId { get; set; }

        public bool LoyaltyMembershipOpted { get; set; }

        public double CartTotal { get; set; }
    }
}
