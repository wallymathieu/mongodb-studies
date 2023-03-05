using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicMongoDbApp.Core;
using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
namespace SomeBasicMongoDbApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{
		private IMongoDatabase _engine;
		[Test]
		public void CanGetCustomerById()
		{
			Assert.IsNotNull(_engine.GetCollection<Customer>("customers").AsQueryable().SingleOrDefault(c=>c.Id==1));
		}

		[Test]
		public void CanGetProductById()
		{
			Assert.IsNotNull(_engine.GetCollection<Product>("products").AsQueryable().SingleOrDefault(c => c.Id == 1));
		}

		[SetUp]
		public void Setup()
		{
		}


		[TearDown]
		public void TearDown()
		{
		}

		[OneTimeSetUp]
		public void TestFixtureSetup()
		{
			var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) 
			                                                    ||( type?.Namespace?.StartsWith("SomeBasicMongoDbApp.Core") ?? false));
			BsonSerializer.RegisterSerializer(objectSerializer);
			_engine = new MongoClient(new MongoClientSettings()).GetDatabase("mongodb");

			XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
							(type, obj) =>
							{
								switch (obj)
								{
									case Customer customer:
										_engine.GetCollection<Customer>("customers").InsertOne(customer);
										break;
									case Product product:
										_engine.GetCollection<Product>("products").InsertOne(product);
										break;
									case Order order:
										_engine.GetCollection<Order>("orders").InsertOne(order);
										break;
								}
							}, "http://tempuri.org/Database.xsd");


		}


		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
		}
	}
}
