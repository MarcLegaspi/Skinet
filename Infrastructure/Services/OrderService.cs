using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interface;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        // private readonly IGenericRepository<Order> _orderRepo;
        // private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        // private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepo;

        // public OrderService(IGenericRepository<Order> orderRepo,
        //                     IGenericRepository<DeliveryMethod> deliveryMethodRepo,
        //                     IGenericRepository<Product> productRepo,
        //                     IBasketRepository basketRepo)
        // {
        //     _orderRepo = orderRepo;
        //     _deliveryMethodRepo = deliveryMethodRepo;
        //     _productRepo = productRepo;
        //     _basketRepo = basketRepo;
        // }
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get the basket from repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get items from the product repo
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(productItemOrdered, productItem.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc sub total
            var subTotal = orderItems.Sum(m => m.Quantity * m.Price);

            // create order
            var order = new Order(orderItems, buyerEmail, shippingAddress, deliveryMethod, subTotal);
            _unitOfWork.Repository<Order>().Add(order);

            // TODO: save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            //Delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecifications(id,buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecifications(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}