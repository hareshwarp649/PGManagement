namespace bca.api.Helpers
{
    public class NormalizedStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (x == null || y == null) return false;

            return Normalize(x) == Normalize(y);
        }

        public int GetHashCode(string obj)
        {
            return Normalize(obj).GetHashCode();
        }

        private string Normalize(string input)
        {
            return new string(input
                .Where(char.IsLetterOrDigit) // Removes punctuation, spaces, symbols
                .ToArray())
                .ToLowerInvariant();         // Case insensitive
        }
    }

}
