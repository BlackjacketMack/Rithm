namespace Rithm.Tests
{
    [TestClass]
    public class ArticleParametersTests
    {
        private ArticleParameters _target = default!;

        [TestInitialize]
        public void Setup()
        {
            _target = new ArticleParameters();
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(_target.Keys);
            Assert.IsNotNull(_target.Tags);
            Assert.IsNotNull(_target.Kinds);
            Assert.IsNotNull(_target.Categories);
        }
    }
}