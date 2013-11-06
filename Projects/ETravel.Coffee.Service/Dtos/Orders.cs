using System;

namespace ETravel.Coffee.Service.Dtos
{
	public class Orders
	{
		public virtual string ExpiresAt { get; set; }
		public virtual string Vendor { get; set; }
		public virtual string Owner { get; set; }
		public virtual string Id { get; set; }
	}
}