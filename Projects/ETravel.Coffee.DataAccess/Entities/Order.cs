using System;

namespace ETravel.Coffee.DataAccess.Entities
{
	public class Order
	{
		public virtual Guid? Id { get; set; }
		public virtual string Vendor { get; set; }
		public virtual DateTime? ExpiresAt { get; set; }
		public virtual long? Interval { get; set; }
		public virtual string Owner { get; set; }
	}
}