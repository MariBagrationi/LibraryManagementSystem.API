namespace LibraryManagementSystem.API.Infrastructure.Middlewares
{
    public class RequestResponseLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLogger> _logger;

        public RequestResponseLogger(RequestDelegate next, ILogger<RequestResponseLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context.Request);
            await _next(context); 
            LogResponse(context.Response);
        }

        private async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering(); 
            string body = string.Empty;

            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            var log = $"[Request] {DateTime.Now}{Environment.NewLine}" +
                      $"IP={request.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
                      $"Method={request.Method}{Environment.NewLine}" +
                      $"Path={request.Path}{Environment.NewLine}" +
                      $"IsSecured={request.IsHttps}{Environment.NewLine}" +
                      $"QueryString={request.QueryString}{Environment.NewLine}" +
                      $"Body={body}{Environment.NewLine}";

            _logger.LogInformation(log);
        }

        private void LogResponse(HttpResponse response)
        {
            var log = $"[Response] {DateTime.Now}{Environment.NewLine}" +
                      $"Status Code={response.StatusCode}{Environment.NewLine}";

            _logger.LogInformation(log);
        }
    }
}
