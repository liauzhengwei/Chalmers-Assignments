using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLP.Tokenization
{
    public class Tokenizer
    {
        public List<Token> Tokenize(string text)
        {
            // Implement your tokenizer here (to handle abbreviations, numbers, special characters, and so on). 
            // You may wish to add more methods to keep the code well-structured

            //List to hold all the Tokens
            List<Token> tokens = new List<Token>();

            //Regular expression pattern
            //Match words(including words with abbreviations), numbers (including decimal numbers), and punctuation
            string tokenPattern = @"([A-Za-z0-9]+(?:\.[A-Za-z0-9]+)* | [.,!?;()\""-]+|\.\.\.+ | !+ | \d+\.\d+|\d+)";

            //Use Regex to find all matches in the input text
            Regex regex = new Regex(tokenPattern);
            MatchCollection matches = regex.Matches(text);

            //Collect all tokens into the result list
            foreach(Match match in matches)
            {
                Token token = new Token();
                token.Spelling = match.Value;
                
                tokens.Add(token);
            }

            //Return the List of Tokens
            return tokens;
        }
    }
}
