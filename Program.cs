using FileContentSorter;

if (args.Length == 0)
{
    Console.WriteLine("Please provide the file path as a command-line argument.");
    Console.WriteLine("Press any key.");
    Console.ReadKey();
    return;
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    Console.WriteLine($"The file '{filePath}' does not exist.");
    Console.WriteLine("Press any key.");
    Console.ReadKey();
    return;
}

Console.WriteLine("Starting splitting file to smaller chunks.");
var tempFiles = FileChunker.SplitlargeFileToSmallerSortedTempFiles(filePath);

Console.WriteLine("Starting merging files.");
FileSorter.MergeSortedFiles([.. tempFiles]);

Console.WriteLine("Starting deleting temporary files.");
FileChunker.DeleteTempFiles(tempFiles);

Console.WriteLine("Sorting is finished!");
Console.WriteLine("Press any key.");
Console.ReadKey();