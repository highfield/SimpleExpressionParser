using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    internal class Reader
    {
        public Reader(string text)
        {
            this.Source = text.ToCharArray();
            this._length = this.Source.Length;
        }

        private int _length;

        public readonly char[] Source;
        public int Index { get; private set; }

        public readonly List<XToken> Tokens = new List<XToken>();


        public bool TryPeek(
            out char value
            )
        {
            if (this.Index < this._length)
            {
                value = this.Source[this.Index];
                return true;
            }
            else
            {
                value = default(char);
                return false;
            }
        }

        public void MoveNext()
        {
            this.Index++;
        }
    }
}
