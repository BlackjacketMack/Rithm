namespace Rithm.Articles
{
    internal class IngestorInfo
    {
        public Type IngestorType { get; set; }
        public Action<IArticleIngestor>? ConfigActions { get; set; }
    }
}
