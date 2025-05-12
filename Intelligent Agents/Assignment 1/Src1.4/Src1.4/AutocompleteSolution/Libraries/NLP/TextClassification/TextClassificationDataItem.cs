using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class TextClassificationDataItem
    { 
        public string Text { get; set; }    
        public int ClassLabel { get; set; }

        public List<Token> TokenList { get; set; } // Should be generated in the Tokenization step.
     }
}
