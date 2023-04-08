﻿using Microsoft.AspNetCore.Components;
using Rithm.Blog;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Rithm
{
    public class EmbeddedMarkdownIngestor : IArticleIngestor
    {
        private readonly RithmOptions _rithmOptions;

        public EmbeddedMarkdownIngestor(RithmOptions rithmOptions)
        {
            _rithmOptions = rithmOptions;
        }

        public Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var embeddedArticles = new List<IArticle>();
            foreach (var assembly in _rithmOptions.Assemblies)
            {
                //load .md article types
                var markdownResources = assembly.GetManifestResourceNames().Where(w => w.EndsWith(".md"));

                foreach (var resourceName in markdownResources)
                {
                    var parseResult = parseEmbeddedResource(assembly, resourceName);
                    var frontMatter = parseResult.frontMatter;
                    var content = parseResult.content;

                    if (frontMatter == null) continue;

                    var serializeOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    var embeddedArticle = JsonSerializer.Deserialize(frontMatter, typeof(BlogArticle), serializeOptions) as BlogArticle;

                    if (embeddedArticle == null) continue;

                    embeddedArticle.LazyContent = new Lazy<MarkupString>(() =>
                    {
                        var markdown = new MarkdownSharp.Markdown().Transform(content);
                        return new MarkupString(markdown);
                    });

                    embeddedArticles.Add(embeddedArticle);
                }
            }

            return Task.FromResult(embeddedArticles.AsEnumerable());
        }

        private (string? frontMatter, string? content) parseEmbeddedResource(Assembly assembly, string resourceName)
        {
            string? frontMatter = null;
            string? content = null;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return (null,null);
                using (StreamReader reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    Regex regex = new Regex("---([\\S\\s]*?)---");
                    var fullFrontMatter = regex.Match(result).Groups[0].ToString();
                    frontMatter = regex.Match(result).Groups[1].ToString();
                    content = result.Replace(fullFrontMatter, String.Empty);
                }
            }

            return (frontMatter, content);
        }
    }
}
