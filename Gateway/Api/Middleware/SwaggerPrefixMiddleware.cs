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

            // só intercepta o swagger.json vindo dos microserviços
            if (path.Contains("/vendas/swagger") || path.Contains("/estoque/swagger"))
            {
                // captura saída original
                var originalBody = context.Response.Body;
                using var newBody = new MemoryStream();
                context.Response.Body = newBody;

                await _next(context);

                // lê JSON original
                newBody.Seek(0, SeekOrigin.Begin);
                var json = await new StreamReader(newBody).ReadToEndAsync();

                // deser
                var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                string prefix = path.Contains("/vendas") ? "/vendas" : "/estoque";

                JsonElement componentsElement;
                if (!root.TryGetProperty("components", out componentsElement))
                {
                    componentsElement = JsonDocument.Parse("{}").RootElement;
                }

                // monta servers
                var newSwagger = new
                {
                    openapi = root.GetProperty("openapi").GetString(),
                    info = root.GetProperty("info"),
                    paths = root.GetProperty("paths"),
                    components = componentsElement,
                    servers = new[]
                    {
                    new { url = $"{context.Request.Scheme}://{context.Request.Host}{prefix}" }
                }
                };

                var newJson = JsonSerializer.Serialize(newSwagger);

                // devolve reescrito
                context.Response.Body = originalBody;
                await context.Response.WriteAsync(newJson, Encoding.UTF8);

                return;
            }

            await _next(context);
        }
    }
}
