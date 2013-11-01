using System;

namespace ETravel.Coffee.DataAccess.Entities
{
	public class Order
	{
		protected internal virtual Guid Id { get; set; }
		public virtual string Vendor { get; set; }
		public virtual DateTime? ExpiresAt { get; set; }
		public virtual TimeSpan? Interval { get; set; }
		public virtual string Owner { get; set; }
	}
}