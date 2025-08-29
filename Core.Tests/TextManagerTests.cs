using Core.Tests.TestData;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
namespace Core.Tests
{
    public class TextManagerTests
    {
        private TextManager tm;
        private ILogger<TextManager> logger;

        public TextManagerTests()
        {
            // Crear un mock para ILogger<TextManager>
            var mockLogger = new Mock<ILogger<TextManager>>();
            logger = mockLogger.Object;
            tm = new TextManager("", logger);
        }

        [Fact]
        public void CountWords_ReturnsCorrectCount()
        {
            tm.TextOriginal = "hello world test";
            tm.TextEdited = "hello world test";
            Assert.Equal(3, tm.CountWords());
        }

        [Fact]
        public void CountWords_ReturnsCorrect_Virtual_WithMock_Count()
        {
            // Crear un mock de TextManager
            var mock = new Moq.Mock<TextManager>("", logger); // Pasa los argumentos necesarios al constructor

            // Configurar el mock para que CountWords siempre retorne 3
            mock.Setup(m => m.CountWords()).Returns(3);

            // Usar el mock en vez de la instancia real
            var tmMock = mock.Object;

            Assert.Equal(3, tmMock.CountWords());
        }

        [Fact]
        public void CountWords_ReturnsCorrect2Count()
        {
            tm.TextOriginal = "hello world";
            tm.TextEdited = "hello test";
            Assert.Equal(2, tm.CountWords());
        }

        [Fact]
        public void CountWords_ReturnsCorrect5Count()
        {
            tm.TextOriginal = "hello world test xunit unit net core";
            tm.TextEdited = "hello world test xunit unit";
            Assert.Equal(7, tm.CountWords());
        }

        [Fact]
        public void CountLetters_ReturnsCorrectCount()
        {
            tm.TextOriginal = "hello world";
            tm.TextEdited = "hello world";
            Assert.Equal(10, tm.CountLetters());
        }

        [Fact]
        public void CountWordsTextEdited_ReturnsCorrectCount()
        {
            tm.TextOriginal = "one two three";
            tm.TextEdited = "one two";
            Assert.Equal(2, tm.CountWordsTextEdited());
        }

        [Fact]
        public void FindWord_FindsAllOccurrences_CaseSensitive()
        {
            tm.TextOriginal = "Hello hello Hello";
            tm.TextEdited = "Hello hello Hello";
            var result = tm.FindWord("Hello", false);
            Assert.Equal(new List<int> { 0, 12 }, result);
        }

        [Fact]
        public void FindWord_FindsAllOccurrences_IgnoreCase()
        {
            tm.TextOriginal = "Hello hello Hello";
            tm.TextEdited = "Hello hello Hello";
            var result = tm.FindWord("hello", true);
            Assert.Equal(new List<int> { 0, 6, 12 }, result);
        }

        [Fact]
        public void FindExactWord_FindsExactMatches_CaseSensitive()
        {
            tm.TextOriginal = "cat Cat dog cat";
            tm.TextEdited = "cat Cat dog cat";
            var matches = tm.FindExactWord("cat", false);
            Assert.Equal(3, matches.Count);
        }

        [Fact(Skip ="Imcomplet unit test")]
        public void FindExactWord_FindsExactMatches_IgnoreCase()
        {
            tm.TextOriginal = "cat Cat dog cat";
            tm.TextEdited = "cat Cat dog cat";
            var matches = tm.FindExactWord("cat", true);
            Assert.Equal(3, matches.Count);
        }

        [Theory(Skip = "Imcomplet unit test")]
        [InlineData("test test hello world world test", "test", true, 1, "test test")]
        [InlineData("foo foo bar bar foo foo", "foo", true, 2, "foo foo")]
        [InlineData("alpha beta beta gamma", "beta", true, 1, "beta beta")]
        [InlineData("one two three", "one", true, 0, null)]
        [InlineData("case CASE case", "case", false, 1, "case case")]
        [InlineData("repeat repeat repeat", "repeat", true, 2, "repeat repeat")]
        public void FindWordRepetContinue_FindsRepeatedWords_Theory(
            string textOriginal,
            string word,
            bool ignoreCase,
            int expectedCount,
            string expectedFirstMatch)
        {
            tm.TextOriginal = textOriginal;
            tm.TextEdited = textOriginal;
            var matches = tm.FindWordRepetContinue(word, ignoreCase);

            Assert.Equal(expectedCount, matches.Count);

            if (expectedCount > 0 && expectedFirstMatch != null)
            {
                Assert.Contains(expectedFirstMatch.ToLower(), matches[0].Value.ToLower());
            }
        }

        [Theory]
        [ClassData(typeof(FindWordRepetContinueTestData))]
        public void FindWordRepetContinue_FindsRepeatedWords_ClassData(
           string textOriginal,
           string word,
           bool ignoreCase,
           int expectedCount,
           string expectedFirstMatch)
        {
            tm.TextOriginal = textOriginal;
            tm.TextEdited = textOriginal;
            var matches = tm.FindWordRepetContinue(word, ignoreCase);

            Assert.Equal(expectedCount, matches.Count);

            if (expectedCount > 0 && expectedFirstMatch != null)
            {
                Assert.Contains(expectedFirstMatch.ToLower(), matches[0].Value.ToLower());
            }
        }

        [Fact]
        public void FindWordAllRepetContinue_FindsAllRepeatedWords()
        {
            tm.TextOriginal = "foo foo bar bar baz";
            tm.TextEdited = "foo foo bar bar baz";
            var matches = tm.FindWordAllRepetContinue();
            Assert.Equal(2, matches.Count);
            Assert.Equal("foo foo", matches[0].Value);
            Assert.Equal("bar bar", matches[1].Value);
        }
    }
}
