using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LargeListsExample
{
    public partial class MainForm : Form
    {
        private List<Token> dataList;
        private List<Token> sortedDataList;
        private Dictionary<string, Token> dataDictionary;
        private Random randomNumberGenerator;

        public MainForm()
        {
            InitializeComponent();
        }

        // This method generates a list of tokens (class: Token), each with a spelling
        // and a (dummy) property of type double (which is not used in the example).
        private void generateDataButton_Click(object sender, EventArgs e)
        {  
            randomNumberGenerator = new Random();
            int dataSetSize = int.Parse(dataSetSizeTextBox.Text);   
            dataList = new List<Token>();
            for (int ii = 0; ii < dataSetSize; ii++)
            {
                Token token = new Token();
                token.Spelling = ii.ToString();
                token.NumericProperty = randomNumberGenerator.NextDouble(); // Just to assign some value ..
                dataList.Add(token);
            }
            dataList = dataList.OrderBy(x => randomNumberGenerator.Next()).ToList();
            sortedDataList = dataList.OrderBy(t => t.Spelling).ToList();
            dataDictionary = new Dictionary<string, Token>();
            foreach (Token token in dataList)
            {
                dataDictionary.Add(token.Spelling, token);
            }
            generateStatisticsButton.Enabled = true;
        }

        // NOTE: For simplicity and clarity, I did not use threading in this
        // example. Thus, when the program runs, the GUI freezes up. If you
        // wish, you can use the method in the ThreadingExample to apply
        // threading here as well (needs a few extra lines of code, plus
        // a thread-safe method for writing the data to the screen).
        private void generateStatisticsButton_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            // First method: Find the elements in the unsorted list
            int numberOfCalls = int.Parse(numberOfCallsTextBox.Text);

            stopwatch.Start();
            for (int jj = 0; jj < numberOfCalls; jj++)
            {
                string stringToFind = randomNumberGenerator.Next(0, dataList.Count).ToString(); // Next() has an exclusive upper bound
                Token token = dataList.Find(t => t.Spelling == stringToFind);
            }
            stopwatch.Stop();
            double linearSearchElapsedTime = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

            stopwatch.Reset();

            // Second method: Make a binary search, over the (note!) sorted list. Requires
            // a custom comparer for indentifying tokens based on their spelling (in this case).
            // Here, the sorting (required for using binary search) is included in
            // the runtime calculation.
            TokenComparer tokenComparer = new TokenComparer();
            stopwatch.Start();

            for (int jj = 0; jj < numberOfCalls; jj++)
            {
                string stringToFind = randomNumberGenerator.Next(0, dataList.Count).ToString(); // Next() has an exclusive upper bound
                Token dummyToken = new Token(); // Needed here: The data elements are of type Token, not string ...
                dummyToken.Spelling = stringToFind; // ...but we are really looking for a token whose spelling is a given string.
                int index = sortedDataList.BinarySearch(dummyToken, tokenComparer);
                Token token = sortedDataList[index];
            }
            stopwatch.Stop();
            double binarySearchElapsedTime = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

            stopwatch.Reset();

            // Third method: Ccarry out lookup in the dictionary.
            stopwatch.Start();

            for (int jj = 0; jj < numberOfCalls; jj++)
            {
                string stringToFind = randomNumberGenerator.Next(0, dataList.Count).ToString(); // Next() has an exclusive upper bound
                Token token = dataDictionary[stringToFind];
            }
            stopwatch.Stop();
            double dictionarySearchElapsedTime = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

            string formatString = "0.000000";
            resultsTextBox.Text = "Running times:" + "\r\n";
            resultsTextBox.Text += "Linear search    :  " + linearSearchElapsedTime.ToString(formatString) + " seconds" + "\r\n";
            resultsTextBox.Text += "Binary search    :  " + binarySearchElapsedTime.ToString(formatString) + " seconds" + "\r\n";
            resultsTextBox.Text += "Dictionary search:  " + dictionarySearchElapsedTime.ToString(formatString) + " seconds" + "\r\n";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
