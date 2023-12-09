using Moq;
using Rithm.Blog;
using Rithm.Models;

namespace Rithm.Tests
{
    [TestClass]
    public class ArticleHelperTests
    {
        private ArticleHelper _target = default!;
        private Mock<IServiceProvider> _mockServiceProvider = new();
        private RithmOptions _rithmOptions = default!;

        private List<BlogArticle> _articlesToSearch = new();

        [TestInitialize]
        public void Setup()
        {
            _articlesToSearch = new List<BlogArticle>()
            {
                new BlogArticle()
                {
                    Title = "Web focused article",
                    LazyContent = new Lazy<Microsoft.AspNetCore.Components.MarkupString>(()=>new Microsoft.AspNetCore.Components.MarkupString("Test Content from blog post 1 which might be about blazor and mvc and razor pages."))
                },
                new BlogArticle()
                {
                    Title = "Db focused article",
                    LazyContent = new Lazy<Microsoft.AspNetCore.Components.MarkupString>(()=>new Microsoft.AspNetCore.Components.MarkupString("Test Content from blog post 2 which might be about mssql and postgresql and redis."))
                }
            };
            _rithmOptions = new();
            _target = new ArticleHelper(_mockServiceProvider.Object, _rithmOptions);
        }

        [TestMethod]
        public async Task TestSearchArticlesAsync()
        {
            var results = await _target.SearchArticlesAsync(new ArticleSearchParameters { Keywords = "mvc"}, _articlesToSearch, default);

            Assert.AreEqual(1, results.Count());
        }
    }
}