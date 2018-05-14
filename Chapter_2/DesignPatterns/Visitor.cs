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
    }

    public class PlainText : DocumentPart
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
        void Visit(PlainText element);
        void Visit(BoldText element);
        void Visit(Hyperlink element);
    }

    public class HtmlVisitor : IVisitor
    {
        public string Output => _output.ToString();

        private StringBuilder _output = new StringBuilder();

        public void Visit(PlainText sedan)
        {
            //var children = VisitParts(sedan.Parts);
            //return $"<p>{sedan.Text}{children}</p>";

            _output.Append($"<p>{sedan.Text}\n");
            VisitParts(sedan.Parts);
            _output.Append($"</p>\n");
        }

        public void Visit(BoldText boldText)
        {
            _output.Append($"<b name=\"{boldText.Name}\">\n{boldText.Text}\n");
            VisitParts(boldText.Parts);
            _output.Append("</b>\n");
        }

        public void Visit(Hyperlink link)
        {
            _output.Append($"<a href=\"{link.Url}\">\n{link.Text}\n");
            VisitParts(link.Parts);
            _output.Append("</a>\n");
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
