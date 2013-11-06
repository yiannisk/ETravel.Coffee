using System.Linq;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
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
	}
}