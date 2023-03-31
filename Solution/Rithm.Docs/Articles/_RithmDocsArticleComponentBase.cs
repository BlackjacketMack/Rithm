using Rithm;

public abstract class RithmDocsArticleComponentBase : ComponentArticle
{
    private CancellationTokenSource? _cancellationTokenSource;

    protected CancellationToken CancellationToken => (_cancellationTokenSource ??= new()).Token;

    public virtual void Dispose()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}