using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private DatabaseContext _db;

        public OrderRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Order obj)
        {
            _db.Order.Update(obj);
        }

        public void UpdateOrderStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            //Retrieve the order from the database.
            var order = _db.Order.FirstOrDefault(u => u.Id == id);

            if (order != null)
            {
                order.OrderStatus = orderStatus;

                if (paymentStatus != null)
                {
                    order.OrderPaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePayment(int id, string sessionId, string paymentIntentId)
        {
            //Retrieve the order from the database.
            var order = _db.Order.FirstOrDefault(u => u.Id == id);

            order.SessionId = sessionId;
            order.PaymentIntentId = paymentIntentId;
        }

        public void UpdateOrderPaymentStatus(int id, string paymentStatus)
        {
            //Retrieve the order from the database.
            var order = _db.Order.FirstOrDefault(u => u.Id == id);

            if (paymentStatus != null)
            {
                order.OrderPaymentStatus = paymentStatus;
            }
        }
    }


}

