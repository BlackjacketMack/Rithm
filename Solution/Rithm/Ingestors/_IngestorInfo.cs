namespace Rithm
{
    internal class IngestorInfo
    {
        public Type IngestorType { get; set; } = default!;
        public Action<IArticleIngestor>? ConfigActions { get; set; }
    }
}
