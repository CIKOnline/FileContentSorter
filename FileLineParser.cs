namespace FileContentSorter
{
    internal static class FileLineParser
    {
        public static FileLine ParseFileLine(string fileLineValue) => TryParseLine(fileLineValue, out FileLine fileLine)
                ? fileLine
                : throw new ArgumentException("Incorrect file line format!");

        private static bool TryParseLine(string line, out FileLine lineObject)
        {
            lineObject = null;

            string[] parts = line.Split('.', 2);
            if (parts.Length != 2)
            {
                return false;
            }

            if (int.TryParse(parts[0].Trim(), out int number))
            {
                string text = parts[1].Trim();
                lineObject = new FileLine(number, text);
                return true;
            }

            return false;
        }
    }
}