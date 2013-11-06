using System;
using System.Collections.Generic;
using System.Linq;
using ETravel.Coffee.DataAccess.Interfaces;
using Order = ETravel.Coffee.DataAccess.Entities.Order;

namespace ETravel.Coffee.DataAccess.Repositories
{
	public class OrdersRepository : Repository, IOrdersRepository
	{
		public virtual IList<Order> All()
		{
			return Session
				.CreateCriteria<Order>()
				.List<Order>()
				.OrderBy(x => x.ExpiresAt)
				.Reverse()
				.ToList();
		}

		public virtual void Save(Order order)
		{
			Session.Save(order);
			Session.Flush();
		}

		public virtual void Update(Order order)
		{
			Session.Update(order);
			Session.Flush();
		}

		public virtual void Delete(Guid id)
		{
			Session.Delete(GetById(id));
			Session.Flush();
		}

		public virtual Order GetById(Guid id)
		{
			return Session.Get<Order>(id);
		}
	}
}