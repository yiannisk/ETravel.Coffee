using ETravel.Coffee.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace ETravel.Coffee.DataAccess.Mappings
{
	public sealed class OrderMap : ClassMap<Order>
	{
		public OrderMap()
		{
			Table("Order");

			Id(x => x.Id).GeneratedBy.Assigned();
			
			Map(x => x.Vendor);
			Map(x => x.ExpiresAt);
			Map(x => x.Interval);
			Map(x => x.Owner);
		}
	}
}