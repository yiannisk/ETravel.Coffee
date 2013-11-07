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
		public IOrdersRepository OrdersRepository { get; set; }
		public IOrderItemsRepository OrderItemsRepository { get; set; }

		public override object OnGet(OrderItems request)
		{
			var orderItems = OrderItemsRepository.ForOrderId(request.OrderId)
				.Select(orderItem => new OrderItem
				{
					Id = orderItem.Id.GetValueOrDefault(),
					Owner = orderItem.Owner,
					Description = orderItem.Description,
					OrderId = orderItem.OrderId,
					Quantity = orderItem.Quantity
				})
				.ToList();

			return orderItems.Count == 0 ? null : orderItems;
		}

		public override object OnPost(OrderItems request)
		{
			var order = OrdersRepository.GetById(request.OrderId);

			if (order == null)
				return new HttpResult
				{
					StatusCode = (HttpStatusCode) 422, 
					StatusDescription = "No order was found for the given OrderId."
				};

			var newOrderItemId = Guid.NewGuid();

			OrderItemsRepository.Save(new DataAccess.Entities.OrderItem
			{
				Description = request.Description,
				OrderId = request.OrderId,
				Id = newOrderItemId,
				Owner = request.Owner,
				Quantity = request.Quantity
			});

			var newOrderItem = OrderItemsRepository.GetById(newOrderItemId);

			return new OrderItem
			{
				Description = newOrderItem.Description,
				OrderId = newOrderItem.OrderId,
				Owner = newOrderItem.Owner,
				Quantity = newOrderItem.Quantity,
				Id = newOrderItemId
			};
		}

		public override object OnDelete(OrderItems request)
		{
			var order = OrdersRepository.GetById(request.OrderId);

			if (order == null)
				return new HttpResult
				{
					StatusCode = (HttpStatusCode)422,
					StatusDescription = "No order was found for the given OrderId."
				};

			var orderItem = OrderItemsRepository.GetById(new Guid(request.Id));

			if (orderItem == null)
				return new HttpResult
				{
					StatusCode = (HttpStatusCode) 422, 
					StatusDescription = "No order item was found for the given identifier."
				};

			OrderItemsRepository.Delete(orderItem.Id.GetValueOrDefault());

			return new HttpResult { StatusCode = HttpStatusCode.OK };
		}
	}
}