using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }

        public async Task<PagedList<Order>> GetAllOrdersAsync(string UserId, OrderParameters orderablePartitioner, bool TrackChanges)
        {
            if (orderablePartitioner.MaxPrice < orderablePartitioner.MinPrice)
                throw new MaxRangeBadRequestException();

            var orders = await FindByCondition(o=>o.UserId == UserId,TrackChanges)
                .Paging(orderablePartitioner.PageNumber, orderablePartitioner.PageSize)
                .Filter(orderablePartitioner.MinPrice, orderablePartitioner.MaxPrice)
                .Sort(orderablePartitioner.ordereby)
                .ToListAsync();


            var count = await FindByCondition(p => orders.Contains(p), TrackChanges).CountAsync();

            return new PagedList<Order>(orders, count, orderablePartitioner.PageNumber, orderablePartitioner.PageSize);
        }

        public async Task<Order> GetOrderAsync( int OrderId, bool TrackChanges)
        {
            return await FindByCondition(o => o.Id == OrderId,TrackChanges).SingleOrDefaultAsync();
        }
    }
}
