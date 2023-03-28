using Rithm.Articles.Abstract;
using Rithm.Articles.Interfaces;

namespace Rithm.Articles.Blog
{
    public class BlogSeries : List<BlogArticle>
    {
        public string? Series { get; }

        public DateTime SeriesStartDate { get; }

        public BlogSeries(IEnumerable<BlogArticle> blogArticlesInSeries) : base(blogArticlesInSeries)
        {
            Series = blogArticlesInSeries.FirstOrDefault()?.Series;
            SeriesStartDate = blogArticlesInSeries.OrderBy(ob => ob.Date)
                                                .Select(s=>s.Date)
                                                .First();
        }
    }
}
