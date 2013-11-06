using System;
using System.Linq;
using System.Net;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ETravel.Coffee.Service.Services
{
	public class OrderItemsService : RestServiceBase<OrderItems>
	{
		public IOrderItemsRepository OrderItemsRepository { get; set; }

		public override object OnGet(OrderItems request)
		{
			return OrderItemsRepository.ForOrderId(request.OrderId)
				.Select(orderItem => new OrderItem
				{
					Id = orderItem.Id.GetValueOrDefault(),
					Owner = orderItem.Owner,
					Description = orderItem.Description,
					OrderId = orderItem.OrderId,
					Quantity = orderItem.Quantity
				})
				.ToList();
		}

		public override object OnPost(OrderItems request)
		{
			OrderItemsRepository.Save(new DataAccess.Entities.OrderItem
			{
				Description = request.Description,
				OrderId = request.OrderId,
				Id = Guid.NewGuid(),
				Owner = request.Owner,
				Quantity = request.Quantity
			});

			return new HttpResult { StatusCode = HttpStatusCode.Created };
		}
	}
}