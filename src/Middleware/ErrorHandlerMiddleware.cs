using Backend_Teamwork.src.Utils;

namespace Backend_Teamwork.src.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next){
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }catch(CustomException ex){
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(ex.Message);
            }   
        }
    }
}