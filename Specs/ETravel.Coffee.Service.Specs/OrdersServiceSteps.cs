using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ETravel.Coffee.DataAccess.Interfaces;
using ETravel.Coffee.Service.Dtos;
using ETravel.Coffee.Service.Services;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ServiceStack.Common.Web;
using TechTalk.SpecFlow;

namespace ETravel.Coffee.Service.Specs
{
    [Binding]
    public class OrdersServiceSteps
    {
		private List<DataAccess.Entities.Order> GetOrdersFixture { get; set; }
		private List<DataAccess.Entities.OrderItem> GetOrderItemsFixture { get; set; }
		private List<Order> GetOrdersResponse { get; set; }
		private OrdersService Service { get; set; }
		private static Fixture Fixture { get { return new Fixture();  } }
		private Order PostNewOrderResponse { get; set; }
		private HttpResult DeleteOrderResponse { get; set; }
		private Orders PostNewOrder { get; set; }
		private Orders DeletedOrder { get; set; }

    	public OrdersServiceSteps()
    	{
    		GetOrdersFixture = Fixture.CreateMany<DataAccess.Entities.Order>().ToList();
    		GetOrderItemsFixture = Fixture.CreateMany<DataAccess.Entities.OrderItem>().ToList();

			var ordersRepoMock = new Mock<IOrdersRepository>();
			
			ordersRepoMock
				.Setup(x => x.All())
				.Returns(GetOrdersFixture);
			
			ordersRepoMock
				.Setup(x => x.Save(It.IsAny<DataAccess.Entities.Order>()))
				.Callback<DataAccess.Entities.Order>(order => GetOrdersFixture.Add(order));

    		ordersRepoMock
    			.Setup(x => x.GetById(It.IsAny<Guid>()))
    			.Returns<Guid>(id => GetOrdersFixture.Single(x => x.Id == id));
		
			ordersRepoMock
				.Setup(x => x.Delete(It.IsAny<Guid>()))
				.Callback<Guid>(id => GetOrdersFixture.Remove(GetOrdersFixture.Single(x => x.Id.HasValue && x.Id.Value == id)));


    		var orderItemsRepoMock = new Mock<IOrderItemsRepository>();

    		orderItemsRepoMock.Setup(x => x.ForOrderId(It.IsAny<Guid>())).Returns(GetOrderItemsFixture);

			Service = new OrdersService
			{
				OrdersRepository = ordersRepoMock.Object, 
				OrderItemsRepository = orderItemsRepoMock.Object
			};
    	}

        [When(@"I place a GET request to the orders service")]
        public void WhenIPlaceAGETRequestToTheService()
        {
        	GetOrdersResponse = Service.Get(null) as List<Order>;
        }
        
        [Then(@"I should get back all the orders currently in the database")]
        public void ThenIShouldGetBackAllTheOrdersCurrentlyInTheDatabase()
        {
        	foreach (var order in GetOrdersResponse)
        	{
        		var fixture = GetOrdersFixture.Single(x => x.Id == order.Id);

				Assert.AreEqual(order.ExpiresAt, fixture.ExpiresAt.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"));
				Assert.AreEqual(order.Interval, fixture.Interval);
				Assert.AreEqual(order.Owner, fixture.Owner);
				Assert.AreEqual(order.Vendor, fixture.Vendor);
        	}
        }

		[When(@"I place a POST request to the orders service for a new order")]
		public void WhenIPlaceAPOSTRequestToTheOrdersServiceForANewOrder()
		{
			PostNewOrder = new Orders
			{
				ExpiresAt = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"),
				Owner = "Owner",
				Vendor = "Vendor"
			};

			PostNewOrderResponse = Service.Post(PostNewOrder) as Order;
		}

		[Then(@"The orders service should respond with the created order")]
		public void ThenTheOrdersServiceShouldRespondWithStatusCreated()
		{
			Assert.AreEqual(PostNewOrder.Owner, PostNewOrderResponse.Owner);
			Assert.AreEqual(PostNewOrder.Vendor, PostNewOrderResponse.Vendor);
			Assert.AreEqual(PostNewOrder.ExpiresAt, PostNewOrderResponse.ExpiresAt);
		}

		[Then(@"The new order should be among them")]
		public void ThenTheNewOrderShouldBeAmongThem()
		{
			Assert.IsNotNull(GetOrdersResponse
				.SingleOrDefault(x => x.ExpiresAt == PostNewOrder.ExpiresAt
								   && x.Owner == PostNewOrder.Owner
								   && x.Vendor == PostNewOrder.Vendor));
		}

		[When(@"I place a DELETE request to the orders service for an existing order with one order item")]
		public void WhenIPlaceADELETERequestToTheOrdersServiceForAnExistingOrderWithOneOrderItem()
		{
			var fixture = GetOrdersFixture.First();

			var orderItemsFixtures = GetOrderItemsFixture.Skip(1).ToList();
			GetOrderItemsFixture.RemoveRange(1, GetOrderItemsFixture.Count - 1);

			DeletedOrder = new Orders { Id = fixture.Id.GetValueOrDefault().ToString() };
			DeleteOrderResponse = Service.Delete(DeletedOrder) as HttpResult;

			GetOrderItemsFixture.AddRange(orderItemsFixtures);
		}

		[When(@"I place a DELETE request to the orders service for an existing order with multiple order items")]
		public void WhenIPlaceADELETERequestToTheOrdersServiceForAnExistingOrderWithMultipleOrderItems()
		{
			var fixture = GetOrdersFixture.First();

			DeletedOrder = new Orders { Id = fixture.Id.GetValueOrDefault().ToString() };
			DeleteOrderResponse = Service.Delete(DeletedOrder) as HttpResult;
		}

		[Then(@"The orders service should respond with status Ok")]
		public void ThenTheOrdersServiceShouldRespondWithStatusOk()
		{
			Assert.AreEqual(HttpStatusCode.OK, DeleteOrderResponse.StatusCode);
		}

		[Then(@"The deleted order should not be among them")]
		public void ThenTheDeletedOrderShouldNotBeAmongThem()
		{
			Assert.IsNull(GetOrdersResponse
				.SingleOrDefault(x => x.Id.GetValueOrDefault().ToString() == DeletedOrder.Id));
		}

    }
}
