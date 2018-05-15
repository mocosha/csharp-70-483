using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{
    /// <summary>
    /// Visitor pattern links:
    /// https://www.codeproject.com/Articles/588882/TheplusVisitorplusPatternplusExplained
    /// http://www.dofactory.com/net/design-patterns    
    /// https://stackoverflow.com/questions/23321669/abstract-tree-with-visitors
    /// </summary>
    public abstract class DocumentPart
    {
        public DocumentPart()
        {
            Parts = new List<DocumentPart>();
        }
        public string Text { get; set; }
        public List<DocumentPart> Parts { get; set; }

        public abstract void Accept(IVisitor visitor);
        public bool HasChildren => Parts.Count > 0;
    }

    public class Paragraph : DocumentPart
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class BoldText : DocumentPart
    {
        public string Name { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Hyperlink : DocumentPart
    {
        public string Url { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public interface IVisitor
    {
        void Visit(Paragraph element);
        void Visit(BoldText element);
        void Visit(Hyperlink element);
    }

    public class HtmlVisitor : IVisitor
    {
        public string Output => _output.ToString();

        private StringBuilder _output = new StringBuilder();

        int tabCounter = 1;
        Func<int, string> GetTab = counter => new string('\t', counter);
        readonly Dictionary<int, string> tabHelper = new Dictionary<int, string>
        {
            { 1, "\t" },
            { 2, "\t\t"},
            { 3, "\t\t\t"},
            { 4, "\t\t\t\t"}
        };

        public void Visit(Paragraph paragraph)
        {
            //var children = VisitParts(sedan.Parts);
            //return $"<p>{sedan.Text}{children}</p>";

            var ret = paragraph.HasChildren ? "\n" : "";
            _output.Append($"{tabHelper[tabCounter]}<p>{paragraph.Text}{ret}");
            tabCounter++;
            VisitParts(paragraph.Parts);
            tabCounter--;

            if (paragraph.HasChildren)
                _output.Append($"{tabHelper[tabCounter]}</p>\n");
            else
                _output.Append($"</p>\n");

        }

        public void Visit(BoldText boldText)
        {
            var ret = boldText.HasChildren ? "\n" : "";
            _output.Append($"{tabHelper[tabCounter]}<b name=\"{boldText.Name}\">{boldText.Text}{ret}");
            tabCounter++;
            VisitParts(boldText.Parts);
            tabCounter--;

            if (boldText.HasChildren)
                _output.Append($"{tabHelper[tabCounter]}</b>\n");
            else
                _output.Append($"</b>\n");

        }

        public void Visit(Hyperlink link)
        {
            var ret = link.HasChildren ? "\n" : "";
            _output.Append($"{tabHelper[tabCounter]}<a href=\"{link.Url}\">{link.Text}{ret}");
            tabCounter++;
            VisitParts(link.Parts);
            tabCounter--;
            
            if (link.HasChildren)
                _output.Append($"{tabHelper[tabCounter]}</a>\n");
            else
                _output.Append($"</a>\n");
        }

        private void VisitParts(List<DocumentPart> parts)
        {
            foreach (var item in parts)
            {
                item.Accept(this);
            }
        }
    }

    public class Document
    {
        private List<DocumentPart> _parts = new List<DocumentPart>();

        public void AddDocument(DocumentPart docPart)
        {
            _parts.Add(docPart);
        }

        public void Detach(DocumentPart docPart)
        {
            _parts.Remove(docPart);
        }

        public void Accept(IVisitor visitor)
        {
            foreach (DocumentPart dp in _parts)
            {
                dp.Accept(visitor);
            }
        }
    }

}
