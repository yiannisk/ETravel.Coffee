using System.Net;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.DataAccess.Repositories;
using ETravel.Coffee.Service.Dtos;
using Funq;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Formats.MessagePack;

namespace ETravel.Coffee.Service
{
	public class CoffeeAppHost : AppHostBase
	{
		public CoffeeAppHost() : base("Coffee Service", typeof(CoffeeAppHost).Assembly) {}

		public override void Configure(Container container)
		{
			ConfigureRoutes();
			ConfigureDependencies(container);
			ConfigureNotFound();

			SetConfig(new EndpointHostConfig { DebugMode = false });

			log4net.Config.XmlConfigurator.Configure();
			MessagePackFormat.Register(this);
		}

		private void ConfigureNotFound()
		{
			ResponseFilters.Add((request, response, obj) =>
			{
				if (obj == null)
					response.StatusCode = (int) HttpStatusCode.NotFound;
			});
		}

		private void ConfigureDependencies(Container container)
		{
			container.Register<IOrderItemsRepository>(new OrderItemsRepository());
			container.Register<IOrdersRepository>(new OrdersRepository());
		}

		private void ConfigureRoutes()
		{
			Routes
				.Add<Orders>("/orders", "GET")
				.Add<Orders>("/orders", "POST")
				.Add<Orders>("/orders", "DELETE")
				.Add<OrderItems>("/orders/{OrderId}/items", "GET")
				.Add<OrderItems>("/orders/{OrderId}/items", "POST")
				.Add<OrderItems>("/orders/{OrderId}/items", "DELETE")
				;
		}

		/// <summary>
		/// Start the service setting it up. 
		/// 
		/// Used by WebActivator.PostApplicationStartMethod
		/// </summary>
		public static void Start()
		{
			new CoffeeAppHost().Init();
		}
	}
}