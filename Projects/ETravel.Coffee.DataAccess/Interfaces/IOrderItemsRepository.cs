using System;
using System.Collections.Generic;
using ETravel.Coffee.DataAccess.Entities;

namespace ETravel.Coffee.DataAccess.Interfaces
{
	public interface IOrderItemsRepository
	{
		IList<OrderItem> ForOrderId(Guid orderId);
		OrderItem GetById(Guid id);
		void Save(OrderItem item);
		void Update(OrderItem item);
		void Delete(Guid id);
	}
}