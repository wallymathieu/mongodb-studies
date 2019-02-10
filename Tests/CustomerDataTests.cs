using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicMongoDbApp.Core;
using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq;
using MongoDB.Driver;
namespace SomeBasicMongoDbApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{
		private MongoDatabase _engine;
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

			_engine = new MongoServer(new MongoServerSettings()).GetDatabase("mongodb");

			XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
							(type, obj) =>
							{
								if (obj is Customer)
								{
									_engine.GetCollection<Customer>("customers").Insert(obj);
								}
								if (obj is Product)
								{
									_engine.GetCollection<Product>("products").Insert(obj);
								}
								if (obj is Order)
								{
									_engine.GetCollection<Product>("orders").Insert(obj);
								}
							}, "http://tempuri.org/Database.xsd");


		}


		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
		}
	}
}
