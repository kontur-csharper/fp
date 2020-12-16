using System;
using System.IO;
using System.Linq;
using System.Security;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public class TxtTextReader : ITextReader
    {
        public Result<string[]> ReadStrings(string pathToFile)
        {
            try
            {
                return File.ReadLines(pathToFile).ToArray();
            }
            catch (PathTooLongException)
            {
                return Result.Fail<string[]>("Too long path: " + pathToFile);
            }
            catch (ArgumentException)
            {
                return Result.Fail<string[]>("Invalid path chars in " + pathToFile);
            }
            catch (UnauthorizedAccessException)
            {
                return Result.Fail<string[]>("you do not have access " + pathToFile);
            }
            catch (DirectoryNotFoundException)
            {
                return Result.Fail<string[]>("Is directory / Directory not found " + pathToFile);
            }
            catch (FileNotFoundException)
            {
                return Result.Fail<string[]>("File not found by path " + pathToFile);
            }
            catch (IOException)
            {
                return Result.Fail<string[]>("Error of ouput when open file by path " + pathToFile);
            }
            catch (SecurityException)
            {
                return Result.Fail<string[]>("You do not have permission to read file " + pathToFile);
            }
            catch (Exception e)
            {
                return Result.Fail<string[]>("Unhandled exception" + e);
            }
        }
    }
}