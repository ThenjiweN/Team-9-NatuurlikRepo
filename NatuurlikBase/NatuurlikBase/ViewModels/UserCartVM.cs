using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class UserCartVM
    {
       
        public IEnumerable<Cart> CartList { get; set; }
		public decimal CartTotal { get; set; }
	}
}
