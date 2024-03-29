﻿using Rithm;

namespace Rithm.Documentation
{
    public class DocumentationArticle : MarkdownArticle
    {
        public sealed override string Kind { get; set; } = "Docs";

        /// <summary>
        /// Draft sets the major version to 0.
        /// </summary>
        public bool Draft
        {
            get => base.Version.Major < 1;
            set{
                if(value)
                    base.Version = new Version(0,base.Version.Minor,base.Version.Build,base.Version.Revision);
                
            }
        }
    }
}
