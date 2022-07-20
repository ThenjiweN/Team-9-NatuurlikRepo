using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order obj);
        void UpdateOrderStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateOrderPaymentStatus(int id, string paymentStatus);
        void UpdateStripePayment(int id, string sessionId, string paymentIntentId);
    }
}
