namespace DevSpot.Middleware
{
    public class CsrfMiddleware
    {
        private readonly RequestDelegate _next;

        public CsrfMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            // Skip CSRF check for login & register
            if (context.Request.Path.StartsWithSegments("/api/Auth/Login") ||
                context.Request.Path.StartsWithSegments("/api/Auth/Register"))
            {
                await _next(context);
                return;
            }

            if (HttpMethods.IsPost(context.Request.Method) ||
                HttpMethods.IsPut(context.Request.Method) ||
                HttpMethods.IsDelete(context.Request.Method))
            {
                var csrfHeader = context.Request.Headers["X-CSRF-TOKEN"].FirstOrDefault();
                var csrfCookie = context.Request.Cookies["XSRF-TOKEN"];

                if (string.IsNullOrEmpty(csrfHeader) || csrfHeader != csrfCookie)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("CSRF token invalid");
                    return;
                }
            }

            await _next(context);
        }
    }

}
