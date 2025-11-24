using System.Text;
using System.Text.Json;

namespace Api.Middleware
{
    public class SwaggerPrefixMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerPrefixMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // Intercepta apenas swagger.json dos microserviços
            if (path.Contains("/vendas/swagger") || path.Contains("/estoque/swagger"))
            {
                // Captura saída original
                var originalBody = context.Response.Body;
                using var newBody = new MemoryStream();
                context.Response.Body = newBody;

                await _next(context);

                // Lê JSON original
                newBody.Seek(0, SeekOrigin.Begin);
                var json = await new StreamReader(newBody).ReadToEndAsync();

                var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                string prefix = path.Contains("/vendas") ? "/vendas" : "/estoque";

                // Copia components (incluindo securitySchemes)
                JsonElement components = root.TryGetProperty("components", out var c)
                    ? c
                    : JsonDocument.Parse("{}").RootElement;

                // Copia security se existir
                JsonElement security = root.TryGetProperty("security", out var s)
                    ? s
                    : JsonDocument.Parse("[]").RootElement;

                // Monta swagger reescrito
                var newSwagger = new Dictionary<string, object?>
                {
                    ["openapi"] = root.GetProperty("openapi").GetString(),
                    ["info"] = root.GetProperty("info"),
                    ["paths"] = root.GetProperty("paths"),
                    ["components"] = components,
                    ["security"] = security,
                    ["servers"] = new[]
                    {
                        new { url = $"{context.Request.Scheme}://{context.Request.Host}{prefix}" }
                    }
                };

                string newJson = JsonSerializer.Serialize(newSwagger, _jsonOptions);

                context.Response.Body = originalBody;
                await context.Response.WriteAsync(newJson, Encoding.UTF8);

                return;
            }

            await _next(context);
        }

        private static readonly JsonSerializerOptions _jsonOptions = new ()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
