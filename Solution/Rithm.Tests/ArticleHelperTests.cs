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

        [TestMethod]
        public async Task TestSearchArticlesAsync_Partial()
        {
            var results = await _target.SearchArticlesAsync(new ArticleSearchParameters { Keywords = "mvc" }, _articlesToSearch, default);

            Assert.AreEqual(1, results.Count());
        }

        [DataRow("from blog post",100,129)]     //second entry picks up 'post' from 'postgresql'
        [DataRow("FrOm BlOg PoSt",100,129)]
        [TestMethod]
        public async Task TestSearchArticlesAsync_Multiple(string searchTerms, int expectedScoreFirst, int expectedScoreSecond)
        {
            var results = await _target.SearchArticlesAsync(new ArticleSearchParameters { Keywords = searchTerms }, _articlesToSearch, default);

            var result1 = results.First();
            Assert.AreEqual(expectedScoreFirst, result1.Score);

            var result2 = results.Skip(1).First();
            Assert.AreEqual(expectedScoreSecond, result2.Score);
        }

        [DataRow("you read", 88)]                           //second entry picks up 'post' from 'postgresql'
        [DataRow("box bed letter read three back", 85)]     //all partials
        [DataRow("but in a box beneath letter", 99)]     //so close...but no exact match to the phrase so limited to 99
        [TestMethod]
        public async Task TestSearchArticlesAsync_MoreTests(string searchTerms, int expectedScoreFirst)
        {
            var blogArticle = new BlogArticle
            {
                Title = "But in a box beneath my bed Is a letter that you never read From three summers back. - Taylor Swift",
                Subtitle = "some subtitle"
            };

            var results = await _target.SearchArticlesAsync(new ArticleSearchParameters { Keywords = searchTerms }, new[] { blogArticle }, default);

            var result1 = results.First();
            Assert.AreEqual(expectedScoreFirst, result1.Score);
        }
    }
}