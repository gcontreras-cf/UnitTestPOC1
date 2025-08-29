using System.Collections;
namespace Core.Tests.TestData
{
    public class FindWordRepetContinueTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "test test hello world world test", "test", true, 1, "test test" };
            yield return new object[] { "foo foo bar bar foo foo", "foo", true, 2, "foo foo" };
            yield return new object[] { "alpha beta beta gamma", "beta", true, 1, "beta beta" };
            yield return new object[] { "one two three", "one", true, 0, null };
            yield return new object[] { "case CASE case", "case", false, 1, "case case" };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
