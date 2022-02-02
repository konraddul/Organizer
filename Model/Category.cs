using System;
using System.Collections.Generic;
using System.Text;
using Organizer.Types;

namespace Organizer.Model
{
    public class Category : IComparable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Currency Limit { get; set; }
        public Category(int id, string name, decimal limit, string currency)
        {
            this.ID = id;
            this.Name = name;
            this.Limit = new Currency(limit, currency);
        }
        public override string ToString()
        {
            return this.Name;
        }

        public int CompareTo(object obj)
        {
            Category cat = obj as Category;
            return Name.CompareTo(cat.Name);
        }
    }
}
