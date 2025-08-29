using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    /// <summary>
    /// Provides text management utilities such as word and letter counting, word finding, and repeated word detection.
    /// </summary>
    public class TextManager
    {
        ILogger<TextManager> _logger;


        /// <summary>
        /// Gets or sets the original text.
        /// </summary>
        public string TextOriginal { get; set; }

        /// <summary>
        /// Gets or sets the edited text.
        /// </summary>
        public string TextEdited { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextManager"/> class with the specified text.
        /// </summary>
        /// <param name="strText">The text to manage.</param>
        public TextManager(string strText, ILogger<TextManager> logger)
        {
            this.TextOriginal = strText;
            this.TextEdited = strText;
            _logger = logger;
        }

        /// <summary>
        /// Counts the number of words in the original text.
        /// </summary>
        /// <returns>The number of words in <see cref="TextOriginal"/>.</returns>
        public virtual int CountWords()
        {
            int intCount = 0;
            string[] arrayTextSplit = this.TextOriginal.Split(' ');

            foreach (var item in arrayTextSplit)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    intCount++;
                }
            }

            _logger.LogInformation("Result is: " +  intCount);

            return intCount;
        }

        /// <summary>
        /// Counts the number of letters in the original text, excluding spaces.
        /// </summary>
        /// <returns>The number of letters in <see cref="TextOriginal"/>.</returns>
        public int CountLetters()
        {
            int intCount = 0;
            string[] arrayTextSplit = this.TextOriginal.Split(' ');

            foreach (var item in arrayTextSplit)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    intCount = intCount + item.Length;
                }
            }

            return intCount;
        }

        /// <summary>
        /// Counts the number of words in the edited text.
        /// </summary>
        /// <returns>The number of words in <see cref="TextEdited"/>.</returns>
        public int CountWordsTextEdited()
        {
            int intCount = 0;
            string[] arrayTextSplit = this.TextEdited.Split(' ');

            foreach (var item in arrayTextSplit)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    intCount++;
                }
            }

            return intCount;
        }

        /// <summary>
        /// Finds all occurrences of a word in the original text and returns their starting indices.
        /// </summary>
        /// <param name="strWord">The word to find.</param>
        /// <param name="bolUpperLowerCase">If true, ignores case when searching.</param>
        /// <returns>A list of starting indices where the word is found.</returns>
        public List<int> FindWord(string strWord, bool bolUpperLowerCase)
        {
            List<int> lstFindResult = new List<int>();
            string textToFind = this.TextOriginal;

            if (bolUpperLowerCase)
            {
                textToFind = this.TextOriginal.ToLower();
                strWord = strWord.ToLower();
            }

            int i = 0;
            while ((i = textToFind.IndexOf(strWord, i)) != -1)
            {
                lstFindResult.Add(i);
                i++;
            }

            return lstFindResult;
        }

        /// <summary>
        /// Finds all exact matches of a word in the original text using regular expressions.
        /// </summary>
        /// <param name="strWord">The word to find.</param>
        /// <param name="bolIgnoreUppercaseLowercase">If true, ignores case when searching.</param>
        /// <returns>A list of <see cref="Match"/> objects representing the found words.</returns>
        public List<Match> FindExactWord(string strWord, bool bolIgnoreUppercaseLowercase)
        {
            Regex rx = new Regex(@"\b(?<word>\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<Match> lstmatch = new List<Match>();
            MatchCollection matches = rx.Matches(this.TextOriginal);

            foreach (Match item in matches)
            {
                if (bolIgnoreUppercaseLowercase)
                {
                    string strTempValue = item.Value.ToLower();
                    if (strTempValue.ToLower() == strWord.ToLower())
                    {
                        lstmatch.Add(item);
                    }
                }
                else
                {
                    if (item.Value.ToLower() == strWord.ToLower())
                    {
                        lstmatch.Add(item);
                    }
                }
            }

            return lstmatch;
        }

        /// <summary>
        /// Finds all occurrences of consecutive repeated words in the original text that contain the specified word.
        /// </summary>
        /// <param name="strWord">The word to search for in repeated sequences.</param>
        /// <param name="bolIgnoreUppercaseLowercase">If true, ignores case when searching.</param>
        /// <returns>A list of <see cref="Match"/> objects representing the repeated words found.</returns>
        public List<Match> FindWordRepetContinue(string strWord, bool bolIgnoreUppercaseLowercase)
        {
            Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<Match> lstmatch = new List<Match>();
            MatchCollection matches = rx.Matches(this.TextOriginal);

            foreach (Match item in matches)
            {
                if (bolIgnoreUppercaseLowercase)
                {
                    string strTempValue = item.Value.ToLower();
                    if (strTempValue.ToLower().Contains(strWord.ToLower()))
                    {
                        lstmatch.Add(item);
                    }
                }
                else
                {
                    if (item.Value.ToLower().Contains(strWord.ToLower()))
                    {
                        lstmatch.Add(item);
                    }
                }
            }

            return lstmatch;
        }

        /// <summary>
        /// Finds all occurrences of consecutive repeated words in the original text.
        /// </summary>
        /// <returns>A <see cref="MatchCollection"/> of all repeated word sequences found.</returns>
        public MatchCollection FindWordAllRepetContinue()
        {
            Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(this.TextOriginal);
            return matches;
        }
    }
}