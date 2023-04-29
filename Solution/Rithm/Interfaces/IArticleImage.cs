using Microsoft.AspNetCore.Components;

namespace Rithm
{
    public interface IArticleImage
    {
        string? Image { get; }
        string? ImageCredit => null;
        string? ImageCaption => null;
    }
}
