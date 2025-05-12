using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP.TextClassification;

namespace NLP
{
    public class Vocabulary
    {   
        //Store the vocabulary dictionary
        private Dictionary<string, Token> vocab;

        //Constructor to initialize the dictionary
        public Vocabulary()
        {
            vocab = new Dictionary<string, Token>();
        }

        //Dictionary of <sentence, word>
        public void vocabularize(TextClassificationDataSet txds)
        {   //Pass in TextClassificationnDataSet as parameter
            //Return Dictionary<string, Token>

            foreach (TextClassificationDataItem txdi in txds.ItemList)
            {
                foreach (Token tk in txdi.TokenList)
                {
                    //Get the word/spelling 
                    string word = tk.Spelling;

                    //Assign word as key and Token as value
                    if (!vocab.ContainsKey(word))
                    {
                        vocab[word] = tk;
                    }
                }
            }
        }

        //Method to get the vocabulary dictionary
        public Dictionary<string, Token> getVocabulary()
        {
            return vocab;
        }

        // Write this class - it should contain a data structure
        // that holds all the words in the vocabulary
        // Use a Dictionary<string, Token> for this purpose.

        // NOTE: In Problem1.1, the Vocabulary class is not used, 
        // instead (only for that problem) the vocabulary is represented
        // as a simple List<Token>.
    }
}
