using System.Collections.Generic;
using System.IO;

namespace FileChopper.FileProcessor
{
    public class FileSplitter
    {
        public void SplitFile(string sourceFilePath, string outputDirectory, int numberOfLinesPerOutputFile)
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            else
            {
                var dir = new DirectoryInfo(outputDirectory);
                foreach (var fi in dir.GetFiles())
                {
                    fi.Delete();
                }
            }

            using (var lineIterator = File.ReadLines(sourceFilePath).GetEnumerator())
            {
                var stillGoing = true;
                for (var chunk = 0; stillGoing; chunk++)
                {
                    stillGoing = WriteChunk(lineIterator, numberOfLinesPerOutputFile, chunk, outputDirectory);
                }
            }
        }

        private static bool WriteChunk(IEnumerator<string> lineIterator, int splitSize, int chunk, string outputDirectory)
        {
            var outputFilePath = outputDirectory + "\\" + chunk + ".txt";

            using (var writer = File.CreateText(outputFilePath))
            {
                for (int i = 0; i < splitSize; i++)
                {
                    if (!lineIterator.MoveNext())
                    {
                        return false;
                    }
                    writer.WriteLine(lineIterator.Current);
                }
            }
            return true;
        }
    }
}
