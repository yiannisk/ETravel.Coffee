using System;

namespace ETravel.Coffee.DataAccess.Entities
{
	public class OrderItem
	{
		protected internal virtual Guid Id { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string Description { get; set; }
		public virtual string Owner { get; set; }
	}
}