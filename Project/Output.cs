using System.Globalization;
using CsvHelper;

public class Output
{
    private TextWriter _writer;
    private CsvWriter _csvWriter;
    private const string _outputFile = "../output.csv";

    public Output()
    {
        _writer = new StreamWriter(_outputFile, false, System.Text.Encoding.UTF8);
        _csvWriter = new CsvWriter(_writer, CultureInfo.InvariantCulture);
        _csvWriter.WriteHeader<SitemapResponse>();
    }

    public void AddRecord(SitemapResponse res)
    {
        _csvWriter.NextRecord();
        _csvWriter.WriteRecord(res);
    }

    public void Log(SitemapResponse res)
    {
        Util.Dump($"{res.Status}: URL: {res.URL}");
    }

    public void Log(string message)
    {
        Util.Dump(message);
    }

}