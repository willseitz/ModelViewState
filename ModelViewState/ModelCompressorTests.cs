using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace OpenSource.ModelViewState
{
	[Serializable]
	public class Fruit
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Calories { get; set; }
		public List<Flavor> Flavors { get; set; }
	}
	[Serializable]
	public class Flavor
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}


	[TestClass()]
	public class ModelCompressorTest
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}


		/// <summary>
		///A test for ModelCompressor Constructor
		///</summary>
		// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
		// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
		// whether you are testing a page, web service, or a WCF service.
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("C:\\Users\\william.seitz\\Documents\\Visual Studio 2010\\Projects\\Experiments\\Experiments", "/")]
		[UrlToTest("http://localhost:4837/")]
		public void ModelCompressorConstructorTest()
		{
			Fruit fruit = new Fruit { Id = 5, Name = "Banana", Calories = 100, Flavors = new List<Flavor> {new Flavor {Id=1,Name="Sweet"}, new Flavor {Id=2,Name="Gooey"}}};

			string jam = ModelCompressor.Compress(fruit);
			Fruit freezeDried = (Fruit)ModelCompressor.Decompress(typeof(Fruit), jam);

			Assert.AreNotEqual(fruit, freezeDried);
			Assert.AreEqual(fruit.Id, freezeDried.Id);
			Assert.AreEqual(fruit.Name, freezeDried.Name);
		}

		/// <summary>
		///A test for Compress
		///</summary>
		// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
		// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
		// whether you are testing a page, web service, or a WCF service.
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("C:\\Users\\william.seitz\\Documents\\Visual Studio 2010\\Projects\\Experiments\\Experiments", "/")]
		[UrlToTest("http://localhost:4837/")]
		public void CompressTest()
		{
			object model = null; // TODO: Initialize to an appropriate value
			string expected = string.Empty; // TODO: Initialize to an appropriate value
			string actual;
			actual = ModelCompressor.Compress(model);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
