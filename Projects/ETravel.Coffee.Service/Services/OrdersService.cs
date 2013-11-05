using System.Linq;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ServiceStack.ServiceInterface;

namespace ETravel.Coffee.Service.Services
{
	public class OrdersService : RestServiceBase<Orders>
	{
		public IOrdersRepository OrdersRepository { get; set; }

		public override object OnGet(Orders request)
		{
			return OrdersRepository.All()
				.Select(order => new Order
				{
					Id = order.Id,
					ExpiresAt = order.ExpiresAt.HasValue ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", order.ExpiresAt.Value) : string.Empty,
					Interval = order.Interval,
					Owner = order.Owner,
					Vendor = order.Vendor
				})
				.ToList();
		}
	}
}