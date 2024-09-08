namespace FileContentSorter
{
    internal static class FileSorter
    {
        private static string OutputFileName => $"GeneratedOutputFile_{DateTime.Now:yyyyMMdd_HHmmss}.txt";        

        public static void MergeSortedFiles(List<string> tempFiles)
        {
            var readers = tempFiles.Select(f => new StreamReader(f)).ToList();
            var priorityQueue = CreateSortetSetForFileLine();

            using (var outputStream = new StreamWriter(OutputFileName))
            {
                AddSmallestLinesFromEachTempFileToSortedSet(readers, priorityQueue);

                while (priorityQueue.Count > 0)
                {
                    int readerIndex = WriteSmallestLineToOutput(priorityQueue, outputStream);

                    AddToSortedSetNextLine(readers, priorityQueue, readerIndex);
                }
            }

            foreach (var reader in readers)
            {
                reader.Dispose();
            }
        }

        private static int WriteSmallestLineToOutput(SortedSet<(FileLine fileLine, int readerIndex)> priorityQueue, StreamWriter outputStream)
        {
            var (smallestFileLine, readerIndex) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            outputStream.WriteLine(smallestFileLine);

            return readerIndex;
        }

        private static void AddToSortedSetNextLine(List<StreamReader> readers, SortedSet<(FileLine fileLine, int readerIndex)> priorityQueue, int readerIndex)
        {
            if (!readers[readerIndex].EndOfStream)
            {
                var line = readers[readerIndex].ReadLine();
                var fileLine = FileLineParser.ParseFileLine(line);
                priorityQueue.Add((fileLine, readerIndex));
            }
        }

        private static void AddSmallestLinesFromEachTempFileToSortedSet(List<StreamReader> readers, SortedSet<(FileLine fileLine, int readerIndex)> priorityQueue)
        {
            for (int i = 0; i < readers.Count; i++)
            {
                if (!readers[i].EndOfStream)
                {
                    var line = readers[i].ReadLine();
                    var fileLine = FileLineParser.ParseFileLine(line);
                    priorityQueue.Add((fileLine, i));
                }
            }
        }

        private static SortedSet<(FileLine fileLine, int readerIndex)> CreateSortetSetForFileLine()
        {
            return new SortedSet<(FileLine fileLine, int readerIndex)>(
                Comparer<(FileLine fileLine, int readerIndex)>.Create((a, b) =>
                {
                    int compare = a.fileLine.CompareTo(b.fileLine);
                    return compare != 0 ? compare : a.readerIndex.CompareTo(b.readerIndex);
                })
            );
        }
    }
}