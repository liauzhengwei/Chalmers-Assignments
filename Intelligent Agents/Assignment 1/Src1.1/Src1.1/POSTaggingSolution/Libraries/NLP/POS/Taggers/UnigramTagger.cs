using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.POS.Taggers
{
    public class UnigramTagger : POSTagger
    {
        //Dictionary of <word, mostFreqTag>
        public Dictionary<string, string> mostFreqTags;

        //Constructor for the UnigramTagger
        public UnigramTagger(List<Sentence> trainingSentences)
        {
            mostFreqTags = new Dictionary<string, string>();
            Train(trainingSentences);
        }

        //Train the unigram tagger by building the unigram model
        private void Train(List<Sentence> trainingSentences)
        {
            //Dictionary<word, Dictionary<POSTag, instances>>
            Dictionary<string, Dictionary<string, int>> unigramModel = new Dictionary<string, Dictionary<string, int>>();

            foreach (Sentence sentence in trainingSentences)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling.ToLower();//Convert to lowercase
                    string posTag = tokenData.Token.POSTag;

                    //Add word into dictionary if not already in it
                    if (!unigramModel.ContainsKey(word))
                    {
                        unigramModel[word] = new Dictionary<string, int>();
                    }

                    //Add POSTag to word if not already in it
                    if (!unigramModel[word].ContainsKey(posTag))
                    {
                        unigramModel[word][posTag] = 0;
                    }

                    unigramModel[word][posTag]++;//Increment count of a posTag for a word

                }
            }

            foreach (var wordEntry in unigramModel)
            {
                string word = wordEntry.Key;
                var posTagCounts = wordEntry.Value;//Dict<string, int>

                //Get mostFreqTag by choosing the highest count 
                var mostFreqTag = posTagCounts.OrderByDescending(kvp => kvp.Value).First().Key;

                //Store the result in the Dictonary for this current word
                mostFreqTags[word] = mostFreqTag;
            }

        }

        //Override the abstract Tag() method from POSTagger
        public override List<string> Tag(Sentence sentence)
        {
            List<string> predictedTags = new List<string>();

            foreach (TokenData tokenData in sentence.TokenDataList)
            {
                string word = tokenData.Token.Spelling.ToLower();//Convert word to lowercase

                //Get the predicted POS tag from the unigram model
                string predictedTag = mostFreqTags.ContainsKey(word) ? mostFreqTags[word] : "NN";

                predictedTags.Add(predictedTag);

            }
            return predictedTags; //Return list of predicted POS tags for the sentence
        }
    }
}