using System;
using System.Globalization;
using System.Linq;
using System.Net;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace ETravel.Coffee.Service.Services
{
	public class OrdersService : RestServiceBase<Orders>
	{
		public IOrdersRepository OrdersRepository { get; set; }

		public override object OnGet(Orders request)
		{
			return OrdersRepository.All()
				.Select(order => new Order
				{
					Id = order.Id.GetValueOrDefault(),
					ExpiresAt = order.ExpiresAt.HasValue ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", order.ExpiresAt.Value) : string.Empty,
					Interval = order.Interval,
					Owner = order.Owner,
					Vendor = order.Vendor
				})
				.ToList();
		}

		public override object OnPost(Orders request)
		{
			DateTime parsed;

			OrdersRepository.Save(new DataAccess.Entities.Order
			{
				Id = Guid.NewGuid(),
				ExpiresAt = DateTime.TryParseExact(request.ExpiresAt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsed) 
					? parsed 
					: default(DateTime?),

				Vendor = request.Vendor,
				Owner = request.Owner
			});

			return new HttpResult { StatusCode = HttpStatusCode.Created };
		}

		public override object OnDelete(Orders request)
		{
			OrdersRepository.Delete(new Guid(request.Id));

			return new HttpResult { StatusCode = HttpStatusCode.OK };
		}
	}
}