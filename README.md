# SitemapValidator

**SitemapValidator** is a C# console application that validates URLs from a sitemap by making asynchronous HTTP requests to each URL, processing the responses, and logging the results. The application uses **SemaphoreSlim** to manage concurrency and optimize the request process without relying on batch processing.

## Features

- **Asynchronous URL Requests**: Fetch URLs from a sitemap asynchronously.
- **Concurrency Control with SemaphoreSlim**: Manages the number of concurrent requests, ensuring efficient resource usage and preventing overload.
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

To rebuild the entire solution (including both the project and tests), run the following command in the **Project** folder:

```bash
cd Project
dotnet build
```

### Running the Project:

To run the project, execute the following command from the **Project** folder:

```bash
dotnet run
```

### Running the Tests:

To run the unit tests, navigate to the **Tests** folder and execute:

```bash
cd Tests
dotnet test
```

## How it Works

### 1. **RequestUrlsAsync(string url)**

This method asynchronously sends an HTTP GET request to the given `url` and returns a `SitemapResponse` object, which contains the response status, status code, and any associated error message.

### 2. **LoadUrlsAsync()**

This method asynchronously loads URLs from an XML sitemap using `XmlReader`. It looks for `<loc>` nodes in the XML document and extracts the URLs.

### 3. **Main()**

The `Main` method orchestrates the process by:

- Loading the URLs.
- Requesting URLs asynchronously with concurrency controlled by **SemaphoreSlim**.
- Processing the responses.
- Logging the status of each URL.
- Logging the total execution time.

### 4. **Concurrency Control with SemaphoreSlim**

Instead of batch processing, the application now uses **SemaphoreSlim** to limit the number of concurrent HTTP requests. This ensures that the program doesn't overwhelm the server or run out of resources while still maintaining efficiency in processing a large number of URLs.

Here’s how it works:

- **SemaphoreSlim** is initialized with a maximum concurrency level (e.g., 20 concurrent requests).
- Each request asynchronously waits for a slot to become available before executing.
- As soon as a request completes and releases its slot, another request is initiated.

```csharp
private static int _concurrencyLimit = 20; // Maximum number of concurrent requests
private static SemaphoreSlim _semaphore = new SemaphoreSlim(_concurrencyLimit);
```

## Configuration

The following static variables can be configured:

- `_urlString`: The URL of the sitemap to be processed. Default value is `http://localhost:8080/v1/common/sitemap-deals`.
- `_concurrencyLimit`: The maximum number of concurrent URLs to process at once. Default value is `20`.

### Example:

```csharp
private static string _urlString = "http://example.com/sitemap.xml";
private static int _concurrencyLimit = 10;
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
