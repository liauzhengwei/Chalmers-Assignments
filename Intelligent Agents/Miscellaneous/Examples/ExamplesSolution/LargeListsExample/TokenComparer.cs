using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeListsExample
{
    public class TokenComparer : IComparer<Token>
    {
        public int Compare(Token token1, Token token2)
        {
            return token1.Spelling.CompareTo(token2.Spelling);
        }
    }
}
