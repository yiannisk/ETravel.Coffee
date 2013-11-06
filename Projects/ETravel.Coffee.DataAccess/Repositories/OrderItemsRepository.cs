using System;
using System.Collections.Generic;
using ETravel.Coffee.DataAccess.Entities;
using ETravel.Coffee.DataAccess.Interfaces;
using NHibernate.Criterion;

namespace ETravel.Coffee.DataAccess.Repositories
{
	public class OrderItemsRepository : Repository, IOrderItemsRepository
	{
		public IList<OrderItem> ForOrderId(Guid orderId)
		{
			return Session
				.CreateCriteria<OrderItem>()
					.Add(Restrictions.Eq("OrderId", orderId)).List<OrderItem>();
		}

		public OrderItem GetById(Guid id)
		{
			return Session.Get<OrderItem>(id);
		}

		public void Save(OrderItem item)
		{
			Session.Save(item);
			Session.Flush();
		}

		public void Update(OrderItem item)
		{
			Session.Update(item);
			Session.Flush();
		}

		public void Delete(Guid id)
		{
			Session.Delete(GetById(id));
			Session.Flush();
		}
	}
}