
using System.Diagnostics;
using System.Xml;

class SitmapValidator
{
    private static string _urlString = "URL_OF_SITEMAP"; //place the URL of Sitemap here
    private static int _batchSize = 20;
    private static SitemapReqClient _client = new SitemapReqClient();
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(_batchSize); // Allow up to 20 concurrent requests.


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
    /// The Main function asynchronously loads URLs, requests them in batches, processes the responses,
    /// and logs the execution time.
    /// </summary>
    public static async Task Main()
    {
        var watch = Stopwatch.StartNew();
        var urls = await LoadUrlsAsync();
        var output = new Output();

        var tasks = urls.Select(async url =>
        {
            await _semaphore.WaitAsync();

            try
            {
                var response = await RequestUrlsAsync(url);

                if (response.Status == SitemapReqClient.NOT_OK)
                {
                    output.AddRecord(response);
                }
                output.Log(response);
            }
            finally
            {
                _semaphore.Release();
            }

        });

        await Task.WhenAll(tasks);

        watch.Stop();
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                watch.Elapsed.Hours,
                                                watch.Elapsed.Minutes,
                                                watch.Elapsed.Seconds
                                            );
        output.Log($"Total Execution Time: {formattedTime} seconds");
    }


}




