using System;
using System.Collections.Generic;
using static System.Console;

namespace SOLID.OpenClosedPrinciple
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var products = new Product[]
            {
                new Product("Apple", Color.Green, Size.Small),
                new Product("Tree", Color.Green, Size.Large),
                new Product("House", Color.Blue, Size.Large)
            };

            // Bad
            var pf = new ProductFilter();
            WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                WriteLine($" - {p.Name} is green");
            }

            // Good
            var bf = new BetterFilter();
            WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                WriteLine($" - {p.Name} is green");
            }

            WriteLine("Large products");
            foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
            {
                WriteLine($" - {p.Name} is large");
            }

            WriteLine("Large blue items");
            foreach (var p in bf.Filter(products,
              new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)))
            )
            {
                WriteLine($" - {p.Name} is big and blue");
            }
        }
    }

    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge
    }

    public class Product
    {
        public Product(string name, Color color, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
        }

        public Size Size { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
    }

    // Bad class
    public class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }

        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                {
                    yield return p;
                }
            }
        }

        public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
            {
                if (p.Size == size && p.Color == color)
                {
                    yield return p;
                }
            }
        }

        /*
            Combinatorial explosión, if we want to add a new filter criteria, we would need to have 7 methods

            So, this class is not OCP: open for extension and closed for modification
         */
    }

    // The following two interfaces will be open for extension

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // Color specification
    public class ColorSpecification : ISpecification<Product>
    {
        private readonly Color _color;

        public ColorSpecification(Color color) => _color = color;

        public bool IsSatisfied(Product p) => p.Color == _color;
    }

    // Size specification
    public class SizeSpecification : ISpecification<Product>
    {
        private readonly Size _size;

        public SizeSpecification(Size size) => _size = size;

        public bool IsSatisfied(Product p) => p.Size == _size;
    }

    // Combination of specifications
    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _first;
        private readonly ISpecification<T> _second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            _first = first ?? throw new ArgumentNullException(nameof(first));
            _second = second ?? throw new ArgumentNullException(nameof(second));
        }

        public bool IsSatisfied(T t) => _first.IsSatisfied(t) && _second.IsSatisfied(t);
    }

    // Good class
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
            {
                if (spec.IsSatisfied(i))
                {
                    yield return i;
                }
            }
        }
    }
}