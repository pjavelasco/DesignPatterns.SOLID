using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace SOLID.SingleResponsibilityPrinciple
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I got up early.");
            j.AddEntry("I ate a sandwich.");
            WriteLine(j);

            var p = new Persistence();
            var filename = @"c:\windows\temp\journal.txt";
            p.SaveToFile(j, filename);
        }
    }

    public class Journal
    {
        private readonly List<string> _entries = new();

        private static int _count = 0;

        private static void IncreaseCount() => _count++;

        public int AddEntry(string text)
        {
            IncreaseCount();
            _entries.Add($"{_count}: {text}");
            return _count; // memento pattern!
        }

        public void RemoveEntry(int index) => _entries.RemoveAt(index);

        public override string ToString() => string.Join(Environment.NewLine, _entries);

        // Next methods would break Single Responsibility Principle
        public void Save(string filename, bool overwrite = false)
        {
        }

        public void Load(string filename)
        {
        }

        public void Load(Uri uri)
        {
        }
    }

    /// <summary>
    /// Class that handles the responsibility of persisting objects
    /// </summary>
    public class Persistence
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, journal.ToString());
            }
        }
        public void Load(string filename)
        {
        }

        public void Load(Uri uri)
        {
        }
    }
}