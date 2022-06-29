using System;

namespace SOLID.InterfaceSegregationPrinciple
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Document
    {
    }

    public interface IMachine
    {
        void Print(Document d);
        void Fax(Document d);
        void Scan(Document d);
    }

    // If you need a multifunction machine
    public class MultiFunctionPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        public void Fax(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    // If you need a old printer that only prints documents, you need to implement of interface methods
    public class OldPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        public void Fax(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new System.NotImplementedException();
        }
    }

    // Segregation
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Printer : IPrinter
    {
        public void Print(Document d)
        {
            //
        }
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    // Interface combination
    public interface IMultiFunctionDevice : IPrinter, IScanner 
    {
    }

    public struct MultiFunctionMachine : IMultiFunctionDevice
    {        
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner)); 
        }

        public void Print(Document d)
        {
            _printer.Print(d);
        }

        public void Scan(Document d)
        {
            _scanner.Scan(d);
        }
    }
}