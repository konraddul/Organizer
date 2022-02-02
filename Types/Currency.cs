using System;
using System.Collections.Generic;
using System.Text;

namespace Organizer.Types
{
    public class Currency : IComparable
    {
        public decimal Value { get; set; }
        public string Symbol { get; set; }
        public Currency(decimal value, string symbol)
        {
            this.Value = value;
            this.Symbol = symbol;
        }
        public override string ToString()
        {
            return Value.ToString() + " " + Symbol;
        }

        public int CompareTo(object obj)
        {
            Currency other = obj as Currency;
            if (this.Value < other.Value) return -1;
            else if (this.Value > other.Value) return 1;
            else return 0;
        }
    }
}
