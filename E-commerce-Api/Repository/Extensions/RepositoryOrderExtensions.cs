using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryOrderExtensions
    {

        public static IQueryable<Order> Paging(this IQueryable<Order> Orders, int PageNumber, int PageSize)
        {
            return Orders
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize);
        }
        public static IQueryable<Order> Filter(this IQueryable<Order> Orders, uint minprice, uint maxprice) => Orders.Where(e => (e.TotalPrice >= minprice && e.TotalPrice <= maxprice));

        public static IQueryable<Order> Sort(this IQueryable<Order> Orders, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return Orders;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Order>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return Orders;

            return Orders.OrderBy(orderQuery);
        }

    }
}
