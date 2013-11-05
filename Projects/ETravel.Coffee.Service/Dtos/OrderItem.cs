using System;

namespace ETravel.Coffee.Service.Dtos
{
	public class OrderItem
	{
		public virtual Guid Id { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string Description { get; set; }
		public virtual string Owner { get; set; }
	}
}