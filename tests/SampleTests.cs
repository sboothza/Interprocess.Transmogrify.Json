using Interprocess.Transmogrify.Json.Tests.DAL;

using NUnit.Framework;

namespace Interprocess.Transmogrify.Json.Tests
{
	[TestFixture]
	public class SampleTests
	{
		[Test]
		public void TestDeserialize()
		{
			var json = File.ReadAllText("dp2.json");

			var options = new JsonSerializerOptions { DontSerializeNulls = true, IgnorePropertyAttributes = true, Naming = NamingOptions.PropertyName };
			var obj = JsonSerializer.Deserialize<MarketListRoot>(json, options);
			Assert.That(obj, Is.Not.Null);

			options = new JsonSerializerOptions { DontSerializeNulls=true, Naming = NamingOptions.SnakeCase };
			var newJson = JsonSerializer.Serialize(obj, options);

			var newObj = JsonSerializer.Deserialize<MarketListRoot>(newJson, options);
			Assert.That(newObj, Is.Not.Null);
		}
	}
}
