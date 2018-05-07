using System;
using static DesignPatterns.CORAndSpecification;

namespace DesignPatterns
{
    public class CORAndSpecification
    {
        public interface ISpecification<T>
        {
            bool IsSatisfiedBy(T o);
        }

        public class Specification<T> : ISpecification<T>
        {
            private readonly Func<T, bool> Expression;

            public Specification(Func<T, bool> expression)
            {
                if (expression == null)
                    throw new ArgumentNullException();
                else
                    Expression = expression;
            }

            public bool IsSatisfiedBy(T o)
            {
                return Expression(o);
            }
        }

        public interface IHandler<T>
        {
            void SetSuccessor(IHandler<T> handler);
            void HandleRequest(T o);
            void SetSpecification(ISpecification<T> specification);
        }

        public abstract class Approver<T> : IHandler<T>
        {
            private IHandler<T> Successor;
            private string Name { set; get; }
            private ISpecification<T> Specification;
            public Approver(string name)
            {
                Name = name;
            }

            public bool CanHandle(T o)
            {
                return Specification.IsSatisfiedBy(o);
            }

            public void SetSuccessor(IHandler<T> handler)
            {
                Successor = handler;
            }

            public abstract void Handle(T o);

            public void HandleRequest(T o)
            {
                if (CanHandle(o))
                {
                    Console.WriteLine("{0}: Request handled by {1}.  ", o, Name);
                    Handle(o);
                }
                else
                {
                    Successor.HandleRequest(o);
                }
            }

            public void SetSpecification(ISpecification<T> specification)
            {
                Specification = specification;
            }
        }

        public abstract class DispatcherCenter : Approver<Reservation>
        {
            public DispatcherCenter(string name) : base(name)
            {
            }
        }

        public class BasicDispatcherCenter : DispatcherCenter
        {
            public BasicDispatcherCenter() : base("BasicDispatcherCenter")
            {
            }

            public override void Handle(Reservation o)
            {

                // validate credit card

                //search affiliates that have compact cars

                //send email to passenger
            }
        }

        public class MidDispatcherCenter : DispatcherCenter
        {
            public MidDispatcherCenter() : base("MidDispatcherCenter")
            {
            }

            public override void Handle(Reservation o)
            {
                // credit card must have 300$

                //search affiliates that have luxury cars

                //send email to passenger

                // schedule for christmas card
            }
        }

        public class PremiumDispatcherCenter : DispatcherCenter
        {
            public PremiumDispatcherCenter() : base("PremiumDispatcherCenter")
            {
            }

            public override void Handle(Reservation o)
            {
                // credit card must have 500$

                //search affiliates that have luxury cars

                //send email to passenger

                // schedule for christmas card

                // send gift for birthday

                //...
            }
        }

    }

    public static class SpecificationExtensions
    {
        public static Specification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new Specification<T>(o => left.IsSatisfiedBy(o) && right.IsSatisfiedBy(o));
        }

        public static Specification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new Specification<T>(o => left.IsSatisfiedBy(o) || right.IsSatisfiedBy(o));
        }

        public static Specification<T> Not<T>(this ISpecification<T> left)
        {
            return new Specification<T>(o => !left.IsSatisfiedBy(o));
        }
    }
}
