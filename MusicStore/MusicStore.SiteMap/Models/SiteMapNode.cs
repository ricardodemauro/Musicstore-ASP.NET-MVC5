using System.Collections.Generic;

namespace MusicStore.SiteMap.Models
{
    public class SiteMapNode
    {
        public string Uri { get; set; }

        public string Title { get; set; }

        public bool IsRoot { get; set; }

        public SiteMapNode Parent { get; set; }

        public IList<SiteMapNode> Childrens { get; private set; }

        public static SiteMapNode Empty => new SiteMapNode();

        public bool IsEmpty { get { return string.IsNullOrEmpty(Uri); } }

        public SiteMapNode()
        {
            Childrens = new List<SiteMapNode>();
        }

        public SiteMapNode CreateChildren()
        {
            var node = Empty;
            node.Parent = this;
            Childrens.Add(node);
            return node;
        }
    }
}
