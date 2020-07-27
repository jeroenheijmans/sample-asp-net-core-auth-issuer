using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleAuthIssuer.Controllers
{
    public class Response
    {
        public string Data { get; set; }
        public DateTimeOffset TimestampUtc => DateTimeOffset.UtcNow;
    }

    [ApiController]
    [Route("foo")]
    public class FooController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<Response> Get() 
            => new Response { Data = "Result from `/` with [AllowAnonymous]" };

        [HttpGet("protected")]
        [Authorize]
        public ActionResult<Response> GetAllowAnonymous()
            => new Response { Data = "Result from `/protected` with [Authorize]" };
    }
}
