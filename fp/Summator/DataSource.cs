using System;
using System.IO;

namespace FP
{
    ///<summary>Этот класс нельзя менять. Считайте, что он вам дан в виде бинарной зависимости.</summary>
    public class DataSource : IDisposable
    {
        private readonly StreamReader reader;
        public DataSource(string filename)
        {
            reader = new StreamReader(filename);
        }

        ///<returns>null if no more data</returns>
        public string[] NextRecord()
        {
            var line = reader.ReadLine();
            return line == null ? null : line.Split(' ');
        }

        public void Dispose()
        {
            reader.Close();
        }
    }
}