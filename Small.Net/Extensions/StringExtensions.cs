using System.Linq;
using System.Text;

namespace Small.Net.Extensions
{
    public static class StringExtensions
    {
        public static string Repeat(this string value, int count)
        {
            return count <= 0
                ? string.Empty
                : Enumerable.Range(0, count).Aggregate(new StringBuilder(), (p, i) => p.Append(value)).ToString();
        }
    }
}