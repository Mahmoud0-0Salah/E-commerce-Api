using Entities.Models;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IOrderService
    {
        public Task ChangeOrderState(string UserId,int OrderId, OrderState orderState);
        public Task<PagedList<OrderDto>> GetAllOrdersAsync(string UserId, OrderParameters orderablePartitioner, bool TrackChanges);
        public Task<PagedList<OrderDto>> GetAllDeliveredOrdersAsync(OrderParameters orderablePartitioner, bool TrackChanges);
        public Task<PagedList<OrderDto>> GetAllShippedOrdersAsync( OrderParameters orderablePartitioner, bool TrackChanges);
        public Task<PagedList<OrderDto>> GetAllPendingOrdersAsync(OrderParameters orderablePartitioner, bool TrackChanges);
        public Task<OrderDto> GetOrderAsync(string UserId,int OrderId, bool TrackChanges);
        public Task DeleteOrder(string UserId,int OrderId);
        public Task<OrderDto> CreateOrder(string UserId, OrderForCreationDto order);

    }
}
