using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // allow us to send the custom 'Application-Error' header
            // (query appeared (in Postman) to still expose application-error without being allowed !? ... but perhaps not available in agnular ?)
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Application-Error", message);
            // specify that all URIs may access response i.e. CORS / cross domain
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}