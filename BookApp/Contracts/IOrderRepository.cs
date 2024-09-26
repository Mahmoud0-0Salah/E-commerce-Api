using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        public void CreateOrder(Order order);
        public Task<Order> GetOrderAsync( int OrderId, bool TrackChanges);
        public Task<PagedList<Order>> GetAllOrdersAsync(string UserId, OrderParameters orderablePartitioner, bool TrackChanges);
        public void DeleteOrder(Order order);
    }
}
