using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ServiceStack.ServiceInterface;

namespace ETravel.Coffee.Service.Services
{
	public class OrdersService : RestServiceBase<Orders>
	{
		public IOrdersRepository OrdersRepository { get; set; }

		public override object OnGet(Orders request)
		{
			return OrdersRepository.All();
		}
	}
}