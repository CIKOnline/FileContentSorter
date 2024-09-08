namespace FileContentSorter
{
    internal sealed class FileLine(int number, string text) : IComparable<FileLine>
    {
        public int Number { get; set; } = number;
        public string Text { get; set; } = text;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FileLine other = (FileLine)obj;
            return Text.Equals(other.Text, StringComparison.OrdinalIgnoreCase) && Number == other.Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text.ToLower(), Number);
        }

        public int CompareTo(FileLine other)
        {
            if (other == null) return 1;

            int textComparison = string.Compare(Text, other.Text, StringComparison.Ordinal);
            if (textComparison == 0)
            {
                return Number.CompareTo(other.Number);
            }
            return textComparison;
        }

        public override string ToString()
        {
            return $"{Number}. {Text}";
        }
    }
}