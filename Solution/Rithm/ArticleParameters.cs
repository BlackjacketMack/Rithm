namespace Rithm
{
    public class ArticleParameters
    {
        public IEnumerable<string> Keys { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Kinds { get; set; } = Enumerable.Empty<string>();
        public Version? MinimumVersion { get; set; }
    }
}
