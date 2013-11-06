using System;
using System.Collections.Generic;
using ETravel.Coffee.DataAccess.Entities;

namespace ETravel.Coffee.DataAccess.Interfaces
{
	public interface IOrdersRepository
	{
		IList<Order> All();
		void Save(Order order);
		void Update(Order order);
		void Delete(Guid id);
		Order GetById(Guid id);
	}
}