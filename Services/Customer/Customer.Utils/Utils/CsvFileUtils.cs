namespace Customer.Utils.Utils
{
    public class CsvFileUtils
    {
        public static void Append(string content, string path)
        {
            var line = CustomerCsvParser.ComposeLine(content);
            File.AppendAllText(path, line);
        }
    }
}
