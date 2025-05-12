using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;

namespace PerceptronClassifierApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private PerceptronClassifier classifier = null;
        private Vocabulary vocabulary = null;
        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet validationSet = null;
        private TextClassificationDataSet testSet = null;

        private Task optimizerTask; //Task for running optimizer    
        private CancellationTokenSource cancellationTokenSource;
        private PerceptronClassifier bestClassifier;

        private int epochs = 200;
        private double learningRate = 0.1;
        private double bestvalidationAccuracy = 0.0;

        private StreamWriter writer;

        TextClassificationDataItem correctExample;
        TextClassificationDataItem incorrectExample;

        public MainForm()
        {
            InitializeComponent();
        }

        private TextClassificationDataSet LoadDataSet()//Function to load different datasets
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
        {//triggers LoadDataSet() which loads the dataset(training set) 
            trainingSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
        }

        private void loadValidationSetToolStripMenuItem_Click(object sender, EventArgs e)
        {//triggers LoadDataSet() which loads the dataset(validation set) 
            validationSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadValidationSetToolStripMenuItem.Enabled = false;
        }

        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {//triggers LoadDataSet() which laods the dataset(test set)
            testSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GenerateVocabulary(TextClassificationDataSet dataSet)
        {
            // Write a method that generates the vocabulary. Note that this
            // should ONLY be done for the training set!

            //Pass in TextClassificationnDataSet as parameter
            //Return Dictionary<string, Token>

            vocabulary = new Vocabulary();
            vocabulary.vocabularize(dataSet);

            // You must generate an instance of the Vocabulary class,
            // which you must also implement (a skeleton is available
            // in the NLP library)
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {//At this point, training, test and validation sets assigned,
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

            GenerateVocabulary(trainingSet);
            progressListBox.Items.Add("Vocabulary generated for training set");

            // Next, tokenize the validation set:
            foreach (var item in validationSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            progressListBox.Items.Add("Validation set is tokenized");

            // Finally, tokenize the test set:
            foreach (var item in testSet.ItemList)
            {
                item.TokenList = tokenizer.Tokenize(item.Text);
            }
            progressListBox.Items.Add("Test set is tokenized");

            //
            initializeOptimizerButton.Enabled = true;
        }


        private void initializeOptimizerButton_Click(object sender, EventArgs e)
        {
            // Write code here for initializing a perceptron optimizer, which
            // you must also write (i.e. a class called PerceptronOptimizer).
            // Moreover, as mentioned in the assignment text,
            // it might be a good idea to define an evaluator class (e.g. PerceptronEvaluator)
            // You should place both classes in the TextClassification folder in the NLP library.

            classifier = new PerceptronClassifier();
            classifier.Initialize(vocabulary);

            progressListBox.Items.Add("");
            progressListBox.Items.Add("Perceptron Optimizer Initialized.");

            startOptimizerButton.Enabled = true;
        }

        private void startOptimizerButton_Click(object sender, EventArgs e)
        {
            startOptimizerButton.Enabled = false;
            stopOptimizerButton.Enabled = true;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken canceltoken = cancellationTokenSource.Token;

            // Start the optimizer here.

            // For every epoch, the optimizer should (after the epoch has been completed)
            // trigger an event that prints the current accuracy (over the training set
            // and the validation set) of the perceptron classifier (in a thread-safe
            // manner, and with proper (clear) formatting). Do *not* involve
            // the test set here.

            //Specify file path to save the validation and training accuracy of every epoch
            //string filePath = "C:\\Users\\liauz\\OneDrive\\Desktop\\Intelligent Agents\\Assignment 1\\Training and Validation Accuracy for Graph.txt";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Training and Validation Accuracy for Graph.txt"); 

            writer = new StreamWriter(filePath, false); //false overwrites the file if it already exists
            writer.WriteLine("Epoch\tTraining Accuracy\tValidation Accuracy");

            optimizerTask = Task.Run(() =>
            {
                if (!progressListBox.IsDisposed)
                {
                    progressListBox.Invoke((MethodInvoker)(() => progressListBox.Items.Add("")));
                }
                //Iterate through each epoch
                for (int epoch = 0; epoch < epochs; epoch++)
                {

                    //Iterate through each item in the training set
                    foreach (var item in trainingSet.ItemList)
                    {
                        //Classify the item
                        int predicted = classifier.Classify(item.TokenList);
                        int actual = item.ClassLabel;
                        //If the classification is incorrect
                        if (predicted != actual)
                        {
                            double error = actual - predicted;
                            //Update the weights
                            foreach (Token token in item.TokenList)
                            {
                                if (classifier.WeightDictionary.ContainsKey(token.Spelling))
                                {
                                    classifier.WeightDictionary[token.Spelling] += learningRate * error;
                                }

                            }

                            //Update bias
                            classifier.Bias += learningRate * error;
                        }
                    }

                    //Log accuracy in a thread safe way
                    double trainingAccuracy = Evaluate(trainingSet.ItemList, classifier);
                    double validationAccuracy = Evaluate(validationSet.ItemList, classifier);

                    writer.WriteLine((epoch+1) + "\t" + trainingAccuracy + "\t" + validationAccuracy);

                    // Check if the validation accuracy is the best so far
                    if (validationAccuracy > bestvalidationAccuracy)
                    {
                        bestvalidationAccuracy = validationAccuracy;
                        // Save the classifier to a file

                        bestClassifier = (PerceptronClassifier)classifier.Copy();
                    }

                    this.Invoke((MethodInvoker)(() =>
                    {
                        progressListBox.Items.Add("Epoch: " + (epoch + 1) + " Training accuracy: " + trainingAccuracy.ToString("P") + " Validation accuracy: " + validationAccuracy.ToString("P"));

                    }));

                    if (canceltoken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                //Flush and Close the writer
                writer.Flush();
                writer.Close();
            }, canceltoken);
            
        }

        private async void stopOptimizerButton_Click(object sender, EventArgs e)
        {
            stopOptimizerButton.Enabled = false;

            // Stop the optimizer here.
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            await Task.Delay(1000); //Wait for a second to ensure the optimizer has stopped

            //Evaluate the best classifier using the test set
            if (bestClassifier != null)
            {
                double bestClassifierTestAccuracy = EvaluateBest(testSet.ItemList, bestClassifier);
                double bestClassifierValidationAccuracy = Evaluate(validationSet.ItemList, bestClassifier);
                double bestClassifierTrainingAccuracy = Evaluate(trainingSet.ItemList, bestClassifier);

                ///Display the result in a thread safe way
                if (!progressListBox.IsDisposed)
                {
                    progressListBox.Invoke((MethodInvoker)(() =>
                    {
                        progressListBox.Items.Clear();
                        progressListBox.Items.Add("Best Classifier:");
                        progressListBox.Items.Add("Training accuracy: " + bestClassifierTrainingAccuracy.ToString("P"));
                        progressListBox.Items.Add("Validation accuracy: " + bestClassifierValidationAccuracy.ToString("P"));
                        progressListBox.Items.Add("Test accuracy: " + bestClassifierTestAccuracy.ToString("P"));
                        progressListBox.Items.Add("");


                        if (correctExample != null && incorrectExample != null)
                        {
                            progressListBox.Items.Add("Correctly Classified Example:");
                            progressListBox.Items.Add($"Sentence:");
                            PrintSentenceWithLineBreaks(string.Join(" ", correctExample.TokenList.Select(t => t.Spelling)));
                            progressListBox.Items.Add($"Actual Class: {correctExample.ClassLabel}, Predicted: {bestClassifier.Classify(correctExample.TokenList)}");
                            progressListBox.Items.Add("");

                            progressListBox.Items.Add("Incorrectly Classified Example:");
                            progressListBox.Items.Add($"Sentence:");
                            PrintSentenceWithLineBreaks(string.Join(" ", incorrectExample.TokenList.Select(t => t.Spelling)));
                            progressListBox.Items.Add($"Actual Class: {incorrectExample.ClassLabel}, Predicted: {bestClassifier.Classify(incorrectExample.TokenList)}");

                        }
                        PrintTopBottomWords();
                    }));
                }
            }
        }

        private void PrintSentenceWithLineBreaks(string sentence)
        {
            //Split the sentence into individual words
            string[] words = System.Text.RegularExpressions.Regex.Split(sentence, @"[\s\t]+");

            //Create a new StringBuilder
            StringBuilder sb = new StringBuilder();

            //Iterate through words and print in groups of 15
            for (int i = 0; i < words.Length; i++)
            {
                sb.Append(words[i] + " ");
                if ((i % 15 == 0 && i!=0)|| i==words.Length - 1)
                {
                    progressListBox.Items.Add(sb.ToString());
                    //Reset the StringBuilder for the next group of words
                    sb.Clear();
                }
            }


            // For simplicity (even though one may perhaps resume the optimizer), at this
            // point, evaluate the best classifier (= best validation performance) over
            // the *test* set, and print the accuracy to the screen (in a thread-safe
            // manner, and with proper (clear) formatting).

            stopOptimizerButton.Enabled = true; // A bit ugly, should wait for the
            // optimizer to actually stop, but that's OK, it will stop quickly.
        }

        private double EvaluateBest(List<TextClassificationDataItem> items, PerceptronClassifier model)
        {
            if (items == null || model == null || items.Count == 0)
            {
                correctExample = null;
                incorrectExample = null;
                return 0.0;
            }

            int correct = 0;
            correctExample = null;
            incorrectExample = null;


            foreach (var item in items)
            {
                int predicted = classifier.Classify(item.TokenList);
                int actual = item.ClassLabel;
                if (predicted == actual)
                {
                    correct++;
                    if (correctExample == null)
                    {
                        correctExample = item;
                    }
                }
                else
                {
                    if (incorrectExample == null)
                    {
                        incorrectExample = item;
                    }
                }
            }
            return (double)correct / items.Count;
        }

        private double Evaluate(List<TextClassificationDataItem> items, PerceptronClassifier model)
        {
            if (items == null || model == null || items.Count == 0)
            {
                return 0.0;
            }

            int correct = 0;

            foreach (var item in items)
            {
                int predicted = classifier.Classify(item.TokenList);
                int actual = item.ClassLabel;
                if (predicted == actual)
                {
                    correct++;
                }
            }
            return (double)correct / items.Count;
        }

        private void PrintTopBottomWords()
        {
            if (bestClassifier == null || bestClassifier.WeightDictionary.Count == 0 || bestClassifier.WeightDictionary == null)
            {
                return;
            }

            //Sort the dictionary by weight value, descending order
            var sortedWeights = bestClassifier.WeightDictionary.OrderByDescending(x => x.Value).ToList();

            // Top 10 positive words
            var topPositiveWords = sortedWeights.Take(10).ToList();

            // Top 10 negative words
            var topNegativeWords = sortedWeights.Reverse<KeyValuePair<string,double>>().Take(10).ToList();

            // Display results
            progressListBox.Items.Add("");
            progressListBox.Items.Add("Top 10 Positive words:");
            foreach (var word in topPositiveWords)
            {
                progressListBox.Items.Add(word.Key + " : " + word.Value);
            }

            progressListBox.Items.Add("");
            progressListBox.Items.Add("Top 10 Negative words:");
            foreach (var word in topNegativeWords)
            {
                progressListBox.Items.Add(word.Key + " : " + word.Value);
            }

        }
    }
}
