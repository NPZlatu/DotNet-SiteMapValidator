
using System.Diagnostics;
using System.Xml;

class SitmapValidator
{
    private static string _urlString = "http://localhost:8080/v1/common/sitemap-deals";
    private static int _batchSize = 20;
    private static SitemapReqClient _client = new SitemapReqClient();

    /// <summary>
    /// The function `RequestUrlsAsync` asynchronously requests a URL using a `SitemapReqClient` and
    /// returns a `SitemapResponse`.
    /// </summary>
    /// <param name="url">The `url` parameter in the `RequestUrlsAsync` method is a string representing
    /// the URL that will be used to make a request to retrieve a sitemap.</param>
    /// <returns>
    /// A `Task` object representing the asynchronous operation of fetching a sitemap response from the
    /// specified URL.
    /// </returns>
    private static async Task<SitemapResponse> RequestUrlsAsync(string url)
    {
        return await _client.Get(url);
    }

    /// <summary>
    /// The function `LoadUrlsAsync` asynchronously loads URLs from an XML document using XmlReader.
    /// </summary>
    /// <returns>
    /// A `Task<List<string>>` is being returned. This method asynchronously loads URLs from an XML
    /// source and returns a list of strings containing the URLs.
    /// </returns>
    private static async Task<List<string>> LoadUrlsAsync()
    {
        var urls = new List<string>();
        using (var reader = XmlReader.Create(_urlString, new XmlReaderSettings { Async = true }))
        {
            while (await reader.ReadAsync())
            {
                if (reader.Name.Equals("loc"))
                {
                    await reader.ReadAsync();
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        urls.Add(reader.Value);
                    }
                }
            }
        }
        return urls;
    }

    /// <summary>
    /// The function `GetBatches` takes a list of URLs and a batch size, and returns batches of URLs
    /// based on the specified batch size.
    /// </summary>
    /// <param name="urls">The `urls` parameter is a list of strings containing the URLs that you want to
    /// batch process.</param>
    /// <param name="batchSize">The `batchSize` parameter in the `GetBatches` method specifies the number
    /// of elements that should be included in each batch when splitting a list of URLs into
    /// batches.</param>
    private static IEnumerable<List<string>> GetBatches(List<string> urls, int batchSize)
    {
        for (int i = 0; i < urls.Count; i += batchSize)
        {
            yield return urls.Skip(i).Take(batchSize).ToList();
        }
    }
    /// <summary>
    /// The Main function asynchronously loads URLs, requests them in batches, processes the responses,
    /// and logs the execution time.
    /// </summary>
    public static async Task Main()
    {
        var watch = Stopwatch.StartNew();
        var urls = await LoadUrlsAsync();
        var output = new Output();

        foreach (var batch in GetBatches(urls, _batchSize))
        {
            var tasks = batch.Select(url => RequestUrlsAsync(url));
            SitemapResponse[] responses = await Task.WhenAll(tasks);

            foreach (var response in responses)
            {
                if (response.Status == SitemapReqClient.NOT_OK)
                {
                    output.AddRecord(response);
                }
                output.Log(response);
            }
        }

        watch.Stop();
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                watch.Elapsed.Hours,
                                                watch.Elapsed.Minutes,
                                                watch.Elapsed.Seconds
                                            );
        output.Log($"Total Execution Time: {formattedTime} seconds");
    }


}




