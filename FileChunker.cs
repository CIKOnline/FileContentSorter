using System.Collections.Concurrent;

namespace FileContentSorter
{
    internal static class FileChunker
    {
        private const int LinesPerChunk = 100_000;
        private static ParallelOptions ParallelOptions => new() { MaxDegreeOfParallelism = Environment.ProcessorCount };

        public static ConcurrentBag<string> SplitlargeFileToSmallerSortedTempFiles(string filePath)
        {
            var tempFiles = new ConcurrentBag<string>();

            using (var fileStream = new StreamReader(filePath))
            {
                
                Parallel.ForEach(
                    GenerateLineChunks(fileStream, LinesPerChunk),
                    ParallelOptions,
                    chunk =>
                    {
                        chunk.Sort();
                        string tempFile = Path.GetTempFileName();
                        tempFiles.Add(tempFile);
                        WriteChunkToTempFile(chunk, tempFile);
                    });
            }

            return tempFiles;
        }

        public static void DeleteTempFiles(ConcurrentBag<string> tempFiles)
        {
            foreach (var tempFile in tempFiles)
            {
                File.Delete(tempFile);
            }
        }

        private static IEnumerable<List<FileLine>> GenerateLineChunks(StreamReader fileStream, int linesPerChunk)
        {
            while (!fileStream.EndOfStream)
            {
                var chunk = new List<FileLine>();

                for (int i = 0; i < linesPerChunk && !fileStream.EndOfStream; i++)
                {
                    var line = fileStream.ReadLine();
                    if (line == null) continue;

                    var fileLine = FileLineParser.ParseFileLine(line);
                    chunk.Add(fileLine);
                }

                yield return chunk;
            }
        }

        private static void WriteChunkToTempFile(List<FileLine> chunk, string tempFile)
        {
            using (var writer = new StreamWriter(tempFile))
            {
                foreach (var fileLine in chunk)
                {
                    writer.WriteLine(fileLine);
                }
            }
        }
    }
}