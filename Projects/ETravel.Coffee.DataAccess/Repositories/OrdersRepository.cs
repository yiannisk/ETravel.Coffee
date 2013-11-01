using System;
using System.Collections.Generic;
using ETravel.Coffee.DataAccess.Interfaces;
using Order = ETravel.Coffee.DataAccess.Entities.Order;

namespace ETravel.Coffee.DataAccess.Repositories
{
	public class OrdersRepository : Repository, IOrdersRepository
	{
		public virtual IList<Order> All()
		{
			return Session.CreateCriteria<Order>().List<Order>();
		}

		public virtual void SaveOrUpdate(Order order)
		{
			Session.SaveOrUpdate(order);
		}

		public virtual Order GetById(Guid id)
		{
			return Session.Get<Order>(id);
		}
	}
}