using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        // A single order can be associated with multiple Order Lines.
        public IEnumerable<OrderLine> OrderLine { get; set; }
    }
}
