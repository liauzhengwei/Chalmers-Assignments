using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class PerceptronClassifier: TextClassifier
    {
        private double bias;
        private Dictionary<string, double> weightDictionary; // The Key (string) should identify (the spelling of) the token

        public override void Initialize(Vocabulary vocabulary)
        {
            // Write this method, setting up the weight dictionary
            // (one key-value pair (string-double) for each token in
            // the vocabulary, where the value represents the weight of
            // the corresponding token in the perceptron classifier.
            // Initially, assign random weights in the range [-1,1].
            // To obtain random numbers (in [0,1[) use the Random class,
            // with a suitable (integer) random number seed.
        }

        public override int Classify(List<Token> tokenList)
        {
            // ToDo: Write this method

            // The input should be the tokens of 
            // the text (i.e., a TextClassificationDataItem) that is being classified.


            // Remove the line below - needed for compilation of this skeleton code,
            // since the method must return an integer.
            // The returned integer should be the class ID (in this case, either 0 or 1).
            return 0; 
 
        }

        // This method generates a copied PerceptronClassifier, but the
        // return type (as defined in the base class) is TextClassifier.
        // Thus, when generating the copy, you must typecast it as 
        // PerceptronClassifier copiedClassifier = (PerceptronClassifier)classifier.Copy();
        public override TextClassifier Copy()
        {
            PerceptronClassifier copiedClassifier = new PerceptronClassifier();
            copiedClassifier.bias = bias;
            copiedClassifier.weightDictionary = new Dictionary<string, double>();
            foreach (string tokenSpelling in weightDictionary.Keys)
            {
                double weight = weightDictionary[tokenSpelling];
                copiedClassifier.weightDictionary.Add(tokenSpelling, weight);
            }
            return copiedClassifier;
        }

        public double Bias
        {
            get { return bias; }
            set { bias = value; }
        }

        public Dictionary<string, double> WeightDictionary
        {
            get { return weightDictionary; }
            set { weightDictionary = value; }
        }
    }
}
