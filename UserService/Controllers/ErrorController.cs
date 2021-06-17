using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserService.Models.Exceptions;

namespace UserMicroservice.Controllers {

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase {
        [Route("error")]
        public async void Error() {
            Exception exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            int code = (int)HttpStatusCode.InternalServerError;

            if (exception is NotFoundException)
                code = (int)HttpStatusCode.NotFound;

            else if (exception is BadRequestException)
                code = (int)HttpStatusCode.BadRequest;

            Response.ContentType = "application/json";
            Response.StatusCode = code;
            var err = new {
                error = new[] { exception.Message },
                stackTrace = exception.StackTrace
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(err)).ConfigureAwait(false);
            await Response.Body.FlushAsync();
        }
    }
}