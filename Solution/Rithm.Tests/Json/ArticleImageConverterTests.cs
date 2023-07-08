using Rithm.Models;
using System.Text.Json;

namespace Rithm.Tests
{
    [TestClass]
    public class ArticleImageConverterTests
    {

        private JsonSerializerOptions _options = new()!;

        public class Foo
        {
            public string Name { get; set; }
            public ArticleImage Image { get; set; } = default!;
        }

        [TestInitialize]
        public void Setup()
        {
            _options.PropertyNameCaseInsensitive = true;
            _options.Converters.Add(new ArticleImageConverter());
        }

        private string serialize<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj, _options);
        }

        private T? deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        [TestMethod]
        public void TestSerialize_WorksAsUsual()
        {
            var articleImage = new ArticleImage
            {
                Url = "http://foo.com/bar.png",
                Caption = "hi there",
                Credit = "Squidward"
            };

            var json = serialize(articleImage);

            var expected = """
                {"Url":"http://foo.com/bar.png","Credit":"Squidward","Caption":"hi there"}
                """;

            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void TestDeserializeFromString()
        {
            var image = "http://foo.com/bar.png";

            var json = $"\"{image}\"";

            var articleImage = deserialize<ArticleImage>(json);

            Assert.AreEqual(image,articleImage.Url);
        }

        [TestMethod]
        public void TestDeserializeFromStringAsProperty()
        {
            var fooJson = """
                {
                    "name":"Rasputin",
                    "image":"http://foo.com/bar.png"
                }
                """;

            var image = "http://foo.com/bar.png";

            var json = $"\"{image}\"";

            var foo = deserialize<Foo>(fooJson);

            Assert.AreEqual("Rasputin", foo.Name);
            Assert.AreEqual("http://foo.com/bar.png", foo.Image.Url);
        }

        [TestMethod]
        public void TestDeserializeRegular()
        {

            var json = """
                {
                    "url":"http://foo.com/bar.png",
                    "caption":"some caption"
                }
                """;

            var articleImage = deserialize<ArticleImage>(json);

            Assert.AreEqual("http://foo.com/bar.png", articleImage.Url);
            Assert.AreEqual("some caption", articleImage.Caption);
        }
        
        [TestMethod]
        public void TestDeserializeRegularAsProperty()
        {
            var fooJson = """
                {
                    "name":"Rasputin",
                    "image":{
                        "url":"http://foo.com/bar.png",
                        "caption":"some caption"
                    }
                }
                """;

            var image = "http://foo.com/bar.png";

            var json = $"\"{image}\"";

            var foo = deserialize<Foo>(fooJson);

            Assert.AreEqual("Rasputin", foo.Name);
            Assert.AreEqual("http://foo.com/bar.png", foo.Image.Url);
            Assert.AreEqual("some caption", foo.Image.Caption);
        }
    }
}