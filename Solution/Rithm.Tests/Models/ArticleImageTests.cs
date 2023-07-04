using Rithm.Models;
using System.Text.Json;

namespace Rithm.Tests;

[TestClass]
public class ArticleImageTests
{
    private ArticleImage _target = default!;

    private string serialize<T>(T obj) => JsonSerializer.Serialize(obj, JsonHelper.GetJsonSerializerOptions());
    private T? deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, JsonHelper.GetJsonSerializerOptions());

    [TestInitialize]
    public void Setup()
    {
    }

    [TestMethod]
    public void TestConstructor()
    {
        
    }

    [TestMethod]
    public void TestBasic()
    {
        var articleImage = new ArticleImage
        {
            Url = "http://foo.com",
            Caption = "Some caption"
        };

        var json = serialize(articleImage);

        Console.WriteLine(json);

        var image = deserialize<ArticleImage>(json);

        Assert.AreEqual(articleImage.Url, image.Url);
    }

    [TestMethod]
    public void TestDeserialize_JustString()
    {
        var json = """
                "foo://foo.com"
                """;

        var articleImage = deserialize<ArticleImage?>(json);

        Assert.AreEqual("foo://foo.com", articleImage.Url);
    }
}