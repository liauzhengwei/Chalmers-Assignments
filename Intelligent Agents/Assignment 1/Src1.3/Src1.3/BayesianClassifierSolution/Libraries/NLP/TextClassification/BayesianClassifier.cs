using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class BayesianClassifier: TextClassifier
    {   
        //Dictionary<classLabel, Dictionary<word, count>>
        public Dictionary<string, Dictionary<string, int>> classTokenCounts { get; private set; }

        //Dictionary<classLabel, count>
        public Dictionary<string, int> classCounts { get; private set; }

        //HashSet<word>
        public HashSet<string> vocabulary { get; private set; }
        public int totalItems { get; private set; }

        //public override void Initialize(Vocabulary vocabulary)
        public override void Initialize()
        {
            classTokenCounts = new Dictionary<string, Dictionary<string, int>>();
            classCounts = new Dictionary<string, int>();
            vocabulary = new HashSet<string>();
            totalItems = 0; 
        }

        public void Train(List<TextClassificationDataItem> dataSet)
        {
            foreach (var item in dataSet)
            {   
                //Count the class occurrences
                if (!classCounts.ContainsKey(item.ClassLabel.ToString()))
                {
                    classCounts[item.ClassLabel.ToString()] = 0;
                    classTokenCounts[item.ClassLabel.ToString()] = new Dictionary<string, int>();
                }
                classCounts[item.ClassLabel.ToString()]++;
                totalItems++;

                //Count the token occurrences for each class
                foreach (var token in item.TokenList)
                {   
                    string tokenSpelling = token.Spelling.ToLower();
                    vocabulary.Add(tokenSpelling);
                    if (!classTokenCounts[item.ClassLabel.ToString()].ContainsKey(tokenSpelling))
                    {
                        classTokenCounts[item.ClassLabel.ToString()][tokenSpelling] = 0;
                    }
                    classTokenCounts[item.ClassLabel.ToString()][tokenSpelling]++;
                }
                
            }
        }
        public override int Classify(List<Token> tokenList)
        {
            double maxProbability = double.MinValue;
            string bestClass = null;

            foreach (var classLabel in classCounts.Keys)
            {
                double classProbability = CalculateClassProbability(classLabel);

                double logProbability = Math.Log(classProbability);
                foreach (var token in tokenList)
                {
                    double tokenProbability = CalculateTokenProbability(token.Spelling, classLabel);
                    logProbability += Math.Log(tokenProbability);
                }
                if (logProbability > maxProbability)
                {
                    maxProbability = logProbability;
                    bestClass = classLabel;
                }
            }
            return int.Parse(bestClass);
        }

        // Calculate prior probability P(class)
        public double CalculateClassProbability(string classLabel)
        {
            return (double)classCounts[classLabel] / totalItems;
        }

        //Calculate conditional probability P(word|class)
        public double CalculateTokenProbability(string token, string classLabel)
        {
            int tokenCountInClass = 0;
            
            if (classTokenCounts[classLabel].ContainsKey(token.ToLower()))
            {
                tokenCountInClass = classTokenCounts[classLabel][token.ToLower()];
            }

            int totalTokensInClass = classTokenCounts[classLabel].Sum(kv => kv.Value);
            //Laplace Smoothing used below
            return (double)(tokenCountInClass +1) / (totalTokensInClass + vocabulary.Count);
        }

        // This method generates a copied BayesianClassifier, but the
        // return type (as defined in the base class) is TextClassifier.
        // Thus, when generating the copy, you must typecast it as 
        // BayesianClassifier copiedClassifier = (BayesianClassifier)classifier.Copy();
        /*public override TextClassifier Copy()
        {
            BayesianClassifier copiedClassifier = new BayesianClassifier();
            copiedClassifier.wordProbabilities = new Dictionary<string, Dictionary<string, double>>();
            copiedClassifier.classProbabilities = new Dictionary<string, double>();
            copiedClassifier.vocabulary = new HashSet<string>();

            // Add the foreach part when copying dictionaries and hashset


            return copiedClassifier;
        }*/
    }
}
