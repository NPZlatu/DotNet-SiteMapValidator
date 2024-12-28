public struct SitemapResponse
{
    public string URL { get; set; }
    public string Status { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }


    public SitemapResponse(string url, string status, int statusCode, string message)
    {
        URL = url;
        Status = status;
        StatusCode = statusCode;
        Message = message;
    }
}
