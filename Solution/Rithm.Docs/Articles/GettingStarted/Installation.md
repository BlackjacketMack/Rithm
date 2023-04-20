---
{
    "title": "Installation",
    "key":  "getting-started-installation",
    "categories":["getting-started"]
}
---

# Installation

<section>

##### Bootstrapping
* * *

Bootstrapping our articles without any addition configuration will look in the executing context's assemblies for any blazor components that implement `IArticle`.


    using Rithm;

    builder.Services.AddRithm();


</section>



<section>

##### Creating an Article
* * *

Articles can be created using any number of built-in ingestors or custom ingestors.  This will create a Blazor component article simply by inheriting from `ComponentArticle` and setting properties on the base class.  Once this is done your component-article is now part of the `ArticleCollection`.

    @inherits ComponentArticle

    @code{

        public SampleArticle()
        {
            base.Title = "Article title";
            base.Key = "getting-started-sample-article";
            base.Description = "This is a sample article that is part of the getting started section.";
            base.Date = new DateTime(2023, 04, 01);
        }
    }

<br />

>Note
    If inheriting from `ComponentArticle` is not possible for whatever reason you can simply `@implement IArticle` and the asssociated properties.  Many of the properties have default implementations (usually null) to make the implementation easier.

</section>

<section>

##### Accessing Articles
* * *

Now that your article is part of the `IArticleCollection` it is possible to query it.

    @page "/sample-list-of-articles"
    @inject IArticleHelper _articleHelper

    Below is a full list of articles registered on this site:
    
        @foreach (var article in _articles)
        {
            <div>
                @article.Title
                @article.Key
                @article.GetType()
            </div>
            <hr />
        }

    @code{

        [Parameter]
        public string ArticleKey{ get; set; }

        private IEnumerable<IArticle> _articles = Enumerable.Empty<IArticle>();

        protected override async Task OnInitializedAsync()
        {
            _articles = await _articleHelper.GetArticlesAsync(CancellationToken);
        }
    }

</section>