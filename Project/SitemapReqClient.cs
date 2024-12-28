public class SitemapReqClient
{
    private HttpClient _httpClient;
    public static string OK = "OK";
    public static string NOT_OK = "NOTOK";

    public SitemapReqClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<SitemapResponse> Get(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            return new SitemapResponse(url, OK, (int)response.StatusCode, "");
        }
        catch (HttpRequestException ex)
        {
            int statusCode = ex.StatusCode != null ? Convert.ToInt32(ex.StatusCode) : 400;
            return new SitemapResponse(url, NOT_OK, statusCode, ex.Message);
        }
    }
}