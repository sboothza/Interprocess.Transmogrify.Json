using NUnit.Framework;
using NUnit.Framework.Legacy;
namespace Interprocess.Transmogrify.Json.Tests;

static class Extensions
{
    public const string TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
    public static string ToBaseString(this DateTime timestamp) => timestamp.ToString(TimestampFormat);
}

public enum DateTimeZoneType
{
    UTC,
    Local,
    Unknown,
}

public class TestListObj
{
    [JsonProperty("list")]
    public List<string> List { get; set; }
}

public class Dummy
{
    public string Value { get; set; }
}

public class TestListObjObj
{
    [JsonProperty("list")]
    public List<Dummy> List { get; set; }
}

public class TestNaming
{
    public string StringValue { get; set; }
    public string OtherValue { get; set; }
    [JsonProperty("overridevalue")]
    public string OverrideValue { get; set; }
}

[TestFixture]
public class ComplexTest
{
    [Test]
    public void TestListObjectDeserialize()
    {
        var json = "{\n    \"list\":[\n        \"value1\",\n        \"value2\"\n    ]\n}";
        var obj = JsonSerializer.Deserialize<TestListObj>(json, new JsonSerializerOptions());
        ClassicAssert.AreEqual(obj.List.Count, 2);
        ClassicAssert.AreEqual(obj.List[0], "value1");
        ClassicAssert.AreEqual(obj.List[1], "value2");
        Console.WriteLine(obj);
    }

    [Test]
    public void TestListDeserialize()
    {
        var json = "[\n        \"value1\",\n        \"value2\"\n    ]";
        var obj = Interprocess.Transmogrify.Json.JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions());
        ClassicAssert.AreEqual(obj.Count, 2);
        ClassicAssert.AreEqual(obj[0], "value1");
        ClassicAssert.AreEqual(obj[1], "value2");
        Console.WriteLine(obj);
    }

    [Test]
    public void TestListObjectObjectDeserialize()
    {
        var json = "{\n    \"list\":[\n        {\"Value\":\"value1\"},\n        {\"Value\":\"value2\"}\n    ]\n}";
        var obj = Interprocess.Transmogrify.Json.JsonSerializer.Deserialize<TestListObjObj>(json, new JsonSerializerOptions());
        ClassicAssert.AreEqual(obj.List.Count, 2);
        ClassicAssert.AreEqual(obj.List[0].Value, "value1");
        ClassicAssert.AreEqual(obj.List[1].Value, "value2");
        Console.WriteLine(obj);
    }

    [Test]
    public void TestNaming()
    {
        var obj = new TestNaming
        {
            StringValue = "test string value",
            OverrideValue = "test override value",
            OtherValue = "test other value"
        };

        var options = new JsonSerializerOptions
        {
            Naming = NamingOptions.SnakeCase
        };

        var json = Interprocess.Transmogrify.Json.JsonSerializer.Serialize(obj, options);
        ClassicAssert.AreEqual("{\"string_value\" : \"test string value\",\"other_value\" : \"test other value\",\"overridevalue\" : \"test override value\"}", json);

        var newObj = Interprocess.Transmogrify.Json.JsonSerializer.Deserialize<TestNaming>(json, options);
        
        ClassicAssert.NotNull(newObj);
        ClassicAssert.AreEqual(newObj.StringValue, obj.StringValue);
        ClassicAssert.AreEqual(newObj.OverrideValue, obj.OverrideValue);
        ClassicAssert.AreEqual(newObj.OtherValue, obj.OtherValue);
    }
}