using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecifications : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecifications(string email) 
            : base(m => m.BuyerEmail == email)
        {
            AddInclude(m => m.OrderItems);
            AddInclude(m => m.DeliveryMethod);
            AddOrderByDescending(m => m.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecifications(int id, string buyerEmail) 
            : base(m => m.Id == id && m.BuyerEmail == buyerEmail)
        {
            AddInclude(m => m.OrderItems);
            AddInclude(m => m.DeliveryMethod);
            AddOrderByDescending(m => m.OrderDate);
        }
    }
}