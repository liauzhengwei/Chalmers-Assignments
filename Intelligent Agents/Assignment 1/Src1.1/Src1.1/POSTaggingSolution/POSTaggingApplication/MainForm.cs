using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.POS;
using NLP.POS.Taggers;

namespace POSTaggingApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";
        private POSDataSet completeDataSet = null;
        private POSDataSet trainingDataSet = null;
        private POSDataSet testDataSet = null;
        private List<TokenData> vocabulary = null;
        private List<string[]> tagMappings = null;
        private UnigramTagger unigramTagger = null;

        //private Dictionary<string, string> mostFreqTags = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Note: 
        // The Brown corpus is available on the course Canvas page.
        // It can also be obtained at http://www.sls.hawaii.edu/bley-vroman/brown_corpus.html
        // in the file "browntag_nolines.txt: Corpus in one file, tagged, no line numbers, each sentence is one line"
        private void loadPOSCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                int tokenCount = 0;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);
                    completeDataSet = new POSDataSet();
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        if (line != "")//divide into sentences
                        {
                            List<string> lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();//get spelling and tag together
                            List<TokenData> tokenDataList = new List<TokenData>();
                            Sentence sentence = new Sentence();
                            foreach (string lineSplitItem in lineSplit)//for each spelling and tag combined term
                            {
                                List<string> spellingAndTag = lineSplitItem.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).ToList();//get spelling and tag separately
                                Token token = new Token();
                                if (spellingAndTag.Count == 2) // Needed in order to ignore the very last line that just contains "_.".
                                {
                                    token.Spelling = spellingAndTag[0].ToLower().Trim(); // Convert all words to lowercase.
                                    token.POSTag = spellingAndTag[1].Trim();
                                }
                                TokenData tokenData = new TokenData(token);
                                if (token.POSTag.Length == 1 || token.POSTag[1] != '|') // A somewhat ugly fix, needed to remove some junk from the data ...
                                {
                                    tokenDataList.Add(tokenData);//add token to tokenDataList and increment tokenCount
                                    tokenCount++;
                                }
                            }
                            sentence.TokenDataList = tokenDataList;
                            completeDataSet.SentenceList.Add(sentence);
                        }
                    }
                    fileReader.Close();
                    resultsListBox.Items.Add("Loaded the Brown corpus with " + completeDataSet.SentenceList.Count.ToString()
                        + " sentences and " + tokenCount.ToString() + " tokens.");
                }
            }
        }

        private List<TokenData> GenerateVocabulary(POSDataSet dataSet)
        {
            List<TokenData> tmpTokenDataList = new List<TokenData>();
            foreach (Sentence sentence in dataSet.SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    tmpTokenDataList.Add(tokenData);
                }
            }
            // Sort in alphabetical order, then by tag (also in alphabetical order)...
            // This takes a few seconds to run: It would have been more elegant (and easy) to put the
            // computation in a separate thread, but I didn't bother to do that here, as it would make
            // the code slightly more complex. Here, it is OK that the application freezes for a few
            // seconds while it is sorting the data.
            tmpTokenDataList = tmpTokenDataList.OrderBy(t => t.Token.Spelling).ThenBy(t => t.Token.POSTag).ToList();
            // ... then merge
            List<TokenData> tokenDataList = MergeTokens(tmpTokenDataList);
            return tokenDataList;
        }

        private List<TokenData> MergeTokens(List<TokenData> unmergedDataSet)
        {
            List<TokenData> mergedDataSet = new List<TokenData>();
            if (unmergedDataSet.Count > 0)
            {
                int index = 0;
                Token currentToken = new Token();
                currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                TokenData currentTokenData = new TokenData(currentToken);
                index++;
                while (index < unmergedDataSet.Count)
                {
                    Token nextToken = unmergedDataSet[index].Token;
                    if ((nextToken.Spelling == currentToken.Spelling) && (nextToken.POSTag == currentToken.POSTag))
                    {
                        currentTokenData.Count += 1;
                    }
                    else
                    {
                        mergedDataSet.Add(currentTokenData);
                        currentToken = new Token();
                        currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                        currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                        currentTokenData = new TokenData(currentToken);
                    }
                    index++;
                }
                mergedDataSet.Add(currentTokenData); // Add the final element as well ...
            }
            return mergedDataSet;
        }


        private void loadTagConversionDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            // Write code here: This method should load the tag conversion file. You must
            // represent the mappings somehow, so that the various tags in Brown set
            // can be mapped to the universal tag set. For example, one can maintain
            // a list with rows containing two elements (e.g. List<string[2]>, or List<List<string>>, where
            // the inner list should then contain two elements): The Brown tag and the corresponding
            // Universal tag, e.g.,
            //
            // VBZ -> VERB (verb in 3rd person present tense)
            // VB  -> VERB (verb in infinitive form)
            // NN  -> NOUN (noun in singular form)
            // NNS -> NOUN (noun in plural form) 
            // 
            // ...and so on. 
            // An even more elegant (and more re-usable) way is to define a class called, for example, POSTagConverter,
            // with a Convert method that takes a tag as input and outputs the converted tags.
            // Note that, since the data sets are not very large, you don't need to care much about the speed of the code.
            // Thus when searching for an input tag, you can use the Find() method instead of, say, a binary search 
            // or a Dictionary (but it's up to you).
            */
            tagMappings = new List<string[]>();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);    
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        // Process data here ...
                        if (line!="")//split into each line of text which is the mapping
                        {
                            string[] parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 2)
                            {
                                tagMappings.Add(new string[] { parts[0], parts[1] });//create tagMappings of [Brown mapping , Universal mapping]
                            }
                        }
                    }
                    fileReader.Close();
                    resultsListBox.Items.Add("Loaded the Tag Conversion Data");
                }
            }

            // Keep these lines: They will activate the conversion button, provided that the
            // Brown data set has been loaded first:
            if (completeDataSet != null)
            {
                if (completeDataSet.SentenceList.Count > 0)
                {
                    convertPOSTagsButton.Enabled = true;
                }
            }
        }
        
        private string GetMappedTag(string originalTag, List<string[]> tagMappings)
        {
            foreach (string[] mapping in tagMappings)
            {
                if (mapping[0] == originalTag)//if left element of tagMapping  same as original Brown POSTag, change to Universal POSTag
                    return mapping[1];
            }
            return "UNKNOWN";
        }

        private void convertPOSTagsButton_Click(object sender, EventArgs e)
        {
            // Write code here, such that the Brown tags are mapped to the 
            // Universal tags (for the complete data set), using the representation described above 
            // After running this method, all the tokens should be assigned
            // one of the 12 Universal tags.
            // 
            // Method call: 
            // completeDataSet.ConvertPOSTags(... <suitable input, namely the tag conversion data> ...); // this you have to write ...
            POSDataSet newDataSet = new POSDataSet();

            foreach (Sentence sentence in completeDataSet.SentenceList)
            {
                Sentence newSentence = new Sentence();

                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string originalTag = tokenData.Token.POSTag;
                    string newTag = GetMappedTag(originalTag, tagMappings);

                    Token newToken = new Token
                    {
                        Spelling = tokenData.Token.Spelling,
                        POSTag = newTag
                    };

                    newSentence.TokenDataList.Add(new TokenData(newToken));
                }

                newDataSet.SentenceList.Add(newSentence);

            }

            completeDataSet = newDataSet;

            resultsListBox.Items.Add("Converted the Brown Corpus to Universal");
            // Next, build the vocabulary, using the 12 universal tags (this method you get for free! :) )
            // NOTE: (Only) in this problem (for simplicity) the vocabulary is a simple List<TokenData> rather
            // than an instance of the Vocabulary class (which defines a Dictionary<string, Token>)
            vocabulary = GenerateVocabulary(completeDataSet);
            resultsListBox.Items.Add("Vocabulary is generated");

            // Keep this line: It will activate the split button.
            splitDataSetButton.Enabled = true;
        }

        private void splitDataSetButton_Click(object sender, EventArgs e)
        {
            // Split the data set into a training set and a test set (a validation
            // set is not needed here, since no optimization is carried out - the
            // unigram tagger is as it is - no optimization required or possible).
            // The result should be 
            //
            //  trainingDataSet (containing, by default, 80% of the sentences)
            //
            //  testDataSet (contaning the remaining 20% of the sentences)
            //
            double splitFraction;
            Boolean splitFractionOK = double.TryParse(splitFractionTextBox.Text, out splitFraction);

            if (splitFractionOK && splitFraction > 0 && splitFraction <= 1)
            {
                // NOTE: The most elegant way to do this is to write a static method in the POSDataSet class,
                // such as, POSDataSet.Split(POSDataSet completeDataSet, double splitFraction).
                // One should always strive to put methods *where they naturally belong*. In this case,
                // the split method belongs with the POSDataSet. One can also, of course,
                // just write the code here (in this method), instantiating the trainingDataSet and
                // the testSet, and then just adding sentences, but the most elegant way is
                // to define a method in the POSDataSet class. You can read about static
                // methods on MSDN or StackOverflow, for example
                int totalSentences = completeDataSet.SentenceList.Count;
                int trainSize = (int)(splitFraction * totalSentences);

                trainingDataSet = new POSDataSet();
                testDataSet = new POSDataSet();

                //Note: GetRange(starting index, number of elements to include in list)
                trainingDataSet.SentenceList = completeDataSet.SentenceList.GetRange(0, trainSize);
                testDataSet.SentenceList = completeDataSet.SentenceList.GetRange(trainSize, totalSentences - trainSize);
                

                resultsListBox.Items.Add("Training and Test sets are generated");

                // Keep these lines: It will activate the statistics generation button and the unigram tagger generation button,
                // once the data set has been split.
                generateStatisticsButton.Enabled = true;
                generateUnigramTaggerButton.Enabled = true;
            }
            else//when the splitFraction value cannot be parsed or not within [0,1]
            {
                MessageBox.Show("Incorrectly specified split fraction", "Error", MessageBoxButtons.OK);
            }
        }

        private void generateStatisticsButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Keep this line.
            /* Write code here for carrying out all the steps described in 
            // Assignment 1.1a, using the (note!) training set

            // Put different parts of the assignment (different subtasks) in
            // separate methods, in order to keep the code neat and tidy.

            // All the results should be printed *neatly* to the screen, in the
            // associated listBox (resultsListBox), after first clearing it.
            // To add things to the result listbox, one uses the command:
            // resultListBox.Items.Add(...);
            // where ... is a text string. Include empty lines with
            // resultListBox.Items.Add(" ");
            // where appropriate (e.g., between different
            // subtasks, to make the output more readable).
            */

            /* In training set:
             * 1) Count the instances of each POS tag
             * 2) Compute the fractions of each POS tag wto total
             * 3) Count fraction of words associated with 1,2,... different POS tags
             */

            Dictionary<string, int> posTagCounts = new Dictionary<string, int>();
            int totalTokenCount = 0;
            Dictionary<string, HashSet<string>> wordToPosTags = new Dictionary<string, HashSet<string>>();//Word to POS tags


            foreach (Sentence sentence in trainingDataSet.SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string trainToken = tokenData.Token.Spelling;
                    string trainTag = tokenData.Token.POSTag;

                    if (!posTagCounts.ContainsKey(trainTag))
                    {
                        posTagCounts[trainTag] = 0;//if POS tag not in dictionary yet
                    }
                    posTagCounts[trainTag]++;//else add to the POS tag's count

                    totalTokenCount++;//increase the total count

                    if (!wordToPosTags.ContainsKey(trainToken))
                    {
                        wordToPosTags[trainToken] = new HashSet<string>();//if a word does not have POS tag in dictionary yet
                    }
                    wordToPosTags[trainToken].Add(trainTag);//else add a POS tag to the word in the dictionary    
                }
            }

            Dictionary<string, double> posTagFractions = new Dictionary<string, double>();
            foreach(var posTagCount in posTagCounts)// for each POS tag
            {
                double fraction = (double)posTagCount.Value / totalTokenCount;
                posTagFractions[posTagCount.Key] = fraction;//assign fraction to the POS tag
            }

            Dictionary<int, int> wordTagCount = new Dictionary<int, int>();

            foreach(var wordEntry in wordToPosTags)
            {
                int numTags = wordEntry.Value.Count;//number of POS tags for this word

                if(!wordTagCount.ContainsKey(numTags))
                {
                    wordTagCount[numTags] = 0;
                }
                wordTagCount[numTags]++;
            }

            //Display results
            resultsListBox.Items.Add("POS Tag Counts:");
            foreach(var posTagCount in posTagCounts)
            {
                resultsListBox.Items.Add($"{posTagCount.Key}:{posTagCount.Value}");
            }

            resultsListBox.Items.Add("");
            resultsListBox.Items.Add("POS Tag Fractions:");
            foreach (var posTagFraction in posTagFractions)
            {
                resultsListBox.Items.Add($"{posTagFraction.Key}:{posTagFraction.Value}");
            }

            resultsListBox.Items.Add("");
            resultsListBox.Items.Add("Word-to-POS Tag Count:");
            foreach (var wordCount in wordTagCount)
            {
                resultsListBox.Items.Add($"Words with {wordCount.Key} different POS tags:{wordCount.Value}");
            }
        }

        private void generateUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            /*
            // Write code here for generating a unigram tagger, again using the *training* set;
            // Here, you *should* Define a class Unigram tagger derived from (inheriting) the base class
            // POSTagger in the NLP library included in this solution.

            // For the actual tagging (once the unigram tagger has been generated)
            // you must override the Tag() method in the base class (POSTagger)).

            // In the Unigram tagger, it might be a good idea to use a Dictionary<string, TokenData>
            // for quickly finding tokens (the TokenData) with a given spelling (the string).

            // Note that, for most POS taggers, it matters whether or not a word is
            // (say) the first word or the last word of a sentence, but not for the
            // unigram tagger, so it is easy to write the Tag() method - it need not
            // take into account the position of the word in the sentence.

            // Keep this line: It will activate the evaluation button for the unigram tagger
            */
            unigramTagger = new UnigramTagger(trainingDataSet.SentenceList);

            resultsListBox.Items.Add("");
            resultsListBox.Items.Add("Unigram tagger has been generated");

            runUnigramTaggerButton.Enabled = true;
        }

        private void runUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Keep this line.

            /* Write code here for running the unigram tagger over the test set.
            // All the results should be printed *neatly* to the screen, in the
            // associated listBox (resultsListBox), after first clearing it.

            // Note again that, for most POS taggers, it matters whether or not a word is
            // (say) the first word or the last word of a sentence, but not for the
            // unigram tagger. Thus, when you run the unigram tagger the "sentence"
            // that goes into the Tag() method can simply be the entire list of
            // tokens in the test set.

            //Compute the F-measure of the unigram tagger
            //Predicted value from unigram tagger
            //Ground truth value from completeDataSet
            */
            int truePositives = 0;
            int falsePositives = 0;
            int falseNegatives = 0;

            foreach (Sentence sentence in testDataSet.SentenceList)
            {
                List<string> predictedTags = unigramTagger.Tag(sentence);

                //Compare predicted tags with the ground truth (true tags)
                for (int i = 0; i < sentence.TokenDataList.Count; i++)
                {
                    string trueTag = sentence.TokenDataList[i].Token.POSTag;
                    string predictedTag = predictedTags[i];//Unigram tagger prediction

                    //Increment true posities, false positives, and false negatives
                    if (predictedTag == trueTag)
                    {
                        truePositives++;
                    }
                    else
                    {
                        //If word exists in the unigram model but was predicted incorrectly
                        if (unigramTagger.mostFreqTags.ContainsKey(sentence.TokenDataList[i].Token.Spelling.ToLower()))
                        {
                            falsePositives++;
                        }
                        else
                        {
                            //If word not in unigram model, missing tag
                            falseNegatives++;
                        }
                    }
                }
            }

            double precision = (truePositives + falsePositives > 0) ? (double)truePositives / (truePositives + falsePositives) : 0;
            double recall = (truePositives + falseNegatives > 0) ? (double)truePositives / (truePositives + falseNegatives) : 0;
            double fMeasure = (precision + recall > 0) ? 2 * (precision * recall) / (precision + recall) : 0;
            double accuracy = (truePositives + falsePositives + falseNegatives > 0) ? (double)truePositives / (truePositives + falsePositives + falseNegatives) : 0;

            resultsListBox.Items.Add($"True Positive: {truePositives}");
            resultsListBox.Items.Add($"False Positive: {falsePositives}");
            resultsListBox.Items.Add($"False Negative: {falseNegatives}");
            resultsListBox.Items.Add("");
            resultsListBox.Items.Add($"Precision: {precision}");
            resultsListBox.Items.Add($"Recall: {recall}");
            resultsListBox.Items.Add($"F-measure: {fMeasure}");
            resultsListBox.Items.Add($"Accuracy: {accuracy}");

        }
    }
}

/*public class UnigramTagger : POSTagger
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
}*/