using System;
using System.Globalization;
using System.Linq;
using System.Net;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ETravel.Coffee.Service.Services
{
	public class OrdersService : RestServiceBase<Orders>
	{
		public IOrderItemsRepository OrderItemsRepository { get; set; }
		public IOrdersRepository OrdersRepository { get; set; }

		public override object OnGet(Orders request)
		{
			return OrdersRepository.All()
				.Select(order => new Order
				{
					Id = order.Id.GetValueOrDefault(),
					ExpiresAt = order.ExpiresAt.HasValue ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", order.ExpiresAt.Value) : string.Empty,
					Interval = order.Interval,
					Owner = order.Owner,
					Vendor = order.Vendor
				})
				.ToList();
		}

		public override object OnPost(Orders request)
		{
			DateTime parsed;

			var orderId = Guid.NewGuid();
			OrdersRepository.Save(new DataAccess.Entities.Order
			{
				Id = Guid.NewGuid(),
				ExpiresAt = DateTime.TryParseExact(request.ExpiresAt, "yyyy-MM-dd HH:mm:ss", 
					CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsed) 
						? parsed 
						: default(DateTime?),

				Vendor = request.Vendor,
				Owner = request.Owner
			});

			var newOrder = OrdersRepository.GetById(orderId);
			
			return new Order
			{
				Id = newOrder.Id,
				ExpiresAt = newOrder.ExpiresAt.HasValue ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", newOrder.ExpiresAt) : null,
				Owner = newOrder.Owner,
				Vendor = newOrder.Vendor,
				Interval = newOrder.Interval
			};
		}

		public override object OnDelete(Orders request)
		{
			var orderItems = OrderItemsRepository.ForOrderId(new Guid(request.Id)).ToList();

			if (orderItems.Count == 0)
				return new HttpResult
				{
					StatusCode = (HttpStatusCode) 422,
					StatusDescription = "No order found for the given identifier."
				};
			
			// BUG: If more than one order items are contained in the order, it can 't be deleted.
			if (orderItems.Count > 1) return new HttpResult { StatusCode = HttpStatusCode.OK };

			orderItems.ForEach(x => OrderItemsRepository.Delete(x.Id.GetValueOrDefault()));

			OrdersRepository.Delete(new Guid(request.Id));

			return new HttpResult { StatusCode = HttpStatusCode.OK };
		}
	}
}