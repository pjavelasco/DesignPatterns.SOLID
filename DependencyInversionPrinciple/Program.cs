using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace SOLID.DependencyInversionPrinciple
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            // low-level module
            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }

    // High-level modules should not depend on low-level; both should depend on abstractions
    // abstractions should not depend on details; details should depend on abstractions
    public enum Relationship
    {
        Parent, Child, Sibling
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // low-level
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> _relations = new();

        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child));
            _relations.Add((child, Relationship.Child, parent));
        }

        public List<(Person, Relationship, Person)> Relations => _relations;

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return _relations.Where(x => x.Item1.Name == name && x.Item2 == Relationship.Parent).Select(r => r.Item3);
        }
    }

    public class Research
    {
        //public Research(Relationships relationships)
        //{
            // high-level: find all of john's children
            //var relations = relationships.Relations;
            //foreach (var r in relations
            //  .Where(x => x.Item1.Name == "John"
            //              && x.Item2 == Relationship.Parent))
            //{
            //  WriteLine($"John has a child called {r.Item3.Name}");
            //}
        //}

        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                WriteLine($"John has a child called {p.Name}");
            }
        }
    }
}
