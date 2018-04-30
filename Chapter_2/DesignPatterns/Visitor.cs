using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Output { get; set; }

        public void Visit(PlainText sedan)
        {
            Output += sedan.Text;
            VisitParts(sedan.Parts);
        }

        private void VisitParts(List<DocumentPart> parts)
        {
            foreach (var item in parts)
            {
                item.Accept(this);
            }
        }

        public void Visit(BoldText boldText)
        {
            Output += "<b>" + boldText.Text + "</b>"; ;
            VisitParts(boldText.Parts);
        }

        public void Visit(Hyperlink link)
        {
            Output += "<a href=\"" + link.Url + "\">" + link.Text + "</a>"; ;
            VisitParts(link.Parts);
        }
    }

    public class Document
    {
        private List<DocumentPart> _parts = new List<DocumentPart>();

        public void AddCar(DocumentPart docPart)
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
