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
using NLP.NGrams;
using NLP.TextClassification;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace AutocompleteApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private Dictionary<string, int> unigramCounts = null;
        private Dictionary<string, int> bigramCounts = null;
        private Dictionary<string, int> trigramCounts = null;
        private List<NGram> allNGrams = null;
        private String scrapedText = "";

        int totalUnigrams = 0;
        int totalBigrams = 0;
        int totalTrigrams = 0;

        public MainForm()
        {
            InitializeComponent();

            // Attach event handler to the sentenceBox
            sentenceTextBox.TextChanged += SentenceTextBox_TextChanged;
            sentenceTextBox.KeyDown += SentenceTextBox_KeyDown;
        }

        // Write this method (and, of course, all other relevant methods, placed in
        // appropriately named classes, placed in a suitable folder.
        //
        // Note: To add a folder, right-click on the project in the Solution Explorer,
        // (e.g., NLP), then select Add - New Folder. 
        // Do NOT add folders externally (outside Visual Studio).
        //
        // Here, no class labels are needed. Instead you simply
        // need to read text and then tokenize it, after which you can generate
        // the n-grams (for n = 1,2, and 3).

        //private List<NGram> LoadDataSet()
        private void LoadDataSet()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    scrapedText = File.ReadAllText(openFileDialog.FileName).ToLower(); // Convert to lower case
                    string fileName = Path.GetFileName(openFileDialog.FileName); // File name without the file path.
                    nGramsListBox.Items.Add("Loaded data file \"" + fileName);
                }
            }
        }

        public List<NGram> GenerateNGrams(List<string> tokens, int n, Dictionary<string, int> frequencyCounts)
        {
            List<NGram> nGrams = new List<NGram>();
            if (tokens.Count < n)
            {
                return nGrams;
            }

            for (int i = 0; i < tokens.Count - n + 1; i++)
            {
                string nGramIdentifier = string.Join(" ", tokens.Skip(i).Take(n));

                // Create amd store the NGram object
                NGram nGram = new NGram(nGramIdentifier);
                nGrams.Add(nGram);

                // Update frequency counts
                if (frequencyCounts.ContainsKey(nGramIdentifier))
                {
                    frequencyCounts[nGramIdentifier]++;
                }
                else
                {
                    frequencyCounts[nGramIdentifier] = 1;
                }
            }
            return nGrams;
        }

        private void loadDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataSet();
            if (scrapedText != null) { tokenizeButton.Enabled = true; }
            loadDataSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            // Split scrapedText into sentences
            var sentences = Regex.Split(scrapedText, @"(?<!\w\.\w.)(?<![A-Z][a-z]\.)(?<=\.|\?)\s");

            // Initialize n-grams dictionaries and lists
            unigramCounts = new Dictionary<string, int>();
            bigramCounts = new Dictionary<string, int>();
            trigramCounts = new Dictionary<string, int>();
            allNGrams = new List<NGram>();

            // Process each sentence separately
            foreach (var sentence in sentences)
            {
                //Tokenize the sentence into words 
                List<string> sentenceTokens = Regex.Split(sentence, @"\W+").Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

                //Generate n-grams for the sentence (unigrams, bigrams, trigrams)
                if (sentenceTokens.Count > 0)
                {
                    totalUnigrams += sentenceTokens.Count;
                    allNGrams.AddRange(GenerateNGrams(sentenceTokens, 1, unigramCounts));

                    totalBigrams += sentenceTokens.Count - 1;
                    allNGrams.AddRange(GenerateNGrams(sentenceTokens, 2, bigramCounts));

                    totalTrigrams += sentenceTokens.Count - 2;
                    allNGrams.AddRange(GenerateNGrams(sentenceTokens, 3, trigramCounts));
                }
            }

            nGramsListBox.Items.Add("");
            nGramsListBox.Items.Add("Number of tokens: " + totalUnigrams);
            int totalNGrams = allNGrams.Count;
            int currentNGram = 0;
            //nGramsListBox.Items.Add("");

            // Compute frequency per million instances and store in NGram objects
            foreach (var nGram in allNGrams)
            {
                currentNGram++;
                double progress = 100.0 * currentNGram / (double)totalNGrams;
                string identifier = nGram.Identifier;
                //nGramsListBox.Items.Add("Processing n-gram " + currentNGram + " of " + totalNGrams + " (" + progress.ToString("F2") + "%)");

                if (identifier != null)
                {
                    if (unigramCounts.ContainsKey(identifier))
                    {
                        nGram.FrequencyPerMillionInstances = 1000000 * unigramCounts[identifier] / totalUnigrams;
                    }
                    else if (bigramCounts.ContainsKey(identifier))
                    {
                        nGram.FrequencyPerMillionInstances = 1000000 * bigramCounts[identifier] / totalBigrams;
                    }
                    else if (trigramCounts.ContainsKey(identifier))
                    {
                        nGram.FrequencyPerMillionInstances = 1000000 * trigramCounts[identifier] / totalTrigrams;
                    }
                }
            }

            nGramsListBox.Items.Add("");
            nGramsListBox.Items.Add("Generated " + totalUnigrams.ToString() +
                " Unigrams, " + totalBigrams.ToString() + " Bigrams, " + totalTrigrams.ToString() + " Trigrams.");

            tokenizeButton.Enabled = false;
            sentenceTextBox.Enabled = true;
        }

        private List<string> PredictNextWord(string text)
        {   
            var predictions = new List<string>();

            // Try to predict using trigrams
            var trigramPredictions = PredictWithTrigrams(text);
            if (trigramPredictions.Count > 0)
            {
                 predictions.AddRange(trigramPredictions);
            }

            // If no trigram prediction, try bigrams
            if (predictions.Count == 0)
            {
                var bigramPredictions = PredictWithBigrams(text);
                predictions.AddRange(bigramPredictions);
            }

            // If no bigram prediction, try unigrams (not for this assignment)
            /*if (predictions.Count == 0)
            {
                var unigramPredictions = PredictWithUnigrams(text);
                predictions.AddRange(unigramPredictions);
            }*/
            return predictions.Distinct().Take(3).ToList();
        }

        private List<string> PredictWithTrigrams(string text)
        {   
            var tokens = text.Split(' ');

            if (tokens.Length < 1)
            {
                return new List<string>();
            }
            if (tokens.Length == 1)
            {
                // Use full stop followed by the word
                return allNGrams
                    .Where(n => n.TokenList.Count > 2 && n.TokenList[0] == "." && n.TokenList[1] == tokens[0])
                    .OrderByDescending(n => n.FrequencyPerMillionInstances)
                    .Take(3)
                    .Select(n => n.TokenList.LastOrDefault())
                    .ToList();
            }
            else
            {
                var lastTwoWords = tokens.Skip(tokens.Length - 2).ToArray();

                return allNGrams
                    .Where(n => n.TokenList.Count > 2 && n.TokenList[0] == lastTwoWords[0] && n.TokenList[1] == lastTwoWords[1])
                    .OrderByDescending(n => n.FrequencyPerMillionInstances)
                    .Take(3)
                    .Select(n => n.TokenList.LastOrDefault())
                    .ToList();
            }
        }

        private List<string> PredictWithBigrams(string text)
        {
            var tokens = text.Split(' ');
            if (tokens.Length < 1)
            {
                return new List<string>();
            }
            var lastWord = tokens.Last();

            return allNGrams
                .Where(n => n.TokenList.Count > 1 && n.TokenList[0] == lastWord)
                .OrderByDescending(n => n.FrequencyPerMillionInstances)
                .Take(3)
                .Select(n => n.TokenList[1])
                .ToList();
        }

        /*
        private List<string> PredictWithUnigrams(string text)
        {
            return allNGrams
                .OrderByDescending(n => n.FrequencyPerMillionInstances)
                .Take(3)
                .Select(n => n.TokenList[0])
                .ToList();
        }*/

        private void SentenceTextBox_TextChanged(object sender, EventArgs e)
        {
            string inputText = sentenceTextBox.Text;

            //Only predict when the last character is a space
            if (!string.IsNullOrEmpty(inputText) && inputText.EndsWith(" "))
            {   
                string trimmedInputText = inputText.Trim();
                //Get the predicted word
                List<string> predictions = PredictNextWord(trimmedInputText);

                nGramsListBox.Items.Clear();
                foreach (var prediction in predictions)
                {
                    nGramsListBox.Items.Add(prediction);
                }
                //Ensures sentenceTextBox gets focus
                sentenceTextBox.Focus();
            }
        }

        private void SentenceTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            //nGramsListBox.Items.Add("Entered keydown funtion"); //Debugging

            //Ensures sentenceTextBox gets focus
            sentenceTextBox.Focus();

            if (e.KeyCode == Keys.Tab)
            {
                //nGramsListBox.Items.Add("Tab registered"); //Debugging

                e.SuppressKeyPress = true; //Prevent default tab behaviour,which is, being entered into text

                //Ensures sentenceTextBox gets focus
                sentenceTextBox.Focus();

                string inputText = sentenceTextBox.Text;

                if (nGramsListBox.Items.Count > 0)
                {
                    //nGramsListBox.Items.Add("n-grams list box items detected"); //Debugging

                    //Ensures sentenceTextBox gets focus
                    sentenceTextBox.Focus();

                    //Append the predicted word to the input text
                    string topPrediction = nGramsListBox.Items[0].ToString();
                    sentenceTextBox.Text = inputText.TrimEnd() + " " + topPrediction + " ";

                    //Move the cursor to the end of the text
                    sentenceTextBox.SelectionStart = sentenceTextBox.Text.Length;

                    //Ensures sentenceTextBox retains focus
                    sentenceTextBox.Focus();
                }
            }
        }

    }
}
