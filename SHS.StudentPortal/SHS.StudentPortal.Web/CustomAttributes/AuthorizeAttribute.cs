//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace SHS.StudentPortal.Web.CustomAttributes;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class AuthorizeAttribute : Attribute, IAuthorizationFilter
//{
//    IConfiguration _configuration { get; set; }

//    public void OnAuthorization(AuthorizationFilterContext filterContext)
//    {
//        // Skip authorization if method or class is decorated with
//        // [AllowAnonymous] attribute
//        if (filterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
//            return;

//        if(filterContext.HttpContext.User.Claims.Count() == 0 || filterContext.HttpContext.User is null)
//        {
//            filterContext.
//        }
//        // Authorize by pulling jwt bearer
//        var jwt = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
//        if (string.IsNullOrWhiteSpace(jwt))
//        {
//            filterContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//            filterContext.Result = new JsonResult(ComposeUnauthorizedError());
//        }
//        else
//        {
//            // This is not advised but we're on a tight schedule
//            var dependencyScope = filterContext.HttpContext.RequestServices;
//            _jwtAuthProvider = dependencyScope.GetService(typeof(IJwtAuthProvider)) as IJwtAuthProvider;
//            _configuration = dependencyScope.GetService(typeof(IConfiguration)) as IConfiguration;

//            if (_jwtAuthProvider is null || _configuration is null)
//                throw new ArgumentNullException($"{nameof(OnAuthorization)}:NullServices");
//            else
//            {
//                var principal = _jwtAuthProvider.ValidateJWT(jwt);

//                if (principal is null)
//                {
//                    filterContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                    filterContext.Result = new JsonResult(ComposeUnauthorizedError("Invalid Token"));
//                }
//                else
//                {
//                    // Validate expiration date
//                    var expDateLong = long.Parse(principal.Claims.FirstOrDefault(c => c.Type == "exp").Value);

//                    DateTime expDate = DateTimeOffset.FromUnixTimeSeconds(expDateLong).DateTime.ToUniversalTime();

//                    if (expDate < DateTime.UtcNow)
//                    {
//                        filterContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                        filterContext.Result = new JsonResult(ComposeUnauthorizedError("Session Expired"));

//                        return;
//                    }

//                    // Validate audience & issuer
//                    if (principal.Claims.FirstOrDefault(c => c.Type == "iss").Value != _configuration["JWTAuth:Issuer"] ||
//                       principal.Claims.FirstOrDefault(c => c.Type == "aud").Value != _configuration["JWTAuth:Issuer"])
//                    {
//                        filterContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                        filterContext.Result = new JsonResult(ComposeUnauthorizedError("Invalid Token"));

//                        return;
//                    }

//                    // Set the principal
//                    filterContext.HttpContext.User = principal;
//                }
//            }
//        }
//    }

//    //JsonResponseModel ComposeUnauthorizedError(string? message = null)
//    //{
//    //    string unauthorizedStr = message ?? "You don't have permission accessing the resource";

//    //    // Initialize new anonymous object (based from Error model in Domain project - Cannot ref from Domain project hence anonymous type)
//    //    var error = new
//    //    {
//    //        type = nameof(UnauthorizedAccessException),
//    //        message = unauthorizedStr
//    //    };
//    //    // Initialize new error jsonresponsemodel
//    //    var jsonResponse = new JsonResponseModel().Error(error, status: StatusCodes.Status401Unauthorized);

//    //    return jsonResponse;
//    //}
//}
