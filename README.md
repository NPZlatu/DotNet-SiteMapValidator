# SitemapValidator

This is a C# console application that validates URLs from a sitemap by making asynchronous HTTP requests to each URL, processing the responses, and logging the results. The application also splits the URLs into batches to optimize the request process.

## Features
- **Asynchronous URL Requests**: Fetch URLs from a sitemap asynchronously.
- **Batch Processing**: URLs are processed in batches for optimized performance.
- **Logging**: Logs the results of each request, including any errors.
- **Execution Time Logging**: Logs the total execution time for processing all URLs.

## Prerequisites
- .NET Core SDK 9.
- Basic knowledge of asynchronous programming in C#.

## Setup & Installation

### Clone the repository:
```bash
git clone https://github.com/NPZlatu/SitemapValidator.git
cd SitemapValidator
```

### Restore dependencies:
```bash
dotnet restore
```

### Build the project:
```bash
dotnet build
```

### Run the application:
```bash
dotnet run
```

## How it Works

### 1. **RequestUrlsAsync(string url)**
This method asynchronously sends an HTTP GET request to the given `url` and returns a `SitemapResponse` object, which contains the response status, status code, and any associated error message.

### 2. **LoadUrlsAsync()**
This method asynchronously loads URLs from an XML sitemap using `XmlReader`. It looks for `<loc>` nodes in the XML document and extracts the URLs.

### 3. **GetBatches(List<string> urls, int batchSize)**
This method splits the list of URLs into smaller batches of the specified size. The `yield return` statement ensures that the batches are returned lazily, improving memory usage.

### 4. **Main()**
The `Main` method orchestrates the process by:
- Loading the URLs.
- Requesting URLs in batches.
- Processing the responses.
- Logging the status of each URL.
- Logging the total execution time.

## Configuration

The following static variables can be configured:
- `_urlString`: The URL of the sitemap to be processed. Default value is `http://localhost:8080/v1/common/sitemap-deals`.
- `_batchSize`: The number of URLs to be processed in each batch. Default value is `20`.

### Example:
```csharp
private static string _urlString = "http://example.com/sitemap.xml";
private static int _batchSize = 10;
```

## Output

The program will log the following:
- The status of each URL (OK or NOT_OK).
- Any error messages if the HTTP request fails.
- The total execution time of the entire URL validation process.

### Example output:
```
[INFO] URL: http://example.com/page1 - Status: OK
[INFO] URL: http://example.com/page2 - Status: NOT_OK - Error: Timeout
[INFO] Total Execution Time: 00:03:15
```

## SitemapResponse

The `SitemapResponse` structure holds the response data for each URL:
- `URL`: The URL of the page.
- `Status`: The status of the request (either "OK" or "NOTOK").
- `StatusCode`: The HTTP status code of the response.
- `Message`: Any error message or additional information about the request.

## SitemapReqClient

The `SitemapReqClient` class is responsible for making HTTP requests to the specified URLs. It uses `HttpClient` to send asynchronous GET requests and returns a `SitemapResponse` object with the result.

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details.
