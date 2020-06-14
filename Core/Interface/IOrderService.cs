using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Interface
{
    public interface IOrderService
    {
         Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address ShippingAddress);

         Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

         Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

         Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}