using ETravel.Coffee.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace ETravel.Coffee.DataAccess.Mappings
{
	public sealed class OrderItemMap : ClassMap<OrderItem>
	{
		public OrderItemMap()
		{
			Table("OrderItems");

			Id(x => x.Id).GeneratedBy.Assigned();

			Map(x => x.OrderId);
			Map(x => x.Quantity);
			Map(x => x.Owner);
			Map(x => x.Description);
		}
	}
}