using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // For the Path class; see LoadDataSet()
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;

namespace BayesianClassifierApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet testSet = null;
        private BayesianClassifier classifier = null;


        public MainForm()
        {
            InitializeComponent();
        }

        private TextClassificationDataSet LoadDataSet()
        {
            TextClassificationDataSet dataSet = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dataSet = new TextClassificationDataSet();
                    StreamReader dataReader = new StreamReader(openFileDialog.FileName);
                    while (!dataReader.EndOfStream)
                    {
                        string line = dataReader.ReadLine();
                        List<string> lineSplit = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        TextClassificationDataItem item = new TextClassificationDataItem();
                        item.Text = lineSplit[0].ToLower();
                        item.ClassLabel = int.Parse(lineSplit[1]);
                        dataSet.ItemList.Add(item);
                    }
                    dataReader.Close();
                    int count0 = dataSet.ItemList.Count(i => i.ClassLabel == 0);
                    int count1 = dataSet.ItemList.Count(i => i.ClassLabel == 1);
                    string fileName = Path.GetFileName(openFileDialog.FileName); // File name without the file path.
                    progressListBox.Items.Add("Loaded data file \"" + fileName + "\" with " + count0.ToString() +
                        " negative reviews and " + count1.ToString() + " positive reviews.");
                }
            }
            return dataSet;
        }

        private void loadTrainingSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainingSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
        }

        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            //At this point, training and test sets assigned,
            //     and are of type TextClassificationDataSet class

            // Write code here for tokenizing the text. That is,
            // implement the Tokenize() method in the Tokenizer class.
            Tokenizer tokenizer = new Tokenizer();

            foreach (var item in trainingSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            progressListBox.Items.Add("");
            progressListBox.Items.Add("Training set is tokenized");

            // First tokenize the training set:

            // Add code here... - should take the raw Text for each
            // TextClassificationDataItem and generate the TokenList
            // (also placed in the TextClassificationDataItem).

            // Then build the vocabulary from the training set:

            ////GenerateVocabulary(trainingSet);
            ////progressListBox.Items.Add("Vocabulary generated for training set");

            // Finally, tokenize the test set:
            foreach (var item in testSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            progressListBox.Items.Add("Test set is tokenized");

            //

            toolStripButton1.Enabled = true;
        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;

            classifier = new BayesianClassifier();

            classifier.Initialize();
            progressListBox.Items.Add("Naive Bayes Classifier initialized");
            progressListBox.Items.Clear();

            // Train the classifier using the training set:
            classifier.Train(trainingSet.ItemList);
            var classCounts = classifier.classCounts;
            var classTokenCounts = classifier.classTokenCounts;
            var vocabulary = classifier.vocabulary;
            var totalItems = classifier.totalItems;

            //Prints prior probabilities of each class in training set
            progressListBox.Items.Add("Prior Probabilities of each class: ");
            foreach (var classLabel in classCounts.Keys)
            {
                double classProbability = classifier.CalculateClassProbability(classLabel);
                progressListBox.Items.Add("Class: " + classLabel + ", Probability: " + classProbability);
                //double logProbability = Math.Log(classProbability);
            }
            progressListBox.Items.Add("");

            progressListBox.Items.Add("Probabilities of each class given a term: ");
            foreach (var classLabel in classTokenCounts.Keys)
            {
                progressListBox.Items.Add("Class: " + classLabel);
                foreach (var token in new string[] { "friendly" , "perfectly", "horrible", "poor" })
                {
                    double tokenProbability = 0;

                    if (classTokenCounts[classLabel].ContainsKey(token)) {
                        tokenProbability = classifier.CalculateTokenProbability(token, classLabel);
                    }
                    progressListBox.Items.Add("Token: " + token + ", Probability: " + tokenProbability);
                }
                progressListBox.Items.Add("");

            }
            progressListBox.Items.Add("Training Set: ");
            Evaluate(trainingSet.ItemList);
        }
            

        public void Evaluate(List<TextClassificationDataItem> dataSet)
        {
            
            Dictionary<string, int> truePositives = new Dictionary<string, int>();
            Dictionary<string, int> falsePositives = new Dictionary<string, int>();
            Dictionary<string, int> falseNegatives = new Dictionary<string, int>();
            int correctPredictions = 0;

            //Initialise counts for each class
            foreach (var classLabel in classifier.classCounts.Keys)
            {
                truePositives[classLabel] = 0;
                falsePositives[classLabel] = 0;
                falseNegatives[classLabel] = 0;
            }

            //Iterate through the data set
            foreach (var item in dataSet)
            {
                string predictedClass = classifier.Classify(item.TokenList).ToString();
                string actualClass = item.ClassLabel.ToString();
                if (predictedClass == actualClass)
                {
                    correctPredictions++;
                    truePositives[predictedClass]++;
                }
                else
                {
                    falsePositives[predictedClass]++;
                    falseNegatives[actualClass]++;
                }
            }

            //Calculate precision, recall, accuracy and F1 for each class
            int totalFP = truePositives.Values.Sum();
            int totalFN = falseNegatives.Values.Sum();
            int totalTP = truePositives.Values.Sum();
            int totalPredictions = 0;
            foreach (var item in dataSet) 
            {
                totalPredictions++;
            }

            double accuracy = (double)totalTP / totalPredictions;
            double precision = (double)totalTP / (totalTP + totalFP);
            double recall = (double)totalTP / (totalTP + totalFN);
            double f1 = 2 * ((precision * recall) / (precision + recall));

            progressListBox.Items.Add("Accuracy: " + accuracy);
            progressListBox.Items.Add("Precision: " + precision);
            progressListBox.Items.Add("Recall: " + recall);
            progressListBox.Items.Add("F1: " + f1);
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = false;
            progressListBox.Items.Add("");
            progressListBox.Items.Add("Test Set: ");
            Evaluate(testSet.ItemList);
        }
    }
}
