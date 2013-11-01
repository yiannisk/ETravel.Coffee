using System;
using System.Collections.Generic;
using ETravel.Coffee.DataAccess.Entities;

namespace ETravel.Coffee.DataAccess.Interfaces
{
	public interface IOrdersRepository
	{
		IList<Order> All();
		void SaveOrUpdate(Order order);
		Order GetById(Guid id);
	}
}