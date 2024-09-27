using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;


namespace Service
{
    internal sealed class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repository;

        private readonly IMapper _mapper;

        public OrderService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task ChangeOrderState(string UserId,int OrderId, OrderState orderState)
        {
            var user = await _repository.User.GetUserAsync(UserId);

            if (user == null)
                throw new UserNotFoundException(UserId);

            var order = await _repository.Order.GetOrderAsync(OrderId, true);
           
            if (order == null)
                throw new OrderNotFoundException(OrderId);

            order.OrderState = orderState;

            await _repository.SaveAsync();
        }

        public async Task<OrderDto> CreateOrder(string UserId, OrderForCreationDto order)
        {
            var user = await _repository.User.GetUserAsync(UserId);

            if (user == null)
                throw new UserNotFoundException(UserId);

            var newOrder = _mapper.Map<Order>(order);

            foreach (var od in order.OrderDetails)
            {
                var product = await _repository.Product.GetProductWithCateogriesAsync((int)od.CateogryId, (int)od.ProductId, true);
                
                if (product == null)
                    throw new ProductNotFoundException((int)od.ProductId);

                var newOd = newOrder.OrderDetails.Where(od => od.ProductId == product.Id).SingleOrDefault();

                if (newOd.Amount > product.Amount)
                    throw new NotEnoughAmountException();

                product.Amount -= newOd.Amount;
                newOd.UnitPrice = product.Price;
                newOrder.TotalPrice += newOd.UnitPrice * newOd.Amount;
            }
            newOrder.UserId = UserId;
            newOrder.OrderState = OrderState.Pending;
            newOrder.CreatedAt = DateTime.Now;

            _repository.Order.CreateOrder(newOrder);
            await _repository.SaveAsync();

            return _mapper.Map<OrderDto>(newOrder);

        }

        public async Task DeleteOrder(string UserId, int OrderId)
        {
            var user = await _repository.User.GetUserAsync(UserId);

            if (user == null)
                throw new UserNotFoundException(UserId);

            var order = await _repository.Order.GetOrderAsync(OrderId, true);

            if (order == null)
                throw new OrderNotFoundException(OrderId);

            _repository.Order.DeleteOrder(order);

            await _repository.SaveAsync();
        }

        public async Task<PagedList<OrderDto>> GetAllOrdersAsync(string UserId, OrderParameters orderablePartitioner, bool TrackChanges)
        {
            var user = await _repository.User.GetUserAsync(UserId);

            if (user == null)
                throw new UserNotFoundException(UserId);

            var order = await _repository.Order.GetAllOrdersAsync(UserId, orderablePartitioner, false);

            var res = _mapper.Map<List<OrderDto>>(order);

            return new PagedList<OrderDto>(res, order.MetaData.TotalCount, orderablePartitioner.PageNumber, orderablePartitioner.PageSize);
        }

        public async Task<OrderDto> GetOrderAsync(string UserId, int OrderId, bool TrackChanges)
        {
            var user = await _repository.User.GetUserAsync(UserId);

            if (user == null)
                throw new UserNotFoundException(UserId);

            var order = await _repository.Order.GetOrderAsync(OrderId, true);

            if (order == null)
                throw new OrderNotFoundException(OrderId);

            return _mapper.Map<OrderDto>(order);
        }
    }
}
