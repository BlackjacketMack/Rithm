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
    using Rithm;

    builder.Services.AddRithm();


<br />
Bootstrapping our articles without any addition configuration will look in the executing context's assemblies for any blazor components that implement `IArticle`.

</section>