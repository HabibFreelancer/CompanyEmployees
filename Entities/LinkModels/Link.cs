using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        /*The Href property defines the URI
to the action*/
        public string? Href { get; set; }
        /*the Rel property defines the identification of the action
type*/
        public string? Rel { get; set; }
        /* the Method property defines which HTTP method should be
used for that action*/
        public string? Method { get; set; }
        public Link()
        { }
        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

    }
}
